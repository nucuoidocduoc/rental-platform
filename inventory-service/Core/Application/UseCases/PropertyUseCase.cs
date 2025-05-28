using booking_service.Core.Application.Interfaces;
using inventory_service.Core.Application.Interfaces;
using inventory_service.Core.Domain.Aggregates;
using inventory_service.Core.Domain.Events;
using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Application.UseCases
{
    public class PropertyUseCase : IPropertyUseCase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public PropertyUseCase(IEventStore eventStore, IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        public async Task CreatePropertyAsync(PropertyCreatedEvent propertyCreatedEvent)
        {
            await _eventStore.SaveEventAsync(propertyCreatedEvent, 0);
        }

        public async Task<bool> CheckAvaibility(Guid propertyId, DateTime checkin, DateTime checkout)
        {
            var property = await RebuildPropertyAsync(propertyId);
            return property.IsAvailable(new DateRange(checkin, checkout));
        }

        private async Task<Property> RebuildPropertyAsync(Guid propertyId)
        {
            var events = await _eventStore.GetEventsAsync(propertyId);
            if (events == null || !events.Any())
            {
                throw new InvalidOperationException($"No events found for property ID: {propertyId}");
            }
            var property = new Property();
            property.Apply(events);
            return property;
        }

        public async Task HandleBookingCreatedEventAsync(BookingCreatedEvent @event)
        {
            var avaibilityChangedEvent = new PropertyBlockedEvent
            {
                EventName = "PropertyBlockedEvent",
                Id = @event.PropertyId,
                BookingId = @event.BookingId,
                Date = @event.Date
            };

            var currentVersion = (await _eventStore.GetEventsAsync(@event.PropertyId)).Count();

            await _eventStore.SaveEventAsync(avaibilityChangedEvent, currentVersion);
            await _eventPublisher.PublishEvent(avaibilityChangedEvent, "booking");
        }

        public async Task HandleBookingPaymentCompletedEventAsync(BookingPaymentCompletedEvent @event)
        {
            var avaibilityChangedEvent = new PropertyConfirmedEvent
            {
                EventName = "PropertyConfirmedEvent",
                Id = @event.PropertyId,
                BookingId = @event.BookingId,
                Date = @event.Date
            };

            var currentVersion = (await _eventStore.GetEventsAsync(@event.PropertyId)).Count();

            await _eventStore.SaveEventAsync(avaibilityChangedEvent, currentVersion);
            await _eventPublisher.PublishEvent(avaibilityChangedEvent, "booking");
        }
    }
}