namespace inventory_service.Core.Domain.Events
{
    public class Event
    {
        public string EventName { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}