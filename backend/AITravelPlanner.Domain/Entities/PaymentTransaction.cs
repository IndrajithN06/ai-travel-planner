using System;
using System.ComponentModel.DataAnnotations;

namespace AITravelPlanner.Domain.Entities
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        
        [Required]
        public int PaymentId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string EventType { get; set; } = string.Empty; // payment_intent.created, payment_intent.succeeded, etc.
        
        [Required]
        [MaxLength(255)]
        public string StripeEventId { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? Status { get; set; }
        
        public string? RawData { get; set; } // JSON payload from Stripe
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual Payment Payment { get; set; } = null!;
    }
}
