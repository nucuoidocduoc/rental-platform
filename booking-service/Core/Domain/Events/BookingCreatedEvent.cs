using booking_service.Core.Domain.Aggregates;

namespace booking_service.Core.Domain.Events
{
    public class BookingCreatedEvent : Event
    {
        public Guid GuestId { get; set; }

        public Guid PropertyId { get; set; }

        public DateRange Date { get; set; }

        public decimal Amount { get; set; }

        public int Guests { get; set; }

        public string Currency { get; set; }
    }
}