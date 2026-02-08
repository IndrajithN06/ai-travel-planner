using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(pt => pt.Id);
            
            builder.Property(pt => pt.PaymentId).IsRequired();
            builder.Property(pt => pt.EventType).IsRequired().HasMaxLength(100);
            builder.Property(pt => pt.StripeEventId).IsRequired().HasMaxLength(255);
            builder.Property(pt => pt.Status).HasMaxLength(50);
            builder.Property(pt => pt.RawData).HasColumnType("nvarchar(max)");
            builder.Property(pt => pt.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            
            // Relationships
            builder.HasOne(pt => pt.Payment)
                   .WithMany(p => p.Transactions)
                   .HasForeignKey(pt => pt.PaymentId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            // Indexes
            builder.HasIndex(pt => pt.StripeEventId).IsUnique();
            builder.HasIndex(pt => pt.PaymentId);
            builder.HasIndex(pt => pt.EventType);
        }
    }
}
