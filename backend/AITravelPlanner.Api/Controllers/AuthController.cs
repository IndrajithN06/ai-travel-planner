using AITravelPlanner.Domain.DTOs;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request) =>
        Ok(await _authService.RegisterAsync(request));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request) =>
        Ok(await _authService.LoginAsync(request));

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request) =>
        Ok(await _authService.RefreshTokenAsync(request));

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request) =>
        Ok(await _authService.LogoutAsync(request.RefreshToken));

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();
        var user = await _authService.GetUserByIdAsync(userId.Value);
        return user != null ? Ok(user) : NotFound();
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : null;
    }
}
