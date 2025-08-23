using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface ITravelPlanService
    {
        // TravelPlan operations
        Task<TravelPlanResponse> CreateTravelPlanAsync(CreateTravelPlanRequest request);
        Task<TravelPlanResponse?> GetTravelPlanByIdAsync(int id);
        Task<IEnumerable<TravelPlanResponse>> GetAllTravelPlansAsync();
        Task<IEnumerable<TravelPlanResponse>> GetPublicTravelPlansAsync();
        Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByDestinationAsync(string destination);
        Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TravelPlanResponse>> GetTravelPlansByTravelStyleAsync(string travelStyle);
        Task<TravelPlanResponse> UpdateTravelPlanAsync(int id, UpdateTravelPlanRequest request);
        Task<bool> DeleteTravelPlanAsync(int id);

        // AI-powered travel plan generation
        Task<TravelPlanResponse> GenerateTravelPlanAsync(GenerateTravelPlanRequest request);

        // Activity operations
        Task<ActivityDto> AddActivityAsync(int travelPlanId, ActivityDto activityDto);
        Task<ActivityDto?> GetActivityByIdAsync(int id);
        Task<IEnumerable<ActivityDto>> GetActivitiesByPlanIdAsync(int travelPlanId);
        Task<ActivityDto> UpdateActivityAsync(int id, ActivityDto activityDto);
        Task<bool> DeleteActivityAsync(int id);

        // Accommodation operations
        Task<AccommodationDto> AddAccommodationAsync(int travelPlanId, AccommodationDto accommodationDto);
        Task<AccommodationDto?> GetAccommodationByIdAsync(int id);
        Task<IEnumerable<AccommodationDto>> GetAccommodationsByPlanIdAsync(int travelPlanId);
        Task<AccommodationDto> UpdateAccommodationAsync(int id, AccommodationDto accommodationDto);
        Task<bool> DeleteAccommodationAsync(int id);

        // Transportation operations
        Task<TransportationDto> AddTransportationAsync(int travelPlanId, TransportationDto transportationDto);
        Task<TransportationDto?> GetTransportationByIdAsync(int id);
        Task<IEnumerable<TransportationDto>> GetTransportationsByPlanIdAsync(int travelPlanId);
        Task<TransportationDto> UpdateTransportationAsync(int id, TransportationDto transportationDto);
        Task<bool> DeleteTransportationAsync(int id);
    }
} 