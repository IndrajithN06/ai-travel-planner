using System;
using System.Collections.Generic;

namespace AITravelPlanner.Domain.Entities
{
    public class TravelPlan
    {
        public int Id { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? AIRecommendations { get; set; }
        public decimal? Budget { get; set; }
        public string? TravelStyle { get; set; }
        public string? GroupSize { get; set; }
        public bool IsPublic { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // User relationship
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        // Navigation properties
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public virtual ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
        public virtual ICollection<Transportation> Transportations { get; set; } = new List<Transportation>();

    }

    public class Activity
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Location { get; set; }
        public decimal? Cost { get; set; }
        public string? Category { get; set; }
        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }

    public class Accommodation
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal? CostPerNight { get; set; }
        public string? Type { get; set; }
        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }

    public class Transportation
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Provider { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }
}
