using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AITravelPlanner.Infrastructure.Data.Configurations
{
        public class TravelPlanConfiguration : IEntityTypeConfiguration<TravelPlan> 
            {
              public void Configure(EntityTypeBuilder<TravelPlan> builder)
              {
                  builder.HasKey(tp => tp.Id);
                  builder.Property(tp => tp.Destination).IsRequired().HasMaxLength(100);
                  builder.Property(tp => tp.Title).IsRequired().HasMaxLength(100);
                  builder.Property(tp => tp.StartDate).IsRequired();
                  builder.Property(tp => tp.EndDate).IsRequired();
                  builder.Property(tp => tp.Description).HasMaxLength(1000);
                  builder.Property(tp => tp.AIRecommendations).HasMaxLength(1000);
                  builder.Property(tp => tp.Budget).HasColumnType("decimal(18,2)");
                  builder.Property(tp => tp.TravelStyle).HasMaxLength(50);
                  builder.Property(tp => tp.GroupSize).HasMaxLength(50);
                  builder.Property(tp => tp.IsPublic).IsRequired().HasDefaultValue(false);
                  builder.Property(tp => tp.CreatedDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");
                  builder.Property(tp => tp.UpdatedDate);
                  // Relationships
                  builder.HasMany(tp => tp.Activities)
                         .WithOne(a=>a.TravelPlan)
                         .HasForeignKey(a => a.TravelPlanId)
                         .OnDelete(DeleteBehavior.Cascade);
                  builder.HasMany(tp => tp.Accommodations)
                         .WithOne(a=>a.TravelPlan)
                         .HasForeignKey(a => a.TravelPlanId)
                         .OnDelete(DeleteBehavior.Cascade);
                  builder.HasMany(tp => tp.Transportations)
                         .WithOne(a=>a.TravelPlan)
                         .HasForeignKey(t => t.TravelPlanId)
                         .OnDelete(DeleteBehavior.Cascade);
                  // User relationship
                  builder.HasOne(tp => tp.User)
                         .WithMany(u => u.TravelPlans)
                         .HasForeignKey(tp => tp.UserId)
                         .OnDelete(DeleteBehavior.SetNull);
            //builder.HasData(
            //    new TravelPlan
            //    {
            //        Id = 1,
            //        Destination = "Paris, France",
            //        Title = "Romantic Getaway",
            //        StartDate = new DateTime(2024, 6, 1),
            //        EndDate = new DateTime(2024, 6, 10),
            //        Description = "A romantic trip to explore the city of love.",
            //        AIRecommendations = "Visit the Eiffel Tower, Louvre Museum, and enjoy a Seine River cruise.",
            //        Budget = 3000.00m,
            //        TravelStyle = "Luxury",
            //        GroupSize = "Couple",
            //        IsPublic = false,
            //        CreatedDate = new DateTime(2024, 06, 01),
            //        UserId = null // Assuming this travel plan is associated with a user with Id 1
            //    },
            //    new TravelPlan
            //    {
            //        Id = 2,
            //        Destination = "Tokyo, Japan",
            //        Title = "Cultural Exploration",
            //        StartDate = new DateTime(2024, 9, 15),
            //        EndDate = new DateTime(2024, 9, 25),
            //        Description = "Experience the rich culture and history of Japan.",
            //        AIRecommendations = "Visit temples in Asakusa, explore Akihabara for tech and anime, and enjoy sushi in Tsukiji.",
            //        Budget = 2500.00m,
            //        TravelStyle = "Cultural",
            //        GroupSize = "Solo",
            //        IsPublic = true,
            //        CreatedDate = new DateTime(2024, 06, 01),
            //        UserId = null // Assuming this travel plan is associated with a user with Id 2
            //    }
            //);
        }          
    }
    }

