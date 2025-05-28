using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Events
{
    public class PropertyPriceUpdatedEvent : Event
    {
        public DateRange Date { get; }
        public decimal Price { get; }
        public PropertyPriceUpdatedEvent()
        {
            
        }
        public PropertyPriceUpdatedEvent(Guid propertyId, DateRange date, decimal price)
        {
            Id = propertyId;
            Date = date;
            Price = price;
        }

        public override string ToString()
        {
            return $"PropertyPriceUpdatedEvent: PropertyId={Id}, Date={Date}, Price={Price}";
        }
    }
}