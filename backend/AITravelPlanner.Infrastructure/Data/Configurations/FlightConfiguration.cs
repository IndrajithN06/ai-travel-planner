using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight> 
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.FlightNumber).IsRequired().HasMaxLength(20);
            builder.Property(f => f.Airline).IsRequired().HasMaxLength(50);
            builder.Property(f => f.FromLocation).IsRequired().HasMaxLength(100);
            builder.Property(f => f.ToLocation).IsRequired().HasMaxLength(100);
            builder.Property(f => f.DepartureTime).IsRequired();
            builder.Property(f => f.ArrivalTime).IsRequired();
            builder.Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");
        }

    }
}
    