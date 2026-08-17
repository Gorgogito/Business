namespace Business.Domain.Entities.Security;

using Business.Domain.Common;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}
