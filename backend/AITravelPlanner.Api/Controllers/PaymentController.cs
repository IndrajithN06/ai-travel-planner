using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Create a payment intent for a travel plan
        /// </summary>
        [HttpPost("create-intent")]
        public async Task<ActionResult<PaymentIntentResponse>> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("User not authenticated");
                }

                var response = await _paymentService.CreatePaymentIntentAsync(request, userId.Value);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid payment intent request");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent");
                return StatusCode(500, "An error occurred while creating the payment intent");
            }
        }

        /// <summary>
        /// Get payment status by payment intent ID
        /// </summary>
        [HttpGet("{paymentIntentId}/status")]
        public async Task<ActionResult<PaymentStatusResponse>> GetPaymentStatus(string paymentIntentId)
        {
            try
            {
                var status = await _paymentService.GetPaymentStatusAsync(paymentIntentId);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Payment not found: {PaymentIntentId}", paymentIntentId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status for {PaymentIntentId}", paymentIntentId);
                return StatusCode(500, "An error occurred while retrieving payment status");
            }
        }

        /// <summary>
        /// Get payment by ID
        /// </summary>
        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentResponse>> GetPayment(int paymentId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
                return Ok(payment);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Payment not found: {PaymentId}", paymentId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment {PaymentId}", paymentId);
                return StatusCode(500, "An error occurred while retrieving payment");
            }
        }

        /// <summary>
        /// Get all payments for the current user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetUserPayments(int userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Unauthorized("User not authenticated");
                }

                // Users can only view their own payments
                if (currentUserId.Value != userId)
                {
                    return Forbid("You can only view your own payments");
                }

                var payments = await _paymentService.GetUserPaymentsAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving payments");
            }
        }

        /// <summary>
        /// Get all payments for the current authenticated user
        /// </summary>
        [HttpGet("my-payments")]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetMyPayments()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("User not authenticated");
                }

                var payments = await _paymentService.GetUserPaymentsAsync(userId.Value);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for current user");
                return StatusCode(500, "An error occurred while retrieving payments");
            }
        }

        /// <summary>
        /// Stripe webhook endpoint (must be public for Stripe to call)
        /// </summary>
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var signature = Request.Headers["Stripe-Signature"].ToString();

                if (string.IsNullOrEmpty(signature))
                {
                    _logger.LogWarning("Webhook request missing Stripe signature");
                    return BadRequest("Missing Stripe signature");
                }

                var handled = await _paymentService.HandleWebhookAsync(json, signature);
                
                if (handled)
                {
                    return Ok(new { received = true });
                }
                else
                {
                    return BadRequest("Failed to handle webhook");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling webhook");
                return StatusCode(500, "Webhook processing failed");
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("userId")?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
