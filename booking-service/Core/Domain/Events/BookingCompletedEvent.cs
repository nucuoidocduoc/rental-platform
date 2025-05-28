namespace booking_service.Core.Domain.Events
{
    public class BookingCompletedEvent : Event
    {
        public Guid GuestId { get; set; }

        public Guid PropertyId { get; set; }
    }
}