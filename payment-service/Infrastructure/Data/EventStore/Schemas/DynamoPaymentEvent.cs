using Amazon.DynamoDBv2.DataModel;

namespace payment_service.Infrastructure.Data.EventStore.Schemas
{
    [DynamoDBTable("PaymentEvents")]
    public class DynamoPaymentEvent
    {
        [DynamoDBHashKey] // Partition key
        public Guid AggregateId { get; set; }

        [DynamoDBRangeKey] // Sort key
        public int Version { get; set; }

        public string EventData { get; set; }

        public Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> GetKeyValuePairs()
        {
            return new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>
            {
                { nameof(AggregateId), new Amazon.DynamoDBv2.Model.AttributeValue { S = AggregateId.ToString() } },
                { nameof(Version), new Amazon.DynamoDBv2.Model.AttributeValue { N = Version.ToString() } },
                { nameof(EventData), new Amazon.DynamoDBv2.Model.AttributeValue { S = EventData } }
            };
        }
    }
}