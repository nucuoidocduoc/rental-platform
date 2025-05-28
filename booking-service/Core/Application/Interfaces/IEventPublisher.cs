using booking_service.Core.Domain.Events;

namespace booking_service.Core.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishEvent(object @event, string queueName);
    }
}