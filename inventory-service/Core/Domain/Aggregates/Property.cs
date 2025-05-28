using inventory_service.Core.Domain.Events;
using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Aggregates
{
    public class Property
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Type { get; set; }

        public decimal PricePerNight { get; set; }

        public int Bedrooms { get; set; }

        public double Bathrooms { get; set; }

        public int MaxGuests { get; set; }

        public double Rating { get; set; }

        public List<string> Images { get; set; }

        public List<string> Amenities { get; set; }

        public List<string> Rules { get; set; }

        public Owner Host { get; set; }

        public List<Booking> Bookings { get; set; } = [];

        public PropertyStatus Status { get; private set; }


        public void Apply(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case PropertyCreatedEvent createdEvent:
                        Apply(createdEvent);
                        break;

                    case PropertyBlockedEvent blockedEvent:
                        Apply(blockedEvent);
                        break;

                    case PropertyConfirmedEvent bookedEvent:
                        Apply(bookedEvent);
                        break;

                    case PropertyStatusChangedEvent statusChangedEvent:
                        Apply(statusChangedEvent);
                        break;

                    case PropertyPriceUpdatedEvent priceUpdatedEvent:
                        Apply(priceUpdatedEvent);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown event type: {@event.GetType().Name}");
                }
            }
        }

        // Invariants
        public void Apply(PropertyCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.EventName;
            Description = @event.Description;
            Location = @event.Location;
            PricePerNight = @event.PricePerNight;
            Rating = @event.Rating;
            Images = @event.Images ?? [];
            Amenities = @event.Amenities ?? [];
            Rules = @event.Rules ?? [];
            Host = @event.Host;
            Type = @event.Type;
            Bedrooms = @event.Bedrooms;
            Bathrooms = @event.Bathrooms;
            MaxGuests = @event.MaxGuests;
            Status = PropertyStatus.Active;
        }

        public void Apply(PropertyBlockedEvent @event)
        {
            Id = @event.Id;
            Bookings.Add(new Booking
            {
                BookingId = @event.BookingId,
                DateRange = @event.Date,
                Status = AvailabilityStatus.Blocked,
                UpdatedDate = @event.CreatedDate
            });
        }

        public void Apply(PropertyConfirmedEvent @event)
        {
            var newBooking = new Booking
            {
                BookingId = @event.BookingId,
                DateRange = @event.Date,
                Status = AvailabilityStatus.Booked,
                UpdatedDate = @event.CreatedDate
            };

            var booking = Bookings.FirstOrDefault(b => b.Equals(newBooking));
            if (booking != null && booking.Status == AvailabilityStatus.Blocked)
            {
                booking.Status = AvailabilityStatus.Booked;
                booking.UpdatedDate = @event.CreatedDate;
            }
        }

        public void Apply(PropertyStatusChangedEvent @event)
        {
            if (Status == PropertyStatus.Deleted && @event.NewStatus != PropertyStatus.Active)
                throw new InvalidOperationException("Deleted property can only be restored to Active.");
            Status = @event.NewStatus;
        }

        public void Apply(PropertyPriceUpdatedEvent @event)
        {
            if (Status != PropertyStatus.Active)
                throw new InvalidOperationException("Cannot update price for inactive or deleted property.");
            if (@event.Price < 0)
                throw new InvalidOperationException("Price cannot be negative.");

            PricePerNight = @event.Price;
        }

        public void Apply(PropertyDeletedEvent @event)
        {
            if (Status == PropertyStatus.Deleted)
                throw new InvalidOperationException("Property is already deleted.");
            Status = PropertyStatus.Deleted;
        }

        public void UpdatePrice(DateRange date, decimal price)
        {
            Apply(new PropertyPriceUpdatedEvent(Id, date, price));
        }

        public void ChangeStatus(PropertyStatus newStatus)
        {
            Apply(new PropertyStatusChangedEvent(Id, newStatus));
        }

        public void Delete()
        {
            Apply(new PropertyDeletedEvent(Id));
        }

        public bool IsAvailable(DateRange date)
        {
            return !Bookings.Any(x => x.DateRange.IsOverlapping(date));
        }
    }
}