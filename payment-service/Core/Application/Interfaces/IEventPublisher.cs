using payment_service.Core.Domain.Events;

namespace payment_service.Core.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishEvent(object @event, string queueName);
    }
}