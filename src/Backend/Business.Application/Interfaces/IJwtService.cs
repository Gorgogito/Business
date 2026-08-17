namespace Business.Application.Interfaces;

using Business.Domain.Entities.Security;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    int? GetUserIdFromToken(string token);
}
