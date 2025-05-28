namespace inventory_service.Core.Domain.Events
{
    public class PropertyDeletedEvent : Event
    {
        public PropertyDeletedEvent()
        {
            
        }
        public PropertyDeletedEvent(Guid propertyId)
        {
            Id = propertyId;
        }
    }
}