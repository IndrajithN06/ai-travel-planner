using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
    public class TransportationConfiguration : IEntityTypeConfiguration<Transportation>
    {
        public void Configure(EntityTypeBuilder<Transportation> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Type).IsRequired().HasMaxLength(50); // e.g., "Flight", "Train", "Car Rental"
            builder.Property(t => t.Provider).IsRequired().HasMaxLength(100);
            builder.Property(t => t.FromLocation).IsRequired().HasMaxLength(200);
            builder.Property(t => t.ArrivalTime).IsRequired().HasMaxLength(200);
            builder.Property(t => t.DepartureTime).IsRequired();
            builder.Property(t => t.ArrivalTime).IsRequired();
            builder.Property(t => t.Cost).HasColumnType("decimal(18,2)");
            
            // Foreign key relationship
            builder.HasOne(t => t.TravelPlan)
                   .WithMany(tp => tp.Transportations)
                   .HasForeignKey(t => t.TravelPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            //seed data 
            //builder.HasData(
            //    new Transportation
            //    {
            //        Id = 1,
            //        TravelPlanId = 1,
            //        Type = "Flight",
            //        Provider = "Air France",
            //        FromLocation = "New York (JFK)",
            //        ToLocation = "Paris (CDG)",
            //        DepartureTime = new DateTime(2024, 6, 1, 19, 0, 0),
            //        ArrivalTime = new DateTime(2024, 6, 2, 8, 0, 0),
            //        Cost = 800.00m,
            //        Notes = "Non-stop flight with in-flight entertainment and meals included."
            //    },
            //    new Transportation
            //    {
            //        Id = 2,
            //        TravelPlanId = 1,
            //        Type = "Train",
            //        Provider = "SNCF",
            //        FromLocation = "Paris Gare du Nord",
            //        ToLocation = "Versailles Château Rive Gauche",
            //        DepartureTime = new DateTime(2024, 6, 4, 10, 0, 0),
            //        ArrivalTime = new DateTime(2024, 6, 4, 10, 30, 0),
            //        Cost = 15.00m,
            //        Notes = "High-speed train with comfortable seating and free Wi-Fi."
            //    }
            //);
        }
    }
}
