using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface ITravelPlanRepository
    {
        // TravelPlan CRUD operations
        Task<TravelPlan> CreateAsync(TravelPlan travelPlan);
        Task<TravelPlan?> GetByIdAsync(int id);
        Task<TravelPlan?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<TravelPlan>> GetAllAsync();
        Task<IEnumerable<TravelPlan>> GetPublicPlansAsync();
        Task<IEnumerable<TravelPlan>> GetByDestinationAsync(string destination);
        Task<IEnumerable<TravelPlan>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TravelPlan>> GetByTravelStyleAsync(string travelStyle);
        Task<TravelPlan> UpdateAsync(TravelPlan travelPlan);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();

        // Activity operations
        Task<Activity> AddActivityAsync(Activity activity);
        Task<Activity?> GetActivityByIdAsync(int id);
        Task<IEnumerable<Activity>> GetActivitiesByPlanIdAsync(int travelPlanId);
        Task<Activity> UpdateActivityAsync(Activity activity);
        Task<bool> DeleteActivityAsync(int id);

        // Accommodation operations
        Task<Accommodation> AddAccommodationAsync(Accommodation accommodation);
        Task<Accommodation?> GetAccommodationByIdAsync(int id);
        Task<IEnumerable<Accommodation>> GetAccommodationsByPlanIdAsync(int travelPlanId);
        Task<Accommodation> UpdateAccommodationAsync(Accommodation accommodation);
        Task<bool> DeleteAccommodationAsync(int id);

        // Transportation operations
        Task<Transportation> AddTransportationAsync(Transportation transportation);
        Task<Transportation?> GetTransportationByIdAsync(int id);
        Task<IEnumerable<Transportation>> GetTransportationsByPlanIdAsync(int travelPlanId);
        Task<Transportation> UpdateTransportationAsync(Transportation transportation);
        Task<bool> DeleteTransportationAsync(int id);
    }
}