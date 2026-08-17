namespace Business.API.Services;

using Business.Application.Interfaces;

/// <summary>Obtiene el usuario autenticado desde el HttpContext de la petición actual.</summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public int? EmpresaId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("empresaId")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}

