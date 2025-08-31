using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface ISearchRepository
    {
        Task<IEnumerable<Flight>> GetFlightsAsync(string searchTerm);
        Task<IEnumerable<Bus>> GetBusesAsync(string searchTerm);
        Task<IEnumerable<Train>> GetTrainsAsync(string searchTerm);
        Task<IEnumerable<Hotel>> GetHotelsAsync(string searchTerm);
    }
}
