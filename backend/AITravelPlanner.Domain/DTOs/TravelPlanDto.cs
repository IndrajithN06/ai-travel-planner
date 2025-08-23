using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AITravelPlanner.Domain.DTOs
{
    public class CreateTravelPlanRequest
    {
        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public decimal? Budget { get; set; }

        [MaxLength(50)]
        public string? TravelStyle { get; set; }

        [MaxLength(50)]
        public string? GroupSize { get; set; }

        public bool IsPublic { get; set; } = false;
    }

    public class UpdateTravelPlanRequest
    {
        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? AIRecommendations { get; set; }

        public decimal? Budget { get; set; }

        [MaxLength(50)]
        public string? TravelStyle { get; set; }

        [MaxLength(50)]
        public string? GroupSize { get; set; }

        public bool IsPublic { get; set; }
    }

    public class TravelPlanResponse
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
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ActivityDto> Activities { get; set; } = new();
        public List<AccommodationDto> Accommodations { get; set; } = new();
        public List<TransportationDto> Transportations { get; set; } = new();
    }

    public class ActivityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Location { get; set; }
        public decimal? Cost { get; set; }
        public string? Category { get; set; }
    }

    public class AccommodationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal? CostPerNight { get; set; }
        public string? Type { get; set; }
    }

    public class TransportationDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Provider { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
    }

    public class GenerateTravelPlanRequest
    {
        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(50)]
        public string? TravelStyle { get; set; }

        [MaxLength(50)]
        public string? GroupSize { get; set; }

        public decimal? Budget { get; set; }

        [MaxLength(500)]
        public string? Preferences { get; set; } // Additional preferences for AI generation
    }
} 