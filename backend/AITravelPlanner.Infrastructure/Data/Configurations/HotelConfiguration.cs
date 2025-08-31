using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            // Primary Key
            builder.HasKey(h => h.Id);

            // Properties
            builder.Property(h => h.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(h => h.City)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(h => h.Address)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(h => h.PricePerNight)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(h => h.IsAvailable)
                   .IsRequired();

            builder.Property(h => h.AvailableRooms)
                   .IsRequired();

            builder.Property(h => h.AvailableFrom)
                   .IsRequired();

            builder.Property(h => h.AvailableTo)
                   .IsRequired();
        }
    }
}
