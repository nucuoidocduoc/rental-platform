namespace payment_service.Core.Domain.Events
{
    public class Event
    {
        public Guid PaymentId { get; set; }

        public string EventName { get; set; }
    }
}
