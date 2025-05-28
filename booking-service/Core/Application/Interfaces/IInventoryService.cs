namespace booking_service.Core.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> IsAvailable(Guid propertyId, DateTime checkin, DateTime checkout);
    }
}