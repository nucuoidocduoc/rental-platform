using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Aggregates
{
    public class Booking
    {
        public Guid BookingId { get; set; }

        public DateRange DateRange { get; set; }

        public AvailabilityStatus Status { get; set; }

        public DateTime UpdatedDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Booking booking)
            {
                return false;
            }

            return BookingId == booking.BookingId
                   && DateRange.Equals(booking.DateRange)
                   && Status == booking.Status;
        }
    }
}