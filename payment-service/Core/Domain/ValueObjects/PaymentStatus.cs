namespace payment_service.Core.Domain.ValueObjects
{
    public enum PaymentStatus
    {
        Created = 1,
        Authorized = 2,
        Captured = 3,
        Failed = 4,
        Cancelled = 5,
        Refunded = 6
    }
}
