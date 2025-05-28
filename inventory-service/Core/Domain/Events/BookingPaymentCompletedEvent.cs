using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Events
{
    public class BookingPaymentCompletedEvent
    {
        public Guid PropertyId { get; set; }

        public Guid BookingId { get; set; }

        public DateRange Date { get; set; }
    }
}