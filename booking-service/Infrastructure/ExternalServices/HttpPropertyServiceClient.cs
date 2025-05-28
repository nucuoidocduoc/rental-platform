using booking_service.Core.Application.Interfaces;
using booking_service.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace booking_service.Infrastructure.ExternalServices
{
    public class HttpPropertyServiceClient : IInventoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _options;

        public HttpPropertyServiceClient(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<bool> IsAvailable(Guid propertyId, DateTime checkin, DateTime checkout)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_options.InventoryService}/api/properties/{propertyId}/availability?checkIn={checkin:yyyy-MM-dd}&checkOut={checkout:yyyy-MM-dd}");
            return response.IsSuccessStatusCode;
        }
    }
}