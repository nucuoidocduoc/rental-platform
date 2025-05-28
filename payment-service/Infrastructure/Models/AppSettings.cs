namespace payment_service.Infrastructure.Models
{
    public class AppSettings
    {
        public AwsSettings AWS { get; set; }
    }

    public class AwsSettings
    {
        public string Region { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string Endpoint { get; set; }

        public SQSSettings SQS { get; set; }
    }

    public class SQSSettings
    {
        public List<QueueInfo> PublishQueues { get; set; }
        public List<QueueInfo> ConsumeQueues { get; set; }
    }

    public class QueueInfo
    {
        public string QueueName { get; set; }
        public string QueueUrl { get; set; }
    }
}