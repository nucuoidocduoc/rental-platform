using booking_service.Core.Domain.Aggregates;

namespace booking_service.Core.Domain.Events
{
    public class BookingPaymentCompletedEvent : Event
    {
        public Guid PropertyId { get; set; }

        public DateRange Date { get; set; }
    }
}
