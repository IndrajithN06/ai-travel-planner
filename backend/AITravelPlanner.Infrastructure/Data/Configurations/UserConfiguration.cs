using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.FullName).HasMaxLength(100);
            builder.Property(u => u.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(u => u.UpdatedDate);
            // Relationships
            builder.HasMany(u => u.TravelPlans)
                   .WithOne(tp => tp.User)
                   .HasForeignKey(tp => tp.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
