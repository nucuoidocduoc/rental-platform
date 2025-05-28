using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Events
{
    public class BookingCreatedEvent
    {
        public string EventName { get; set; }

        public Guid BookingId { get; set; }

        public int Version { get; set; }

        public Guid GuestId { get; set; }

        public Guid PropertyId { get; set; }

        public DateRange Date { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime OccurredOn { get; set; }
    }
}