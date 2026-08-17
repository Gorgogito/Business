namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Security;
using Business.Application.Interfaces;
using Business.Domain.Entities.Security;
using Business.Domain.Interfaces;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _roleRepo;
    private readonly IRepository<RolePermission> _rolePermRepo;
    private readonly IRepository<RoleMenu> _roleMenuRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(
        IRepository<Role> roleRepo,
        IRepository<RolePermission> rolePermRepo,
        IRepository<RoleMenu> roleMenuRepo,
        IUnitOfWork unitOfWork)
    {
        _roleRepo = roleRepo;
        _rolePermRepo = rolePermRepo;
        _roleMenuRepo = roleMenuRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _roleRepo.Query()
            .Include(r => r.RolePermissions)
            .Include(r => r.RoleMenus)
            .ToListAsync();
        return roles.Select(MapToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await _roleRepo.Query()
            .Include(r => r.RolePermissions)
            .Include(r => r.RoleMenus)
            .FirstOrDefaultAsync(r => r.Id == id);
        return role != null ? MapToDto(role) : null;
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _roleRepo.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        foreach (var permId in dto.PermissionIds)
            await _rolePermRepo.AddAsync(new RolePermission { RoleId = role.Id, PermissionId = permId });

        foreach (var menuId in dto.MenuIds)
            await _roleMenuRepo.AddAsync(new RoleMenu { RoleId = role.Id, MenuId = menuId });

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(role);
    }

    public async Task<RoleDto?> UpdateAsync(int id, CreateRoleDto dto)
    {
        var role = await _roleRepo.Query()
            .Include(r => r.RolePermissions)
            .Include(r => r.RoleMenus)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return null;

        role.Name = dto.Name;
        role.Description = dto.Description;
        role.UpdatedAt = DateTime.UtcNow;

        foreach (var rp in role.RolePermissions.ToList())
            await _rolePermRepo.DeleteAsync(rp);
        foreach (var rm in role.RoleMenus.ToList())
            await _roleMenuRepo.DeleteAsync(rm);

        foreach (var permId in dto.PermissionIds)
            await _rolePermRepo.AddAsync(new RolePermission { RoleId = role.Id, PermissionId = permId });
        foreach (var menuId in dto.MenuIds)
            await _roleMenuRepo.AddAsync(new RoleMenu { RoleId = role.Id, MenuId = menuId });

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(role);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _roleRepo.GetByIdAsync(id);
        if (role == null) return false;
        role.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static RoleDto MapToDto(Role role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Description = role.Description,
        IsActive = role.IsActive,
        PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList(),
        MenuIds = role.RoleMenus.Select(rm => rm.MenuId).ToList()
    };
}
