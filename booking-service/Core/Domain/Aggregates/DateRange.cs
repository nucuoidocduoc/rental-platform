namespace booking_service.Core.Domain.Aggregates
{
    public class DateRange
    {
        public DateRange(DateTime checkin, DateTime checkout)
        {
            CheckIn = checkin;
            CheckOut = checkout;
        }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
    }
}