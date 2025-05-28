namespace inventory_service.Core.Domain.ValueObjects
{
    public class DateRange
    {
        public DateRange(DateTime checkin, DateTime checkout)
        {
            CheckIn = checkin;
            CheckOut = checkout;
        }
        public DateRange()
        {
            
        }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DateRange other)
            {
                return CheckIn == other.CheckIn && CheckOut == other.CheckOut;
            }
            return false;
        }

        public bool IsOverlapping(DateRange other)
        {
            return CheckIn >= other.CheckIn && CheckIn <= other.CheckOut
                || CheckOut >= other.CheckIn && CheckOut <= other.CheckOut
                || CheckIn <= other.CheckIn && CheckOut >= other.CheckOut;
        }
    }
}