using payment_service.Core.Domain.Events;

namespace payment_service.Core.Application.Interfaces
{
    public interface IPaymentUseCase
    {
        Task HandleBookingReservedEvent(BookingReservedEvent @event);

        Task HandlePaymentAuthorizedCallback(Guid paymentId);
    }
}