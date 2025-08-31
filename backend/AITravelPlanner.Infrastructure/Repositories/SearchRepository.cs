using AITravelPlanner.Infrastructure.Data;
using AITravelPlanner.Domain.Interfaces;
using AITravelPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AITravelPlanner.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Search Flights by keyword (FlightNumber, FromLocation, ToLocation)
        public async Task<IEnumerable<Flight>> GetFlightsAsync(string searchTerm)
        {
            return await _context.Flights
                .Where(f => f.FlightNumber.Contains(searchTerm) ||
                            f.FromLocation.Contains(searchTerm) ||
                            f.ToLocation.Contains(searchTerm))
                .ToListAsync();
        }

        // Search Buses by keyword (BusName, FromLocation, ToLocation)
        public async Task<IEnumerable<Bus>> GetBusesAsync(string searchTerm)
        {
            return await _context.Buses
                .Where(b => b.BusName.Contains(searchTerm) ||
                            b.FromLocation.Contains(searchTerm) ||
                            b.ToLocation.Contains(searchTerm))
                .ToListAsync();
        }

        // Search Trains by keyword (TrainName, FromLocation, ToLocation)
        public async Task<IEnumerable<Train>> GetTrainsAsync(string searchTerm)
        {
            return await _context.Trains
                .Where(t => t.TrainName.Contains(searchTerm) ||
                            t.FromLocation.Contains(searchTerm) ||
                            t.ToLocation.Contains(searchTerm))
                .ToListAsync();
        }

        // Search Hotels by keyword (Name, City, Address)
        public async Task<IEnumerable<Hotel>> GetHotelsAsync(string searchTerm)
        {
            return await _context.Hotels
                .Where(h => h.Name.Contains(searchTerm) ||
                            h.City.Contains(searchTerm) ||
                            h.Address.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
