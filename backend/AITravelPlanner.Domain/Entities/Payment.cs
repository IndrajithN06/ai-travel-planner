using System;
using System.ComponentModel.DataAnnotations;

namespace AITravelPlanner.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        
        [Required]
        public int TravelPlanId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        // Stripe-specific fields
        [Required]
        [MaxLength(255)]
        public string StripePaymentIntentId { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string? StripeCustomerId { get; set; }
        
        // Payment details
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "pending"; // pending, succeeded, failed, canceled, refunded
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; } // card, paypal, etc.
        
        // Metadata
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [MaxLength(500)]
        public string? ReceiptUrl { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedDate { get; set; }
        
        // Navigation properties
        public virtual TravelPlan TravelPlan { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
    }
}
