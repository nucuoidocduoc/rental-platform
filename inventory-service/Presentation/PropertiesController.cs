using Amazon.DynamoDBv2;
using inventory_service.Core.Application.Interfaces;
using inventory_service.Core.Domain.Aggregates;
using inventory_service.Core.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace inventory_service.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyUseCase _propertyUseCase;
        private readonly IEventStore _eventStore;
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public PropertiesController(IPropertyUseCase propertyUseCase, IEventStore eventStore, IAmazonDynamoDB amazonDynamoDB)
        {
            _propertyUseCase = propertyUseCase;
            _eventStore = eventStore;
            _amazonDynamoDB = amazonDynamoDB;
        }

        [HttpGet("{propertyId}/availability")]
        public async Task<IActionResult> CheckPropertyAvailable(Guid propertyId, DateTime checkIn, DateTime checkout)
        {
            var isAvailable = await _propertyUseCase.CheckAvaibility(propertyId, checkIn, checkout);
            return Ok(new { Available = isAvailable });
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPropertyEvents()
        {
            var propertyEvents = await _eventStore.GetEventsAsync();
            foreach(var property in propertyEvents)
            {
                var eventData = property as Property;
                eventData.Bookings = [];
            }
            return Ok(propertyEvents);
        }

        [HttpGet("seed")]
        public async Task<IActionResult> Seed()
        {
            // read json file and
            var properties = LoadPropertiesFromJson("MockData.json");
            foreach (var property in properties)
            {
                property.EventName = "PropertyCreatedEvent";
                await _propertyUseCase.CreatePropertyAsync(property);
            }
            return Ok();
        }

        public static List<PropertyCreatedEvent> LoadPropertiesFromJson(string jsonFilePath)
        {
            try
            {
                var jsonString = System.IO.File.ReadAllText(jsonFilePath);
                var properties = JsonSerializer.Deserialize<List<PropertyCreatedEvent>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return properties;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading properties: {ex.Message}");
                return new List<PropertyCreatedEvent>();
            }
        }
    }
}