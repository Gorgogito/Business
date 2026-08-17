namespace Business.Application.DTOs.Security;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<int> PermissionIds { get; set; } = new();
    public List<int> MenuIds { get; set; } = new();
}
