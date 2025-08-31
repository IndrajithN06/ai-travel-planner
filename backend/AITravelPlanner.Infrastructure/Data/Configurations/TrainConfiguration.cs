using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class TrainConfiguration : IEntityTypeConfiguration<Train>
    {
        public void Configure(EntityTypeBuilder<Train> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.TrainName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.TrainNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(t => t.FromLocation)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.ToLocation)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(t => t.DepartureDate)
                   .IsRequired();

            builder.Property(t => t.DepartureTime)
                   .IsRequired();

            builder.Property(t => t.ArrivalTime)
                   .IsRequired();

            builder.Property(t => t.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
        }
    }
}
