using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class AccommodationConfiguration : IEntityTypeConfiguration<Accommodation>
    {
        public void Configure(EntityTypeBuilder<Accommodation> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Type).IsRequired().HasMaxLength(50); // e.g., "Hotel", "Hostel", "Apartment"
            builder.Property(a => a.Address).IsRequired().HasMaxLength(200);
            builder.Property(a => a.CheckInDate).IsRequired();
            builder.Property(a => a.CheckOutDate).IsRequired();
            builder.Property(a => a.CostPerNight).HasColumnType("decimal(18,2)");
            
            // Foreign key relationship
            builder.HasOne(a => a.TravelPlan)
                   .WithMany(tp => tp.Accommodations)
                   .HasForeignKey(a => a.TravelPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            //seed data 
            //builder.HasData(
            //    new Accommodation
            //    {
            //        Id = 1,
            //        TravelPlanId = 1,
            //        Name = "Hotel Le Meurice",
            //        Description = "Luxury hotel located in the heart of Paris.",
            //        Address = "228 Rue de Rivoli, 75001 Paris, France",
            //        CheckInDate = new DateTime(2024, 6, 1),
            //        CheckOutDate = new DateTime(2024, 6, 10),
            //        CostPerNight = 450.00m,
            //        Type = "Hotel"
            //    },
            //    new Accommodation
            //    {
            //        Id = 2,
            //        TravelPlanId = 1,
            //        Name = "Charming Apartment in Montmartre",
            //        Description = "Cozy apartment with a view of the city.",
            //        Address = "12 Rue Lepic, 75018 Paris, France",
            //        CheckInDate = new DateTime(2024, 6, 5),
            //        CheckOutDate = new DateTime(2024, 6, 10),
            //        CostPerNight = 150.00m,
            //        Type = "Apartment"
            //    }
            //);
        }
    }
}
