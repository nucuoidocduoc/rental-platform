using inventory_service.Core.Domain.Events;
using inventory_service.Core.Domain.ValueObjects;

namespace inventory_service.Core.Application.Interfaces
{
    public interface IPropertyUseCase
    {
        Task CreatePropertyAsync(PropertyCreatedEvent propertyCreatedEvent);

        Task<bool> CheckAvaibility(Guid propertyId, DateTime checkin, DateTime checkout);

        Task HandleBookingCreatedEventAsync(BookingCreatedEvent @event);

        Task HandleBookingPaymentCompletedEventAsync(BookingPaymentCompletedEvent @event);
    }
}
