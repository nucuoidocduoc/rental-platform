using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using booking_service.Core.Application.Interfaces;
using booking_service.Core.Domain.Events;
using booking_service.Infrastructure.Data.EventStore.Schemas;
using System.Text.Json;

namespace booking_service.Infrastructure.Data.EventStore
{
    public class EventRepository : IEventRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _dynamoDb;

        public EventRepository(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDB)
        {
            _dynamoDbContext = dynamoDbContext ?? throw new ArgumentNullException(nameof(dynamoDbContext));
            _dynamoDb = amazonDynamoDB;
        }

        public async Task<T> GetEventAsync<T>(Guid eventId)
        {
            var eventItem = await _dynamoDbContext.LoadAsync<DynamoBookingEvent>(eventId.ToString());
            if (eventItem == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }
            return JsonSerializer.Deserialize<T>(eventItem.EventData) ?? throw new InvalidOperationException("Failed to deserialize event data.");
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId)
        {
            var request = new QueryRequest
            {
                TableName = "BookingEvents",
                KeyConditionExpression = "AggregateId = :aggregateId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":aggregateId", new AttributeValue { S = aggregateId.ToString() } }
                }
            };
            var response = await _dynamoDb.QueryAsync(request);
            return response.Items
                .OrderBy(item => int.Parse(item["Version"].N))
                .Select(item =>
                {
                    var eventData = item["EventData"].S;
                    var eventType = JsonSerializer.Deserialize<JsonElement>(eventData).GetProperty("EventName").GetString();
                    if (eventType == nameof(BookingCreatedEvent))
                    {
                        return JsonSerializer.Deserialize<BookingCreatedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(BookingReservedEvent))
                    {
                        return JsonSerializer.Deserialize<BookingReservedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(BookingPaymentCompletedEvent))
                    {
                        return JsonSerializer.Deserialize<BookingPaymentCompletedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(BookingConfirmedEvent))
                    {
                        return JsonSerializer.Deserialize<BookingConfirmedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(BookingCompletedEvent))
                    {
                        return JsonSerializer.Deserialize<BookingCompletedEvent>(item["EventData"].S) as Event;
                    }
                    else
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();
        }

        public async Task SaveEventAsync<T>(T @event, int expectedVersion = 0) where T : Event
        {
            var aggregateId = @event.BookingId;
            var item = new DynamoBookingEvent
            {
                AggregateId = aggregateId,
                Version = expectedVersion + 1,
                EventData = JsonSerializer.Serialize(@event)
            };

            try
            {
                await _dynamoDb.PutItemAsync(new PutItemRequest
                {
                    TableName = "BookingEvents",
                    Item = item.GetKeyValuePairs(),
                    ConditionExpression = "attribute_not_exists(AggregateId) AND attribute_not_exists(Version)",
                });
                //await _dynamoDbContext.SaveAsync(item);
            }
            catch (ConditionalCheckFailedException)
            {
                throw new Exception("Concurrent update detected.");
            }
        }
    }
}