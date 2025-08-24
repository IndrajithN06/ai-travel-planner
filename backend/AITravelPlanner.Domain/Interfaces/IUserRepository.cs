using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface IUserRepository
    {
        // CRUD operations
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

        // Authentication specific
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> UpdatePasswordAsync(int userId, string passwordHash);

        // Validation
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
    }
} 