using payment_service.Core.Application.Interfaces;
using payment_service.Core.Domain.Aggregates;
using payment_service.Core.Domain.Events;

namespace payment_service.Core.Application.UseCases
{
    public class PaymentUseCase : IPaymentUseCase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public PaymentUseCase(IEventStore eventStore, IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        public async Task HandleBookingReservedEvent(BookingReservedEvent @event)
        {
            var paymentCreated = new PaymentCreatedEvent
            {
                PaymentId = @event.BookingId,
                Amount = @event.Amount,
                EventName = "PaymentCreatedEvent",
            };

            await _eventStore.SaveEventAsync(paymentCreated, 0);
        }

        public async Task HandlePaymentAuthorizedCallback(Guid paymentId)
        {
            var payment = await RebuildPropertyAsync(paymentId);
            var currentVersion = (await _eventStore.GetEventsAsync(paymentId)).Count();

            var paymentAuthorized = new PaymentAuthorizedEvent
            {
                PaymentId = paymentId,
                EventName = "PaymentAuthorizedEvent",
            };

            await _eventStore.SaveEventAsync(paymentAuthorized, currentVersion);

            var paymentCaptured = new PaymentCapturedEvent
            {
                PaymentId = paymentId,
                EventName = "PaymentCapturedEvent",
            };

            await _eventStore.SaveEventAsync(paymentCaptured, currentVersion + 1);
            await _eventPublisher.PublishEvent(paymentCaptured, "booking");
        }

        private async Task<Payment> RebuildPropertyAsync(Guid paymentId)
        {
            var events = await _eventStore.GetEventsAsync(paymentId);
            var property = new Payment();
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case PaymentCreatedEvent createdEvent:
                        property.Apply(createdEvent);
                        break;

                    case PaymentAuthorizedEvent blockedEvent:
                        property.Apply(blockedEvent);
                        break;

                    case PaymentCapturedEvent bookedEvent:
                        property.Apply(bookedEvent);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown event type: {@event.GetType().Name}");
                }
            }
            return property;
        }
    }
}