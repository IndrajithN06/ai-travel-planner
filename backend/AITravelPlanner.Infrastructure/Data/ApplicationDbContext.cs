using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TravelPlan> TravelPlans { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Transportation> Transportations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TravelPlan configuration
            modelBuilder.Entity<TravelPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Destination).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.AIRecommendations).HasMaxLength(1000);
                entity.Property(e => e.TravelStyle).HasMaxLength(50);
                entity.Property(e => e.GroupSize).HasMaxLength(50);
                entity.Property(e => e.Budget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            });

            // Activity configuration
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Duration).HasColumnType("time");
                
                entity.HasOne(e => e.TravelPlan)
                    .WithMany(e => e.Activities)
                    .HasForeignKey(e => e.TravelPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Accommodation configuration
            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.CostPerNight).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.TravelPlan)
                    .WithMany(e => e.Accommodations)
                    .HasForeignKey(e => e.TravelPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Transportation configuration
            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Provider).HasMaxLength(100);
                entity.Property(e => e.FromLocation).HasMaxLength(100);
                entity.Property(e => e.ToLocation).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.TravelPlan)
                    .WithMany(e => e.Transportations)
                    .HasForeignKey(e => e.TravelPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed some sample data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Sample Travel Plans
            modelBuilder.Entity<TravelPlan>().HasData(
                new TravelPlan
                {
                    Id = 1,
                    Destination = "Paris, France",
                    Title = "Romantic Paris Getaway",
                    StartDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(37),
                    Description = "A romantic week in the City of Light",
                    TravelStyle = "Luxury",
                    GroupSize = "Couple",
                    Budget = 5000,
                    IsPublic = true,
                    CreatedDate = DateTime.UtcNow
                },
                new TravelPlan
                {
                    Id = 2,
                    Destination = "Tokyo, Japan",
                    Title = "Tokyo Adventure",
                    StartDate = DateTime.Now.AddDays(60),
                    EndDate = DateTime.Now.AddDays(67),
                    Description = "Exploring the vibrant culture of Tokyo",
                    TravelStyle = "Adventure",
                    GroupSize = "Solo",
                    Budget = 3000,
                    IsPublic = true,
                    CreatedDate = DateTime.UtcNow
                }
            );

            // Sample Activities
            modelBuilder.Entity<Activity>().HasData(
                new Activity
                {
                    Id = 1,
                    TravelPlanId = 1,
                    Name = "Eiffel Tower Visit",
                    Description = "Visit the iconic Eiffel Tower",
                    ScheduledDate = DateTime.Now.AddDays(30),
                    Duration = TimeSpan.FromHours(2),
                    Location = "Eiffel Tower, Paris",
                    Cost = 30,
                    Category = "Sightseeing"
                },
                new Activity
                {
                    Id = 2,
                    TravelPlanId = 1,
                    Name = "Louvre Museum",
                    Description = "Explore the world's largest art museum",
                    ScheduledDate = DateTime.Now.AddDays(31),
                    Duration = TimeSpan.FromHours(3),
                    Location = "Louvre Museum, Paris",
                    Cost = 17,
                    Category = "Culture"
                },
                new Activity
                {
                    Id = 3,
                    TravelPlanId = 2,
                    Name = "Shibuya Crossing",
                    Description = "Experience the world's busiest pedestrian crossing",
                    ScheduledDate = DateTime.Now.AddDays(60),
                    Duration = TimeSpan.FromHours(1),
                    Location = "Shibuya, Tokyo",
                    Cost = 0,
                    Category = "Sightseeing"
                }
            );

            // Sample Accommodations
            modelBuilder.Entity<Accommodation>().HasData(
                new Accommodation
                {
                    Id = 1,
                    TravelPlanId = 1,
                    Name = "Hotel Ritz Paris",
                    Description = "Luxury hotel in the heart of Paris",
                    Address = "15 Place Vendôme, 75001 Paris, France",
                    CheckInDate = DateTime.Now.AddDays(30),
                    CheckOutDate = DateTime.Now.AddDays(37),
                    CostPerNight = 800,
                    Type = "Hotel"
                },
                new Accommodation
                {
                    Id = 2,
                    TravelPlanId = 2,
                    Name = "Capsule Hotel",
                    Description = "Traditional Japanese capsule hotel experience",
                    Address = "Shibuya, Tokyo, Japan",
                    CheckInDate = DateTime.Now.AddDays(60),
                    CheckOutDate = DateTime.Now.AddDays(67),
                    CostPerNight = 50,
                    Type = "Hostel"
                }
            );

            // Sample Transportations
            modelBuilder.Entity<Transportation>().HasData(
                new Transportation
                {
                    Id = 1,
                    TravelPlanId = 1,
                    Type = "Flight",
                    Provider = "Air France",
                    FromLocation = "New York",
                    ToLocation = "Paris",
                    DepartureTime = DateTime.Now.AddDays(30).AddHours(10),
                    ArrivalTime = DateTime.Now.AddDays(30).AddHours(22),
                    Cost = 800,
                    Notes = "Direct flight to Charles de Gaulle Airport"
                },
                new Transportation
                {
                    Id = 2,
                    TravelPlanId = 2,
                    Type = "Flight",
                    Provider = "Japan Airlines",
                    FromLocation = "Los Angeles",
                    ToLocation = "Tokyo",
                    DepartureTime = DateTime.Now.AddDays(60).AddHours(12),
                    ArrivalTime = DateTime.Now.AddDays(61).AddHours(16),
                    Cost = 1200,
                    Notes = "Direct flight to Narita Airport"
                }
            );
        }
    }
} 