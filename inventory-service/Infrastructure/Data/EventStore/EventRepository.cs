using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using inventory_service.Core.Application.Interfaces;
using inventory_service.Core.Domain.Aggregates;
using inventory_service.Core.Domain.Events;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Data;
using System.Text.Json;

namespace inventory_service.Infrastructure.Data.EventStore
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
                    TableName = "PropertyEvents",
                    Item = item,
                    ConditionExpression = "attribute_not_exists(AggregateId) AND attribute_not_exists(Version)",
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId)
        {
            var queryRequest = new QueryRequest
            {
                TableName = "PropertyEvents",
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
                    if (eventType == nameof(PropertyCreatedEvent))
                    {
                        return JsonSerializer.Deserialize<PropertyCreatedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PropertyBlockedEvent))
                    {
                        return JsonSerializer.Deserialize<PropertyBlockedEvent>(item["EventData"].S) as Event;
                    }
                    else if (eventType == nameof(PropertyConfirmedEvent))
                    {
                        return JsonSerializer.Deserialize<PropertyConfirmedEvent>(item["EventData"].S) as Event;
                    }
                    else
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();
        }

        public async Task<IEnumerable<object>> GetEventsAsync()
        {
            var scanRequest = new ScanRequest
            {
                TableName = "PropertyEvents",
            };
            var response = await _dynamoDb.ScanAsync(scanRequest);
            return response.Items
                .GroupBy(item => item["AggregateId"].S)
                .Select(gr =>
                {
                    var events = gr.ToList()
                    .OrderBy(item => int.Parse(item["Version"].N))
                    .Select(x =>
                    {
                        var eventData = x["EventData"].S;
                        var eventType = JsonSerializer.Deserialize<JsonElement>(eventData).GetProperty("EventName").GetString();
                        if (eventType == nameof(PropertyCreatedEvent))
                        {
                            return JsonSerializer.Deserialize<PropertyCreatedEvent>(eventData) as object;
                        }
                        else if (eventType == nameof(PropertyBlockedEvent))
                        {
                            return JsonSerializer.Deserialize<PropertyBlockedEvent>(eventData) as object;
                        }
                        else if (eventType == nameof(PropertyConfirmedEvent))
                        {
                            return JsonSerializer.Deserialize<PropertyConfirmedEvent>(eventData) as object;
                        }
                        else
                        {
                            return null;
                        }
                    })
                    .Where(x => x != null)
                    .Cast<Event>()
                    .ToList();

                    var property = new Property();
                    property.Apply(events);
                    return property;
                })
                .ToList();
        }

        private Guid GetAggregateId(object @event) => @event switch
        {
            PropertyCreatedEvent e => e.Id,
            PropertyConfirmedEvent e => e.Id,
            PropertyBlockedEvent e => e.Id,
            PropertyAvailabilityUpdatedEvent e => e.Id,
            PropertyPriceUpdatedEvent e => e.Id,
            PropertyStatusChangedEvent e => e.Id,
            PropertyDeletedEvent e => e.Id,
            _ => throw new ArgumentException("Unknown event type")
        };
    }
}