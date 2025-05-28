using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Events
{
    public class PropertyCreatedEvent : Event
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal PricePerNight { get; set; }

        public int Bedrooms { get; set; }

        public double Bathrooms { get; set; }

        public int MaxGuests { get; set; }

        public double Rating { get; set; }

        public List<string> Images { get; set; }

        public List<string> Amenities { get; set; }

        public List<string> Rules { get; set; }

        public Owner Host { get; set; }

        public string Type { get; set; }

        public PropertyCreatedEvent()
        {
        }

        public PropertyCreatedEvent(Guid propertyId, string name, string type)
        {
            EventName = "PropertyCreatedEvent";
            Id = propertyId;
            Name = name;
            Type = type;
        }
    }
}