using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using Amazon.SQS.Model;
using booking_service.Infrastructure.Data.EventStore.Schemas;
using booking_service.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace booking_service.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTestingController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly IAmazonSQS _sqsClient;
        private readonly Dictionary<string, string> _sqsQueueUrls;

        public DataTestingController(IDynamoDBContext dynamoDBContext, IAmazonSQS sqsClient, IOptions<AppSettings> config)
        {
            _dynamoDBContext = dynamoDBContext;
            _sqsClient = sqsClient;
            _sqsQueueUrls = config.Value.AWS.SQS.PublishQueues.Concat(config.Value.AWS.SQS.ConsumeQueues)
                .ToDictionary(q => q.QueueName, q => q.QueueUrl);
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _dynamoDBContext.ScanAsync<DynamoBookingEvent>([]).GetRemainingAsync();
            return Ok(events);
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetAllMessages([FromQuery] string queueName)
        {
            if (!_sqsQueueUrls.TryGetValue(queueName, out var queueUrl))
            {
                return NotFound($"Queue '{queueName}' not found.");
            }

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 1, // Long polling,
                QueueUrl = queueUrl // Replace with your actual queue URL
            };

            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
            return Ok(response.Messages);
        }
    }
}