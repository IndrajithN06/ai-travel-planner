using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITravelPlanRepository _travelPlanRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly CustomerService _customerService;
        private readonly EventService _eventService;

        public PaymentService(
            IPaymentRepository paymentRepository,
            ITravelPlanRepository travelPlanRepository,
            IUserRepository userRepository,
            IConfiguration configuration,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _travelPlanRepository = travelPlanRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;

            // Initialize Stripe with secret key
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            _paymentIntentService = new PaymentIntentService();
            _customerService = new CustomerService();
            _eventService = new EventService();
        }

        public async Task<PaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request, int userId)
        {
            try
            {
                // Validate travel plan exists
                var travelPlan = await _travelPlanRepository.GetByIdAsync(request.TravelPlanId);
                if (travelPlan == null)
                {
                    throw new ArgumentException($"Travel plan with ID {request.TravelPlanId} not found.");
                }

                // Validate user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException($"User with ID {userId} not found.");
                }

                // Get or create Stripe customer
                var stripeCustomerId = await GetOrCreateStripeCustomerAsync(user);

                // Create payment intent options
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100), // Convert to cents
                    Currency = request.Currency.ToLower(),
                    Customer = stripeCustomerId,
                    Description = request.Description ?? $"Payment for travel plan: {travelPlan.Title}",
                    Metadata = new Dictionary<string, string>
                    {
                        { "travel_plan_id", request.TravelPlanId.ToString() },
                        { "user_id", userId.ToString() }
                    },
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    }
                };

                // Create payment intent in Stripe
                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                // Create payment record in database
                var payment = new Payment
                {
                    TravelPlanId = request.TravelPlanId,
                    UserId = userId,
                    StripePaymentIntentId = paymentIntent.Id,
                    StripeCustomerId = stripeCustomerId,
                    Amount = request.Amount,
                    Currency = request.Currency.ToUpper(),
                    Status = paymentIntent.Status,
                    Description = request.Description,
                    CreatedDate = DateTime.UtcNow
                };

                await _paymentRepository.CreateAsync(payment);

                // Log transaction
                await LogTransactionAsync(payment.Id, paymentIntent.Id, "payment_intent.created", paymentIntent.Status, paymentIntent.ToJson());

                return new PaymentIntentResponse
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id,
                    Amount = request.Amount,
                    Currency = request.Currency.ToUpper()
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error creating payment intent for user {UserId}", userId);
                throw new Exception($"Payment processing error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment intent for user {UserId}", userId);
                throw;
            }
        }

        public async Task<PaymentStatusResponse> GetPaymentStatusAsync(string paymentIntentId)
        {
            try
            {
                // Get payment from database
                var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntentId);
                if (payment == null)
                {
                    throw new ArgumentException($"Payment with intent ID {paymentIntentId} not found.");
                }

                // Get latest status from Stripe
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId);

                // Update payment status if changed
                if (payment.Status != paymentIntent.Status)
                {
                    payment.Status = paymentIntent.Status;
                    if (paymentIntent.Status == "succeeded")
                    {
                        payment.CompletedDate = DateTime.UtcNow;
                        // Get receipt URL from latest charge if available
                        if (paymentIntent.LatestCharge != null)
                        {
                            var chargeService = new ChargeService();
                            var charge = await chargeService.GetAsync(paymentIntent.LatestCharge.Id);
                            payment.ReceiptUrl = charge.ReceiptUrl;
                        }
                    }
                    await _paymentRepository.UpdateAsync(payment);
                }

                return new PaymentStatusResponse
                {
                    PaymentIntentId = paymentIntentId,
                    Status = paymentIntent.Status,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    CompletedDate = payment.CompletedDate,
                    ReceiptUrl = payment.ReceiptUrl
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe error getting payment status for {PaymentIntentId}", paymentIntentId);
                throw new Exception($"Payment status error: {ex.Message}", ex);
            }
        }

        public async Task<PaymentResponse> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new ArgumentException($"Payment with ID {paymentId} not found.");
            }

            return MapToPaymentResponse(payment);
        }

        public async Task<IEnumerable<PaymentResponse>> GetUserPaymentsAsync(int userId)
        {
            var payments = await _paymentRepository.GetByUserIdAsync(userId);
            return payments.Select(MapToPaymentResponse);
        }

        public async Task<bool> HandleWebhookAsync(string payload, string signature)
        {
            try
            {
                var webhookSecret = _configuration["Stripe:WebhookSecret"];
                if (string.IsNullOrEmpty(webhookSecret))
                {
                    _logger.LogWarning("Webhook secret not configured");
                    return false;
                }

                // Construct and verify the event
                var stripeEvent = EventUtility.ConstructEvent(
                    payload,
                    signature,
                    webhookSecret
                    
                );

                // Check if we've already processed this event (idempotency)
                var existingTransaction = await _paymentRepository.TransactionExistsAsync(stripeEvent.Id);
                if (existingTransaction)
                {
                    _logger.LogInformation("Event {EventId} already processed, skipping", stripeEvent.Id);
                    return true;
                }

                // Handle the event
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        await HandlePaymentIntentSucceeded(stripeEvent);
                        break;
                    case "payment_intent.payment_failed":
                        await HandlePaymentIntentFailed(stripeEvent);
                        break;
                    case "payment_intent.canceled":
                        await HandlePaymentIntentCanceled(stripeEvent);
                        break;
                    case "charge.refunded":
                        await HandleChargeRefunded(stripeEvent);
                        break;
                    default:
                        _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                        break;
                }

                // Log the transaction
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
                    if (payment != null)
                    {
                        await LogTransactionAsync(
                            payment.Id,
                            stripeEvent.Id,
                            stripeEvent.Type,
                            paymentIntent.Status,
                            stripeEvent.ToJson()
                        );
                    }
                }

                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe webhook error");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling webhook");
                return false;
            }
        }

        private async Task HandlePaymentIntentSucceeded(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null) return;

            var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
            if (payment == null) return;

            payment.Status = "succeeded";
            payment.CompletedDate = DateTime.UtcNow;
            
            // Get receipt URL from latest charge if available
            if (paymentIntent.LatestCharge != null)
            {
                var chargeService = new ChargeService();
                var charge = await chargeService.GetAsync(paymentIntent.LatestCharge.Id);
                payment.ReceiptUrl = charge.ReceiptUrl;
            }

            await _paymentRepository.UpdateAsync(payment);
            _logger.LogInformation("Payment {PaymentIntentId} succeeded", paymentIntent.Id);
        }

        private async Task HandlePaymentIntentFailed(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null) return;

            var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
            if (payment == null) return;

            payment.Status = "failed";
            await _paymentRepository.UpdateAsync(payment);
            _logger.LogWarning("Payment {PaymentIntentId} failed", paymentIntent.Id);
        }

        private async Task HandlePaymentIntentCanceled(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null) return;

            var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntent.Id);
            if (payment == null) return;

            payment.Status = "canceled";
            await _paymentRepository.UpdateAsync(payment);
            _logger.LogInformation("Payment {PaymentIntentId} canceled", paymentIntent.Id);
        }

        private async Task HandleChargeRefunded(Event stripeEvent)
        {
            var charge = stripeEvent.Data.Object as Charge;
            if (charge == null) return;

            var paymentIntentId = charge.PaymentIntentId;
            if (string.IsNullOrEmpty(paymentIntentId)) return;

            var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(paymentIntentId);
            if (payment == null) return;

            payment.Status = "refunded";
            await _paymentRepository.UpdateAsync(payment);
            _logger.LogInformation("Payment {PaymentIntentId} refunded", paymentIntentId);
        }

        private async Task<string> GetOrCreateStripeCustomerAsync(User user)
        {
            // Check if user already has a Stripe customer ID
            if (!string.IsNullOrEmpty(user.StripeCustomerId))
            {
                return user.StripeCustomerId;
            }

            // Create new Stripe customer
            var customerOptions = new CustomerCreateOptions
            {
                Email = user.Email,
                Name = user.FullName,
                Metadata = new Dictionary<string, string>
                {
                    { "user_id", user.Id.ToString() }
                }
            };

            var customer = await _customerService.CreateAsync(customerOptions);

            // Update user with Stripe customer ID
            user.StripeCustomerId = customer.Id;
            await _userRepository.UpdateAsync(user);

            return customer.Id;
        }

        private async Task LogTransactionAsync(int paymentId, string stripeEventId, string eventType, string? status, string? rawData)
        {
            var transaction = new PaymentTransaction
            {
                PaymentId = paymentId,
                StripeEventId = stripeEventId,
                EventType = eventType,
                Status = status,
                RawData = rawData,
                CreatedDate = DateTime.UtcNow
            };

            await _paymentRepository.AddTransactionAsync(transaction);
        }

        private static PaymentResponse MapToPaymentResponse(Payment payment)
        {
            return new PaymentResponse
            {
                Id = payment.Id,
                TravelPlanId = payment.TravelPlanId,
                UserId = payment.UserId,
                StripePaymentIntentId = payment.StripePaymentIntentId,
                StripeCustomerId = payment.StripeCustomerId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                PaymentMethod = payment.PaymentMethod,
                Description = payment.Description,
                ReceiptUrl = payment.ReceiptUrl,
                CreatedDate = payment.CreatedDate,
                CompletedDate = payment.CompletedDate
            };
        }
    }
}
