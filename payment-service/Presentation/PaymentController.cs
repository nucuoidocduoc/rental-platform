using Microsoft.AspNetCore.Mvc;
using payment_service.Core.Application.Interfaces;

namespace payment_service.Presentation
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentUseCase _paymentUseCases;
        private readonly IEventStore _eventStore;

        public PaymentController(IPaymentUseCase paymentUseCases, IEventStore eventStore)
        {
            _paymentUseCases = paymentUseCases;
            _eventStore = eventStore;
        }

        [HttpPost("callback/{paymentId}")]
        public async Task<IActionResult> PaymentAuthorizedCallback(Guid paymentId)
        {
            // Here you would typically call your payment use case to handle the callback
            // For example:
            await _paymentUseCases.HandlePaymentAuthorizedCallback(paymentId);

            // For now, we just return a success response
            return Ok(new { Message = "Payment authorized successfully", PaymentId = paymentId });
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(string paymentId)
        {
            var paymentEvents = await _eventStore.GetEventsAsync(Guid.Parse(paymentId));

            return Ok(paymentEvents);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPayments()
        {
            var paymentEvents = await _eventStore.GetEventsAsync();

            return Ok(paymentEvents);
        }
    }
}