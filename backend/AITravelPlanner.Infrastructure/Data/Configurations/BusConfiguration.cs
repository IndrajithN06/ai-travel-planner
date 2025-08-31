using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            // Primary Key
            builder.HasKey(b => b.Id);

            // Properties
            builder.Property(b => b.BusName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.FromLocation)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.ToLocation)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.DepartureTime)
                   .IsRequired();

            builder.Property(b => b.ArrivalTime)
                   .IsRequired();

            builder.Property(b => b.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)"); // specify precision for SQL Server
        }
    }
}
