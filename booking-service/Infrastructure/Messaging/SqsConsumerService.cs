using Amazon.SQS;
using Amazon.SQS.Model;
using booking_service.Core.Application.Interfaces;
using booking_service.Core.Domain.Events;
using booking_service.Events;
using booking_service.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace booking_service.Infrastructure.Messaging
{
    public class SqsConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IBookingUseCases _bookingService;
        private readonly SQSSettings _sqsSetting;
        private readonly ILogger _logger;

        public SqsConsumerService(
        IAmazonSQS sqsClient,
        IBookingUseCases bookingService,
        IOptions<AppSettings> appSettings,
        ILogger<SqsConsumerService> logger)
        {
            _sqsClient = sqsClient;
            _bookingService = bookingService;
            _sqsSetting = appSettings.Value.AWS.SQS;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = _sqsSetting.ConsumeQueues.FirstOrDefault(q => q.QueueName == "booking");
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
                        WaitTimeSeconds = 5 // Long polling
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
                            if (eventType == nameof(PropertyBlockedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<PropertyBlockedEvent>(message.Body);
                                await _bookingService.HandlePropertyBlockedAsync(@event);
                            }
                            else if (eventType == nameof(PropertyConfirmedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<PropertyConfirmedEvent>(message.Body);
                                await _bookingService.HandlePropertyConfirmedAsync(@event);
                            }
                            else if (eventType == nameof(PaymentCapturedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<PaymentCapturedEvent>(message.Body);
                                await _bookingService.HandlePaymentProcessedAsync(@event);
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