namespace inventory_service.Core.Domain.Events
{
    public class PropertyStatusChangedEvent : Event
    {
        public PropertyStatus NewStatus { get; set; }

        public PropertyStatusChangedEvent(Guid propertyId, PropertyStatus newStatus)
        {
            Id = propertyId;
            NewStatus = newStatus;
        }

        public override string ToString()
        {
            return $"PropertyStatusChangedEvent: PropertyId={Id}, NewStatus={NewStatus}";
        }
    }
}