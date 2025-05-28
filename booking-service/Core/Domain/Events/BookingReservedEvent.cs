namespace booking_service.Core.Domain.Events
{
    public class BookingReservedEvent : Event
    {
        public Guid PropertyId { get; set; }

        public double Amount { get; set; }

        public bool IsSuccessful { get; set; }
    }
}