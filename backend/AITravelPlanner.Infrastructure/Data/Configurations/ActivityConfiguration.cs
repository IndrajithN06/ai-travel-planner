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
    public class Activityconfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Description).HasMaxLength(1000);
            builder.Property(a => a.Location).HasMaxLength(200);
            builder.Property(a => a.ScheduledDate).IsRequired();
            builder.Property(a => a.Duration).IsRequired();
            builder.Property(a => a.Cost).HasColumnType("decimal(18,2)");
            // Foreign key relationship
            builder.HasOne(a => a.TravelPlan)
                   .WithMany(tp => tp.Activities)
                   .HasForeignKey(a => a.TravelPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            //see data
            //builder.HasData(
            //    new Activity
            //    {
            //        Id = 1,
            //        TravelPlanId = 1,
            //        Name = "Eiffel Tower Visit",
            //        Description = "Visit the iconic Eiffel Tower and enjoy panoramic views of Paris.",
            //        ScheduledDate = new DateTime(2024, 6, 2),
            //        Duration = TimeSpan.FromHours(2),
            //        Location = "Eiffel Tower, Paris",
            //        Cost = 25.00m,
            //        Category = "Sightseeing"
            //    },
            //    new Activity
            //    {
            //        Id = 2,
            //        TravelPlanId = 1,
            //        Name = "Louvre Museum Tour",
            //        Description = "Explore the world-famous Louvre Museum and see the Mona Lisa.",
            //        ScheduledDate = new DateTime(2024, 6, 3),
            //        Duration = TimeSpan.FromHours(3),
            //        Location = "Louvre Museum, Paris",
            //        Cost = 30.00m,
            //        Category = "Culture"
            //    },
            //    new Activity
            //    {
            //        Id = 3,
            //        TravelPlanId = 1,
            //        Name = "Seine River Cruise",
            //        Description = "Enjoy a romantic cruise along the Seine River at sunset.",
            //        ScheduledDate = new DateTime(2024, 6, 4),
            //        Duration = TimeSpan.FromHours(1.5),
            //        Location = "Seine River, Paris",
            //        Cost = 40.00m,
            //        Category = "Sightseeing"
            //    }
            //);
        }
    }
}
