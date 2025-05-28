using booking_service.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace booking_service.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingUseCases _bookingService;

        public BookingController(IBookingUseCases bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var booking = await _bookingService.CreateBookingAsync(
                request.GuestId,
                request.PropertyId,
                request.CheckIn,
                request.CheckOut,
                request.Guests,
                request.TotalPrice);
            return Ok(booking);
        }
    }

    public record CreateBookingRequest(
    Guid GuestId,
    Guid PropertyId,
    DateTime CheckIn,
    DateTime CheckOut,
    int Guests,
    decimal TotalPrice);
}