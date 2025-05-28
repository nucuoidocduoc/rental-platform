namespace booking_service.Core.Domain.Aggregates
{
    public class Price
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Price(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
    }
}
