using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Application.Services
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly ITravelPlanRepository _repository;

        public TravelPlanService(ITravelPlanRepository repository)
        {
            _repository = repository;
        }

        // TravelPlan operations
        public async Task<TravelPlanResponse> CreateTravelPlanAsync(CreateTravelPlanRequest request)
        {
            var travelPlan = new TravelPlan
            {
                Destination = request.Destination,
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description,
                Budget = request.Budget,
                TravelStyle = request.TravelStyle,
                GroupSize = request.GroupSize,
                IsPublic = request.IsPublic,
                CreatedDate = DateTime.UtcNow
            };

            var createdPlan = await _repository.CreateAsync(travelPlan);
            return MapToTravelPlanResponse(createdPlan);
        }

        public async Task<TravelPlanResponse?> GetTravelPlanByIdAsync(int id)
        {
            var travelPlan = await _repository.GetByIdWithDetailsAsync(id);
            return travelPlan != null ? MapToTravelPlanResponse(travelPlan) : null;
        }

        public async Task<IEnumerable<TravelPlanResponse>> GetAllTravelPlansAsync()
        {
            var travelPlans = await _repository.GetAllAsync();
            return travelPlans.Select(MapToTravelPlanResponse);
        }

        public async Task<IEnumerable<TravelPlanResponse>> GetPublicTravelPlansAsync()
        {
            var travelPlans = await _repository.GetPublicPlansAsync();
            return travelPlans.Select(MapToTravelPlanResponse);
        }

        public async Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByDestinationAsync(string destination)
        {
            var travelPlans = await _repository.GetByDestinationAsync(destination);
            return travelPlans.Select(MapToTravelPlanResponse);
        }

        public async Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var travelPlans = await _repository.GetByDateRangeAsync(startDate, endDate);
            return travelPlans.Select(MapToTravelPlanResponse);
        }

        public async Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByTravelStyleAsync(string travelStyle)
        {
            var travelPlans = await _repository.GetByTravelStyleAsync(travelStyle);
            return travelPlans.Select(MapToTravelPlanResponse);
        }

        public async Task<TravelPlanResponse> UpdateTravelPlanAsync(int id, UpdateTravelPlanRequest request)
        {
            var existingPlan = await _repository.GetByIdAsync(id);
            if (existingPlan == null)
                throw new ArgumentException($"Travel plan with ID {id} not found.");

            existingPlan.Destination = request.Destination;
            existingPlan.Title = request.Title;
            existingPlan.StartDate = request.StartDate;
            existingPlan.EndDate = request.EndDate;
            existingPlan.Description = request.Description;
            existingPlan.AIRecommendations = request.AIRecommendations;
            existingPlan.Budget = request.Budget;
            existingPlan.TravelStyle = request.TravelStyle;
            existingPlan.GroupSize = request.GroupSize;
            existingPlan.IsPublic = request.IsPublic;
            existingPlan.UpdatedDate = DateTime.UtcNow;

            var updatedPlan = await _repository.UpdateAsync(existingPlan);
            return MapToTravelPlanResponse(updatedPlan);
        }

        public async Task<bool> DeleteTravelPlanAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // AI-powered travel plan generation
        public async Task<TravelPlanResponse> GenerateTravelPlanAsync(GenerateTravelPlanRequest request)
        {
            // TODO: Integrate with AI service for generating recommendations
            var aiRecommendations = await GenerateAIRecommendations(request);

            var travelPlan = new TravelPlan
            {
                Destination = request.Destination,
                Title = $"AI Generated Plan for {request.Destination}",
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TravelStyle = request.TravelStyle,
                GroupSize = request.GroupSize,
                Budget = request.Budget,
                AIRecommendations = aiRecommendations,
                IsPublic = false,
                CreatedDate = DateTime.UtcNow
            };

            var createdPlan = await _repository.CreateAsync(travelPlan);
            return MapToTravelPlanResponse(createdPlan);
        }

        // Activity operations
        public async Task<ActivityDto> AddActivityAsync(int travelPlanId, ActivityDto activityDto)
        {
            var activity = new Activity
            {
                TravelPlanId = travelPlanId,
                Name = activityDto.Name,
                Description = activityDto.Description,
                ScheduledDate = activityDto.ScheduledDate,
                Duration = activityDto.Duration,
                Location = activityDto.Location,
                Cost = activityDto.Cost,
                Category = activityDto.Category
            };

            var createdActivity = await _repository.AddActivityAsync(activity);
            return MapToActivityDto(createdActivity);
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _repository.GetActivityByIdAsync(id);
            return activity != null ? MapToActivityDto(activity) : null;
        }

        public async Task<IEnumerable<ActivityDto>> GetActivitiesByPlanIdAsync(int travelPlanId)
        {
            var activities = await _repository.GetActivitiesByPlanIdAsync(travelPlanId);
            return activities.Select(MapToActivityDto);
        }

        public async Task<ActivityDto> UpdateActivityAsync(int id, ActivityDto activityDto)
        {
            var existingActivity = await _repository.GetActivityByIdAsync(id);
            if (existingActivity == null)
                throw new ArgumentException($"Activity with ID {id} not found.");

            existingActivity.Name = activityDto.Name;
            existingActivity.Description = activityDto.Description;
            existingActivity.ScheduledDate = activityDto.ScheduledDate;
            existingActivity.Duration = activityDto.Duration;
            existingActivity.Location = activityDto.Location;
            existingActivity.Cost = activityDto.Cost;
            existingActivity.Category = activityDto.Category;

            var updatedActivity = await _repository.UpdateActivityAsync(existingActivity);
            return MapToActivityDto(updatedActivity);
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            return await _repository.DeleteActivityAsync(id);
        }

        // Accommodation operations
        public async Task<AccommodationDto> AddAccommodationAsync(int travelPlanId, AccommodationDto accommodationDto)
        {
            var accommodation = new Accommodation
            {
                TravelPlanId = travelPlanId,
                Name = accommodationDto.Name,
                Description = accommodationDto.Description,
                Address = accommodationDto.Address,
                CheckInDate = accommodationDto.CheckInDate,
                CheckOutDate = accommodationDto.CheckOutDate,
                CostPerNight = accommodationDto.CostPerNight,
                Type = accommodationDto.Type
            };

            var createdAccommodation = await _repository.AddAccommodationAsync(accommodation);
            return MapToAccommodationDto(createdAccommodation);
        }

        public async Task<AccommodationDto?> GetAccommodationByIdAsync(int id)
        {
            var accommodation = await _repository.GetAccommodationByIdAsync(id);
            return accommodation != null ? MapToAccommodationDto(accommodation) : null;
        }

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsByPlanIdAsync(int travelPlanId)
        {
            var accommodations = await _repository.GetAccommodationsByPlanIdAsync(travelPlanId);
            return accommodations.Select(MapToAccommodationDto);
        }

        public async Task<AccommodationDto> UpdateAccommodationAsync(int id, AccommodationDto accommodationDto)
        {
            var existingAccommodation = await _repository.GetAccommodationByIdAsync(id);
            if (existingAccommodation == null)
                throw new ArgumentException($"Accommodation with ID {id} not found.");

            existingAccommodation.Name = accommodationDto.Name;
            existingAccommodation.Description = accommodationDto.Description;
            existingAccommodation.Address = accommodationDto.Address;
            existingAccommodation.CheckInDate = accommodationDto.CheckInDate;
            existingAccommodation.CheckOutDate = accommodationDto.CheckOutDate;
            existingAccommodation.CostPerNight = accommodationDto.CostPerNight;
            existingAccommodation.Type = accommodationDto.Type;

            var updatedAccommodation = await _repository.UpdateAccommodationAsync(existingAccommodation);
            return MapToAccommodationDto(updatedAccommodation);
        }

        public async Task<bool> DeleteAccommodationAsync(int id)
        {
            return await _repository.DeleteAccommodationAsync(id);
        }

        // Transportation operations
        public async Task<TransportationDto> AddTransportationAsync(int travelPlanId, TransportationDto transportationDto)
        {
            var transportation = new Transportation
            {
                TravelPlanId = travelPlanId,
                Type = transportationDto.Type,
                Provider = transportationDto.Provider,
                FromLocation = transportationDto.FromLocation,
                ToLocation = transportationDto.ToLocation,
                DepartureTime = transportationDto.DepartureTime,
                ArrivalTime = transportationDto.ArrivalTime,
                Cost = transportationDto.Cost,
                Notes = transportationDto.Notes
            };

            var createdTransportation = await _repository.AddTransportationAsync(transportation);
            return MapToTransportationDto(createdTransportation);
        }

        public async Task<TransportationDto?> GetTransportationByIdAsync(int id)
        {
            var transportation = await _repository.GetTransportationByIdAsync(id);
            return transportation != null ? MapToTransportationDto(transportation) : null;
        }

        public async Task<IEnumerable<TransportationDto>> GetTransportationsByPlanIdAsync(int travelPlanId)
        {
            var transportations = await _repository.GetTransportationsByPlanIdAsync(travelPlanId);
            return transportations.Select(MapToTransportationDto);
        }

        public async Task<TransportationDto> UpdateTransportationAsync(int id, TransportationDto transportationDto)
        {
            var existingTransportation = await _repository.GetTransportationByIdAsync(id);
            if (existingTransportation == null)
                throw new ArgumentException($"Transportation with ID {id} not found.");

            existingTransportation.Type = transportationDto.Type;
            existingTransportation.Provider = transportationDto.Provider;
            existingTransportation.FromLocation = transportationDto.FromLocation;
            existingTransportation.ToLocation = transportationDto.ToLocation;
            existingTransportation.DepartureTime = transportationDto.DepartureTime;
            existingTransportation.ArrivalTime = transportationDto.ArrivalTime;
            existingTransportation.Cost = transportationDto.Cost;
            existingTransportation.Notes = transportationDto.Notes;

            var updatedTransportation = await _repository.UpdateTransportationAsync(existingTransportation);
            return MapToTransportationDto(updatedTransportation);
        }

        public async Task<bool> DeleteTransportationAsync(int id)
        {
            return await _repository.DeleteTransportationAsync(id);
        }

        // Private helper methods
        private static TravelPlanResponse MapToTravelPlanResponse(TravelPlan travelPlan)
        {
            return new TravelPlanResponse
            {
                Id = travelPlan.Id,
                Destination = travelPlan.Destination,
                Title = travelPlan.Title,
                StartDate = travelPlan.StartDate,
                EndDate = travelPlan.EndDate,
                Description = travelPlan.Description,
                AIRecommendations = travelPlan.AIRecommendations,
                Budget = travelPlan.Budget,
                TravelStyle = travelPlan.TravelStyle,
                GroupSize = travelPlan.GroupSize,
                IsPublic = travelPlan.IsPublic,
                CreatedDate = travelPlan.CreatedDate,
                UpdatedDate = travelPlan.UpdatedDate,
                Activities = travelPlan.Activities?.Select(MapToActivityDto).ToList() ?? new List<ActivityDto>(),
                Accommodations = travelPlan.Accommodations?.Select(MapToAccommodationDto).ToList() ?? new List<AccommodationDto>(),
                Transportations = travelPlan.Transportations?.Select(MapToTransportationDto).ToList() ?? new List<TransportationDto>()
            };
        }

        private static ActivityDto MapToActivityDto(Activity activity)
        {
            return new ActivityDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                ScheduledDate = activity.ScheduledDate,
                Duration = activity.Duration,
                Location = activity.Location,
                Cost = activity.Cost,
                Category = activity.Category
            };
        }

        private static AccommodationDto MapToAccommodationDto(Accommodation accommodation)
        {
            return new AccommodationDto
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Description = accommodation.Description,
                Address = accommodation.Address,
                CheckInDate = accommodation.CheckInDate,
                CheckOutDate = accommodation.CheckOutDate,
                CostPerNight = accommodation.CostPerNight,
                Type = accommodation.Type
            };
        }

        private static TransportationDto MapToTransportationDto(Transportation transportation)
        {
            return new TransportationDto
            {
                Id = transportation.Id,
                Type = transportation.Type,
                Provider = transportation.Provider,
                FromLocation = transportation.FromLocation,
                ToLocation = transportation.ToLocation,
                DepartureTime = transportation.DepartureTime,
                ArrivalTime = transportation.ArrivalTime,
                Cost = transportation.Cost,
                Notes = transportation.Notes
            };
        }

        private async Task<string> GenerateAIRecommendations(GenerateTravelPlanRequest request)
        {
            // TODO: Implement AI integration
            // This is a placeholder implementation
            await Task.Delay(100); // Simulate async operation

            var recommendations = new List<string>
            {
                $"Based on your {request.TravelStyle ?? "travel"} style and {request.GroupSize ?? "group"} size, here are some recommendations for {request.Destination}:",
                "• Visit the main attractions during off-peak hours to avoid crowds",
                "• Consider local transportation options for authentic experience",
                "• Try local cuisine at recommended restaurants",
                "• Book accommodations in advance, especially during peak season"
            };

            if (request.Budget.HasValue)
            {
                recommendations.Add($"• With your budget of ${request.Budget}, consider these cost-saving tips:");
                recommendations.Add("  - Use public transportation instead of taxis");
                recommendations.Add("  - Look for free walking tours");
                recommendations.Add("  - Book activities in advance for better rates");
            }

            return string.Join("\n", recommendations);
        }
    }
}
