namespace Business.Application.DTOs.Auth;

public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public List<MenuDto> Children { get; set; } = new();
}
