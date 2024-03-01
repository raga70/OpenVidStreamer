using Account.Model.DTO;
using Account.Services;
using Microsoft.AspNetCore.Mvc;

namespace Account.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly StripePaymentService _paymentService;

        public PaymentController(StripePaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("processPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] IncomingPaymentDTO incomingPayment)
        {
            var accId = Common.AccIdExtractorFromHttpContext.GetAccId(HttpContext);
            var charge = await _paymentService.ProcessPaymentAsync(incomingPayment, accId);
            if (charge.Status == "succeeded")
            {
                return Ok(charge);
            }
            else
            {
                return BadRequest(new { message = "Payment processing failed." });
            }
        }
    }
}