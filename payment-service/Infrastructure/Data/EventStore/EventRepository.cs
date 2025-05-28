using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using payment_service.Core.Application.Interfaces;
using payment_service.Core.Domain.Events;
using System.Data;
using System.Text.Json;

namespace payment_service.Infrastructure.Data.EventStore
{
    public class EventRepository : IEventStore
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public EventRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task SaveEventAsync(object @event, int expectedVersion)
        {
            var aggregateId = GetAggregateId(@event);
            var version = expectedVersion + 1;
            var item = new Dictionary<string, AttributeValue>
            {
                ["AggregateId"] = new AttributeValue { S = aggregateId.ToString() },
                ["Version"] = new AttributeValue { N = version.ToString() },
                ["EventData"] = new AttributeValue { S = JsonSerializer.Serialize(@event) }
            };
            try
            {
                await _dynamoDb.PutItemAsync(new PutItemRequest
                {
                    TableName = "PaymentEvents",
                    Item = item,
                    ConditionExpression = "attribute_not_exists(AggregateId) AND attribute_not_exists(Version)",
                });
            }
            catch (ConditionalCheckFailedException)
            {
                throw new Exception("Concurrent update detected.");
            }
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId)
        {
            var queryRequest = new QueryRequest
            {
                TableName = "PaymentEvents",
                KeyConditionExpression = "AggregateId = :aggregateId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":aggregateId"] = new AttributeValue { S = aggregateId.ToString() }
                }
            };
            var response = await _dynamoDb.QueryAsync(queryRequest);
            return response.Items
                .OrderBy(item => int.Parse(item["Version"].N))
                .Select(item =>
                {
                    var eventData = item["EventData"].S;
                    var eventType = JsonSerializer.Deserialize<JsonElement>(eventData).GetProperty("EventName").GetString();
                    if (eventType == nameof(PaymentAuthorizedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentAuthorizedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PaymentCapturedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentCapturedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PaymentCreatedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentCreatedEvent>(item["EventData"].S) as Event;
                    }
                    else
                    {
                        return null;
                    }
                })
                .ToList();
        }

        private Guid GetAggregateId(object @event) => @event switch
        {
            PaymentAuthorizedEvent e => e.PaymentId,
            PaymentCapturedEvent e => e.PaymentId,
            PaymentCreatedEvent e => e.PaymentId,
            _ => throw new ArgumentException("Unknown event type")
        };

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            var queryRequest = new ScanRequest
            {
                TableName = "PaymentEvents"
            };
            var response = await _dynamoDb.ScanAsync(queryRequest);
            return response.Items
                .OrderBy(item => int.Parse(item["Version"].N))
                .Select(item =>
                {
                    var eventData = item["EventData"].S;
                    var eventType = JsonSerializer.Deserialize<JsonElement>(eventData).GetProperty("EventName").GetString();
                    if (eventType == nameof(PaymentAuthorizedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentAuthorizedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PaymentCapturedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentCapturedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PaymentCreatedEvent))
                    {
                        return JsonSerializer.Deserialize<PaymentCreatedEvent>(item["EventData"].S) as Event;
                    }
                    else
                    {
                        return null;
                    }
                })
                .ToList();
        }
    }
}