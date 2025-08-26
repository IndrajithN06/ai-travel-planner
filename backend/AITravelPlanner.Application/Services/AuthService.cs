using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly Dictionary<string, string> _refreshTokens = new(); // In production, use Redis or database

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Check if email already exists
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email already registered"
                    };
                }

                // Create new user
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    Country = request.Country,
                    City = request.City,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    CreatedDate = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Generate tokens
                var token = _jwtService.GenerateJwtToken(createdUser.Id, createdUser.Email, createdUser.FullName);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token (in production, store in database)
                _refreshTokens[refreshToken] = createdUser.Id.ToString();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDto(createdUser)
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Get user by email
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Verify password
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Check if user is active
                if (!user.IsActive)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Account is deactivated"
                    };
                }

                // Update last login
                await _userRepository.UpdateLastLoginAsync(user.Id);

                // Generate tokens
                var token = _jwtService.GenerateJwtToken(user.Id, user.Email, user.FullName);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Store refresh token
                _refreshTokens[refreshToken] = user.Id.ToString();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                // Check if refresh token exists
                if (!_refreshTokens.TryGetValue(request.RefreshToken, out string? userIdStr))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                if (!int.TryParse(userIdStr, out int userId))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }

                // Get user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Generate new tokens
                var newToken = _jwtService.GenerateJwtToken(user.Id, user.Email, user.FullName);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                // Remove old refresh token and add new one
                _refreshTokens.Remove(request.RefreshToken);
                _refreshTokens[newRefreshToken] = user.Id.ToString();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Token refresh failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            try
            {
                _refreshTokens.Remove(refreshToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return false;

                // Verify current password
                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                    return false;

                // Update password
                var newPasswordHash = HashPassword(request.NewPassword);
                return await _userRepository.UpdatePasswordAsync(userId, newPasswordHash);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                    return false;

                // In production, send email with reset link
                // For now, just return true
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                // In production, validate the reset token
                // For now, just return true
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                return user != null ? MapToUserDto(user) : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                return user != null ? MapToUserDto(user) : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, UserDto userDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return false;

                // Update user properties
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PhoneNumber = userDto.PhoneNumber;
                user.Country = userDto.Country;
                user.City = userDto.City;
                user.DateOfBirth = userDto.DateOfBirth;
                user.Gender = userDto.Gender;
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                return await _userRepository.DeleteAsync(userId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return   _jwtService.ValidateToken(token);
        }

        public async Task<int?> GetUserIdFromTokenAsync(string token)
        {
            return  _jwtService.GetUserIdFromToken(token);
        }

        // Private helper methods
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
                City = user.City,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                IsEmailVerified = user.IsEmailVerified,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate
            };
        }
    }
} 
