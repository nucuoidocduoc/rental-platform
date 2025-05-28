namespace booking_service.Core.Domain.Events
{
    public class BookingConfirmedEvent : Event
    {
        public Guid PropertyId { get; set; }

        public bool IsSuccessful { get; set; }
    }
}