using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.TravelPlanId).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.StripePaymentIntentId).IsRequired().HasMaxLength(255);
            builder.Property(p => p.StripeCustomerId).HasMaxLength(255);
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Currency).IsRequired().HasMaxLength(10).HasDefaultValue("USD");
            builder.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("pending");
            builder.Property(p => p.PaymentMethod).HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.ReceiptUrl).HasMaxLength(500);
            builder.Property(p => p.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.CompletedDate);
            
            // Relationships
            builder.HasOne(p => p.TravelPlan)
                   .WithMany()
                   .HasForeignKey(p => p.TravelPlanId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(p => p.Transactions)
                   .WithOne(t => t.Payment)
                   .HasForeignKey(t => t.PaymentId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            // Indexes
            builder.HasIndex(p => p.StripePaymentIntentId).IsUnique();
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.TravelPlanId);
            builder.HasIndex(p => p.Status);
        }
    }
}
