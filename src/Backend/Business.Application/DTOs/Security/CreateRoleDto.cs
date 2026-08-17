namespace Business.Application.DTOs.Security;

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<int> PermissionIds { get; set; } = new();
    public List<int> MenuIds { get; set; } = new();
}
