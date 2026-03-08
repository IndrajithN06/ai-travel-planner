using System;
using System.ComponentModel.DataAnnotations;

namespace AITravelPlanner.Domain.DTOs
{
    public class CreatePaymentIntentRequest
    {
        [Required]
        public int TravelPlanId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";
        
        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class PaymentIntentResponse
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }

    public class ConfirmPaymentRequest
    {
        [Required]
        public string PaymentIntentId { get; set; } = string.Empty;
    }

    public class PaymentResponse
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }
        public int UserId { get; set; }
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public string? StripeCustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? Description { get; set; }
        public string? ReceiptUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class PaymentStatusResponse
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime? CompletedDate { get; set; }
        public string? ReceiptUrl { get; set; }
    }
}
