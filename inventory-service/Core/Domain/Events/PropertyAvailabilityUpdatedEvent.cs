using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Domain.Events
{
    public class PropertyAvailabilityUpdatedEvent : Event
    {
        public DateRange Date { get; set; }

        public AvailabilityStatus Status { get; set; }

        public PropertyAvailabilityUpdatedEvent()
        {
            
        }
        public PropertyAvailabilityUpdatedEvent(Guid propertyId, DateRange date, AvailabilityStatus status)
        {
            Id = propertyId;
            Date = date;
            Status = status;
        }
    }
}