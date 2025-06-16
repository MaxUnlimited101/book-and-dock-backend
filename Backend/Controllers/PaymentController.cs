using Backend.DTO.Payment;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly StripeService _stripeService;

        public PaymentController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentRequestDto request)
        {
            var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
                request.Amount,
                request.Currency,
                request.SuccessUrl,
                request.CancelUrl
            );

            return Ok(new { url = sessionUrl });
        }
    }

}
