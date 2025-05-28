using booking_service.Core.Domain.Events;

namespace booking_service.Core.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<T> GetEventAsync<T>(Guid eventId);

        Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId);

        Task SaveEventAsync<T>(T @event, int expectedVersion = 0) where T : Event;
    }
}