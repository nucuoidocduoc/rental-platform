using payment_service.Core.Domain.Events;
using payment_service.Core.Domain.ValueObjects;

namespace payment_service.Core.Domain.Aggregates
{
    public class Payment
    {
        public Guid Id { get; set; }

        public string PaymentGateway { get; set; }

        public double Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public void Apply(PaymentAuthorizedEvent @event)
        {
            if (Status != PaymentStatus.Created)
                throw new InvalidOperationException("Cannot update availability for inactive or deleted property.");

            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            Status = PaymentStatus.Authorized;
        }

        public void Apply(PaymentCapturedEvent @event)
        {
            if (Status != PaymentStatus.Authorized)
                throw new InvalidOperationException("Payment must be authorized before it can be captured.");

            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            Status = PaymentStatus.Captured;
        }

        public void Apply(PaymentCreatedEvent @event)
        {
            Id = @event.PaymentId;
            PaymentGateway = @event.PaymentGateway;
            Amount = @event.Amount;
            Status = PaymentStatus.Failed;
        }
    }
}