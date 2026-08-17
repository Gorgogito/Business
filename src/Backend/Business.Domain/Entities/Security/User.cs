namespace Business.Domain.Entities.Security;

using Business.Domain.Common;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public int? EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
