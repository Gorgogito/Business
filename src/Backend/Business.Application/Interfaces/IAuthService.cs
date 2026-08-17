namespace Business.Application.Interfaces;

using Business.Application.DTOs.Auth;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(int userId);
}
