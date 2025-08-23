using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AITravelPlanner.Domain.Entities
{
    public class TravelPlan
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? AIRecommendations { get; set; }

        public decimal? Budget { get; set; }

        [MaxLength(50)]
        public string? TravelStyle { get; set; } // e.g., "Budget", "Luxury", "Adventure", "Cultural"

        [MaxLength(50)]
        public string? GroupSize { get; set; } // e.g., "Solo", "Couple", "Family", "Group"

        public bool IsPublic { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties for related entities
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public virtual ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
        public virtual ICollection<Transportation> Transportations { get; set; } = new List<Transportation>();
    }

    public class Activity
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime? ScheduledDate { get; set; }
        public TimeSpan? Duration { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public decimal? Cost { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; } // e.g., "Sightseeing", "Food", "Adventure", "Culture"

        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }

    public class Accommodation
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public decimal? CostPerNight { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; } // e.g., "Hotel", "Hostel", "Apartment", "Resort"

        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }

    public class Transportation
    {
        public int Id { get; set; }
        public int TravelPlanId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty; // e.g., "Flight", "Train", "Bus", "Car", "Walking"

        [MaxLength(100)]
        public string? Provider { get; set; }

        [MaxLength(100)]
        public string? FromLocation { get; set; }

        [MaxLength(100)]
        public string? ToLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public decimal? Cost { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public virtual TravelPlan TravelPlan { get; set; } = null!;
    }
}