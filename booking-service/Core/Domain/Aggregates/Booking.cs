using booking_service.Core.Domain.Events;

namespace booking_service.Core.Domain.Aggregates
{
    public class Booking
    {
        public Guid BookingId { get; private set; }
        public Guid GuestId { get; private set; }
        public Guid PropertyId { get; private set; }
        public DateRange DateRange { get; private set; }
        public Price Price { get; private set; }

        public int Guests { get; set; }

        public string Status { get; private set; }

        public Booking()
        { }

        // Constructor cho sự kiện BookingCreated
        public Booking(Guid guestId, Guid propertyId, DateRange dateRange, int guests, Price price)
        {
            BookingId = Guid.NewGuid();
            GuestId = guestId;
            PropertyId = propertyId;
            DateRange = dateRange ?? throw new ArgumentNullException(nameof(dateRange));
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Status = "Pending";
            Guests = guests;
        }

        // Phương thức áp dụng sự kiện
        public void Apply(BookingCreatedEvent @event)
        {
            BookingId = @event.BookingId;
            GuestId = @event.GuestId;
            PropertyId = @event.PropertyId;
            DateRange = @event.Date;
            Price = new Price(@event.Amount, @event.Currency);
            Guests = @event.Guests;
            Status = "Pending";
        }

        public void Apply(BookingReservedEvent @event)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Booking must be pending to process payment.");
            Status = "Reserved";
        }

        public void Apply(BookingPaymentCompletedEvent @event)
        {
            if (Status != "Reserved")
                throw new InvalidOperationException("Booking must be pending to process payment.");
            Status = "Payment Paid";
        }

        public void Apply(BookingConfirmedEvent @event)
        {
            if (Status != "Payment Paid")
                throw new InvalidOperationException("Booking must be pending to process payment.");
            Status = "Confirmed";
        }

        public void Apply(BookingCompletedEvent @event)
        {
            if (Status != "Confirmed" || Status != "Fail")
                throw new InvalidOperationException("Booking must be pending to process payment.");
            Status = "Completed";
        }

        // Phương thức để hoàn tất Booking
        public void Complete()
        {
            if (Status != "Confirmed" && Status != "Fail")
                throw new InvalidOperationException("Booking must be confirmed before completion.");
            Status = "Completed";
        }
    }
}