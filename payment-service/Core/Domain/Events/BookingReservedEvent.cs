namespace payment_service.Core.Domain.Events
{
    public class BookingReservedEvent
    {
        public string EventName { get; set; }

        public Guid BookingId { get; set; }

        public Guid PropertyId { get; set; }

        public double Amount { get; set; }

        public DateTime OccurredOn { get; set; }
    }
}