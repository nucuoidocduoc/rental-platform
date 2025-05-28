namespace booking_service.Core.Domain.Events
{
    public class Event
    {
        public string EventName { get; set; }

        public Guid BookingId { get; set; }

        public int Version { get; set; }

        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
    }
}