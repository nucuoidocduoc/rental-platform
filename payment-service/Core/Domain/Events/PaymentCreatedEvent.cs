namespace payment_service.Core.Domain.Events
{
    public class PaymentCreatedEvent : Event
    {
        public string PaymentGateway { get; set; }

        public double Amount { get; set; }
    }
}