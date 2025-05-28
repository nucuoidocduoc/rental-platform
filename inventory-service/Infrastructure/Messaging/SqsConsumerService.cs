using Amazon.SQS;
using Amazon.SQS.Model;
using inventory_service.Core.Application.Interfaces;
using inventory_service.Core.Domain.Events;
using inventory_service.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace inventory_service.Infrastructure.Messaging
{
    public class SqsConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly SQSSettings _sqsSetting;
        private readonly IPropertyUseCase _propertyUseCases;
        private readonly ILogger _logger;

        public SqsConsumerService(
        IAmazonSQS sqsClient,
        IOptions<AppSettings> appSettings,
        IPropertyUseCase propertyUseCases,
        ILogger<SqsConsumerService> logger)
        {
            _sqsClient = sqsClient;
            _sqsSetting = appSettings.Value.AWS.SQS;
            _propertyUseCases = propertyUseCases;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = _sqsSetting.ConsumeQueues.FirstOrDefault(q => q.QueueName == "property");
            if (queue == null)
            {
                return;
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var receiveMessageRequest = new ReceiveMessageRequest
                    {
                        QueueUrl = queue.QueueUrl,
                        MaxNumberOfMessages = 10,
                        WaitTimeSeconds = 20 // Long polling
                    };

                    var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
                    if (response?.Messages == null || response.Messages.Count == 0)
                    {
                        _logger.LogInformation("No messages received from SQS queue.");
                        continue; // No messages to process
                    }
                    foreach (var message in response.Messages)
                    {
                        try
                        {
                            var eventType = JsonSerializer.Deserialize<JsonElement>(message.Body).GetProperty("EventName").GetString();
                            if (eventType == nameof(BookingCreatedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<BookingCreatedEvent>(message.Body);
                                await _propertyUseCases.HandleBookingCreatedEventAsync(@event);
                            }
                            else if (eventType == nameof(BookingPaymentCompletedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<BookingPaymentCompletedEvent>(message.Body);
                                await _propertyUseCases.HandleBookingPaymentCompletedEventAsync(@event);
                            }
                            else
                            {
                                _logger.LogWarning("Unknown event type: {EventType}", eventType);
                                continue; // Skip unknown event types
                            }

                            // Delete message from queue after processing
                            await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
                            {
                                QueueUrl = queue.QueueUrl,
                                ReceiptHandle = message.ReceiptHandle
                            }, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing SQS message: {MessageId}", message.MessageId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error receiving messages from SQS");
                    await Task.Delay(1000, stoppingToken); // Wait before retrying
                }
            }
        }
    }
}