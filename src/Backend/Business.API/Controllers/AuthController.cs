namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Auth;
using Business.Application.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(ApiResponse<LoginResponseDto>.Fail("Usuario o contraseña incorrectos"));
        return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Login exitoso"));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Refresh([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        if (result == null)
            return Unauthorized(ApiResponse<LoginResponseDto>.Fail("Token de refresco inválido o expirado"));
        return Ok(ApiResponse<LoginResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<bool>>> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
            await _authService.LogoutAsync(userId);
        return Ok(ApiResponse<bool>.Ok(true, "Sesión cerrada exitosamente"));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<ApiResponse<object>> Me()
    {
        var user = new
        {
            Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Username = User.FindFirst(ClaimTypes.Name)?.Value,
            Email = User.FindFirst(ClaimTypes.Email)?.Value,
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        };
        return Ok(ApiResponse<object>.Ok(user));
    }
}
