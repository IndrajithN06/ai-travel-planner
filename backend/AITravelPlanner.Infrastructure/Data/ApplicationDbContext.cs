using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;
using System.Reflection;

namespace AITravelPlanner.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TravelPlan> TravelPlans { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Transportation> Transportations { get; set; }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Train> Trains { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Apply Custom Entity configurations

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);         
        }      
    }
} 