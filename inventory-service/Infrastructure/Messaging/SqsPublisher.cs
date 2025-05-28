using Amazon.SQS;
using Amazon.SQS.Model;
using booking_service.Core.Application.Interfaces;
using inventory_service.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace inventory_service.Infrastructure.Messaging
{
    public class SqsPublisher : IEventPublisher
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IOptions<AppSettings> _configuration;

        public SqsPublisher(IAmazonSQS sqsClient, IOptions<AppSettings> configuration)
        {
            _sqsClient = sqsClient;
            _configuration = configuration;
        }

        public async Task PublishEvent(object @event, string queueName)
        {
            var queueUrl = _configuration.Value.AWS.SQS.PublishQueues.FirstOrDefault(x => x.QueueName == queueName).QueueUrl;
            var message = JsonSerializer.Serialize(@event);
            await _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = message
            });
        }
    }
}