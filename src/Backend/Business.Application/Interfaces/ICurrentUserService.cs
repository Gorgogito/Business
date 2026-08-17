namespace Business.Application.Interfaces;

/// <summary>Expone el usuario autenticado de la petición actual (para auditoría, etc.).</summary>
public interface ICurrentUserService
{
    string? UserName { get; }
    /// <summary>Empresa del usuario autenticado (multiempresa). Null si no aplica.</summary>
    int? EmpresaId { get; }
}
