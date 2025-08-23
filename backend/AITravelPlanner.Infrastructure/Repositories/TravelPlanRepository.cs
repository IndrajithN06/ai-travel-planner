using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;
using AITravelPlanner.Infrastructure.Data;

namespace AITravelPlanner.Infrastructure.Repositories
{
    public class TravelPlanRepository : ITravelPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public TravelPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // TravelPlan CRUD operations
        public async Task<TravelPlan> CreateAsync(TravelPlan travelPlan)
        {
            _context.TravelPlans.Add(travelPlan);
            await _context.SaveChangesAsync();
            return travelPlan;
        }

        public async Task<TravelPlan?> GetByIdAsync(int id)
        {
            return await _context.TravelPlans.FindAsync(id);
        }

        public async Task<TravelPlan?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<IEnumerable<TravelPlan>> GetAllAsync()
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .OrderByDescending(tp => tp.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TravelPlan>> GetPublicPlansAsync()
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .Where(tp => tp.IsPublic)
                .OrderByDescending(tp => tp.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TravelPlan>> GetByDestinationAsync(string destination)
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .Where(tp => tp.Destination.Contains(destination, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(tp => tp.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TravelPlan>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .Where(tp => tp.StartDate >= startDate && tp.EndDate <= endDate)
                .OrderByDescending(tp => tp.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TravelPlan>> GetByTravelStyleAsync(string travelStyle)
        {
            return await _context.TravelPlans
                .Include(tp => tp.Activities)
                .Include(tp => tp.Accommodations)
                .Include(tp => tp.Transportations)
                .Where(tp => tp.TravelStyle == travelStyle)
                .OrderByDescending(tp => tp.CreatedDate)
                .ToListAsync();
        }

        public async Task<TravelPlan> UpdateAsync(TravelPlan travelPlan)
        {
            _context.TravelPlans.Update(travelPlan);
            await _context.SaveChangesAsync();
            return travelPlan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var travelPlan = await _context.TravelPlans.FindAsync(id);
            if (travelPlan == null)
                return false;

            _context.TravelPlans.Remove(travelPlan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TravelPlans.AnyAsync(tp => tp.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _context.TravelPlans.CountAsync();
        }

        // Activity operations
        public async Task<Activity> AddActivityAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity?> GetActivityByIdAsync(int id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByPlanIdAsync(int travelPlanId)
        {
            return await _context.Activities
                .Where(a => a.TravelPlanId == travelPlanId)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<Activity> UpdateActivityAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
                return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Accommodation operations
        public async Task<Accommodation> AddAccommodationAsync(Accommodation accommodation)
        {
            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();
            return accommodation;
        }

        public async Task<Accommodation?> GetAccommodationByIdAsync(int id)
        {
            return await _context.Accommodations.FindAsync(id);
        }

        public async Task<IEnumerable<Accommodation>> GetAccommodationsByPlanIdAsync(int travelPlanId)
        {
            return await _context.Accommodations
                .Where(a => a.TravelPlanId == travelPlanId)
                .OrderBy(a => a.CheckInDate)
                .ToListAsync();
        }

        public async Task<Accommodation> UpdateAccommodationAsync(Accommodation accommodation)
        {
            _context.Accommodations.Update(accommodation);
            await _context.SaveChangesAsync();
            return accommodation;
        }

        public async Task<bool> DeleteAccommodationAsync(int id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation == null)
                return false;

            _context.Accommodations.Remove(accommodation);
            await _context.SaveChangesAsync();
            return true;
        }

        // Transportation operations
        public async Task<Transportation> AddTransportationAsync(Transportation transportation)
        {
            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();
            return transportation;
        }

        public async Task<Transportation?> GetTransportationByIdAsync(int id)
        {
            return await _context.Transportations.FindAsync(id);
        }

        public async Task<IEnumerable<Transportation>> GetTransportationsByPlanIdAsync(int travelPlanId)
        {
            return await _context.Transportations
                .Where(t => t.TravelPlanId == travelPlanId)
                .OrderBy(t => t.DepartureTime)
                .ToListAsync();
        }

        public async Task<Transportation> UpdateTransportationAsync(Transportation transportation)
        {
            _context.Transportations.Update(transportation);
            await _context.SaveChangesAsync();
            return transportation;
        }

        public async Task<bool> DeleteTransportationAsync(int id)
        {
            var transportation = await _context.Transportations.FindAsync(id);
            if (transportation == null)
                return false;

            _context.Transportations.Remove(transportation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 