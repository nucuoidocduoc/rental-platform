using booking_service.Core.Domain.Events;

namespace booking_service.Events
{
    public class PaymentCapturedEvent : Event
    {
        public Guid PaymentId { get; set; }

    }
}