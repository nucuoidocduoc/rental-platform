using payment_service.Core.Domain.Events;

namespace payment_service.Core.Application.Interfaces
{
    public interface IEventStore
    {
        Task SaveEventAsync(object @event, int expectedVersion);

        Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId);

        Task<IEnumerable<Event>> GetEventsAsync();
    }
}