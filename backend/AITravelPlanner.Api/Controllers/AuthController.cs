using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Interfaces;

namespace AITravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                
                if (response.Success)
                {
                    return CreatedAtAction(nameof(GetCurrentUser), response);
                }
                
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                
                if (response.Success)
                {
                    return Ok(response);
                }
                
                return Unauthorized(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                });
            }
        }

        // POST: api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request);
                
                if (response.Success)
                {
                    return Ok(response);
                }
                
                return Unauthorized(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = $"Token refresh failed: {ex.Message}"
                });
            }
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var success = await _authService.LogoutAsync(request.RefreshToken);
                
                if (success)
                {
                    return Ok(new { message = "Logout successful" });
                }
                
                return BadRequest(new { message = "Logout failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Logout failed: {ex.Message}" });
            }
        }

        // GET: api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var user = await _authService.GetUserByIdAsync(userId.Value);
                
                if (user == null)
                {
                    return NotFound();
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to get user: {ex.Message}" });
            }
        }

        // PUT: api/auth/me
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserDto userDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var success = await _authService.UpdateUserAsync(userId.Value, userDto);
                
                if (success)
                {
                    return Ok(new { message = "User updated successfully" });
                }
                
                return BadRequest(new { message = "Failed to update user" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to update user: {ex.Message}" });
            }
        }

        // POST: api/auth/change-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var success = await _authService.ChangePasswordAsync(userId.Value, request);
                
                if (success)
                {
                    return Ok(new { message = "Password changed successfully" });
                }
                
                return BadRequest(new { message = "Failed to change password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to change password: {ex.Message}" });
            }
        }

        // POST: api/auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var success = await _authService.ForgotPasswordAsync(request);
                
                if (success)
                {
                    return Ok(new { message = "Password reset email sent" });
                }
                
                return BadRequest(new { message = "Failed to send password reset email" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to send password reset email: {ex.Message}" });
            }
        }

        // POST: api/auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var success = await _authService.ResetPasswordAsync(request);
                
                if (success)
                {
                    return Ok(new { message = "Password reset successfully" });
                }
                
                return BadRequest(new { message = "Failed to reset password" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to reset password: {ex.Message}" });
            }
        }

        // DELETE: api/auth/me
        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized();
                }

                var success = await _authService.DeleteUserAsync(userId.Value);
                
                if (success)
                {
                    return Ok(new { message = "User deleted successfully" });
                }
                
                return BadRequest(new { message = "Failed to delete user" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to delete user: {ex.Message}" });
            }
        }

        // Helper method to get current user ID from JWT token
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            
            return null;
        }
    }
} 