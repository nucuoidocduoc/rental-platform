using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using payment_service.Core.Application.Interfaces;
using payment_service.Core.Domain.Events;
using payment_service.Infrastructure.Models;
using System.Text.Json;

namespace payment_service.Infrastructure.Messaging
{
    public class SqsConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IPaymentUseCase _paymentUseCases;
        private readonly SQSSettings _sqsSetting;
        private readonly ILogger _logger;

        public SqsConsumerService(
        IAmazonSQS sqsClient,
        IPaymentUseCase paymentUseCases,
        IOptions<AppSettings> appSettings,
        ILogger<SqsConsumerService> logger)
        {
            _sqsClient = sqsClient;
            _paymentUseCases = paymentUseCases;
            _sqsSetting = appSettings.Value.AWS.SQS;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = _sqsSetting.ConsumeQueues.FirstOrDefault(q => q.QueueName == "payment");
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
                            if (eventType == nameof(BookingReservedEvent))
                            {
                                var @event = JsonSerializer.Deserialize<BookingReservedEvent>(message.Body);
                                await _paymentUseCases.HandleBookingReservedEvent(@event);
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