using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace inventory_service.Infrastructure.Data.EventStore.Schemas
{
    [DynamoDBTable("PropertyEvents")]
    public class DynamoPropertyEvent
    {
        [DynamoDBHashKey]// Partition key
        public Guid AggregateId { get; set; }

        [DynamoDBRangeKey]// Sort key
        public int Version { get; set; }

        public string EventData { get; set; }

        public Dictionary<string, AttributeValue> GetKeyValuePairs()
        {
            return new Dictionary<string, AttributeValue>
            {
                { nameof(AggregateId), new AttributeValue { S = AggregateId.ToString() } },
                { nameof(Version), new AttributeValue { N = Version.ToString() } },
                { nameof(EventData), new AttributeValue { S = EventData } }
            };
        }
    }
}