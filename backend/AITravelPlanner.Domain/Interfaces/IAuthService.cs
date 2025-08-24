using System;
using System.Threading.Tasks;
using AITravelPlanner.Domain.DTOs;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface IAuthService
    {
        // Authentication
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string refreshToken);

        // Password management
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);

        // User management
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int userId, UserDto userDto);
        Task<bool> DeleteUserAsync(int userId);

        // Token validation
        Task<bool> ValidateTokenAsync(string token);
        Task<int?> GetUserIdFromTokenAsync(string token);
    }
} 