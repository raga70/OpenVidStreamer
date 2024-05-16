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
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
            var charge = await _paymentService.ProcessPaymentAsync(incomingPayment, accId);
           if(charge.Status == "requires_action")
            {
                return Ok(charge);
            }
           
            if (charge.Status == "succeeded")
            {
                return Ok(charge);
            }
            else
            {
                return BadRequest(new { message = "Payment processing failed." });
            }
        }



        [HttpPost("confirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentIntentIdBody paymentid)
        {
            var paymentIntentId = paymentid.paymentIntentId;
            HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var accId = Common.AccIdExtractorFromHttpContext.ExtractAccIdUpnFromJwtToken(token);
            var charge = await _paymentService.ConfirmPaymentAsync(paymentIntentId, accId);
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

    public record PaymentIntentIdBody
    {
        public string paymentIntentId { get; set; }
    }
    
}