using booking_service.Core.Domain.Aggregates;
using booking_service.Core.Domain.Events;
using booking_service.Events;

namespace booking_service.Core.Application.Interfaces
{
    public interface IBookingUseCases
    {
        Task<Booking> CreateBookingAsync(Guid guestId, Guid propertyId, DateTime checkIn, DateTime checkOut, int guests, decimal amount);

        Task HandlePaymentProcessedAsync(PaymentCapturedEvent @event);

        Task HandlePropertyBlockedAsync(PropertyBlockedEvent @event);

        Task HandlePropertyConfirmedAsync(PropertyConfirmedEvent @event);
    }
}