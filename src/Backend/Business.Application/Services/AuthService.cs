namespace Business.Application.Services;

using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Auth;
using Business.Application.Interfaces;
using Business.Domain.Entities.Security;
using Business.Domain.Interfaces;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<Menu> _menuRepo;
    private readonly IRepository<RoleMenu> _roleMenuRepo;
    private readonly IRepository<RolePermission> _rolePermissionRepo;
    private readonly IRepository<Permission> _permissionRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public AuthService(
        IRepository<User> userRepo,
        IRepository<Menu> menuRepo,
        IRepository<RoleMenu> roleMenuRepo,
        IRepository<RolePermission> rolePermissionRepo,
        IRepository<Permission> permissionRepo,
        IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _userRepo = userRepo;
        _menuRepo = menuRepo;
        _roleMenuRepo = roleMenuRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _permissionRepo = permissionRepo;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepo.Query()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

        if (user == null || !BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return null;

        var permissions = await GetUserPermissionsAsync(user.RoleId);
        var token = _jwtService.GenerateToken(user, permissions);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.SaveChangesAsync();

        var menus = await GetUserMenusAsync(user.RoleId);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(1),
            Username = user.Username,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            RoleName = user.Role?.Name,
            Menus = menus,
            Permissions = permissions
        };
    }

    public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepo.Query()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user == null) return null;

        var permissions = await GetUserPermissionsAsync(user.RoleId);
        var token = _jwtService.GenerateToken(user, permissions);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.SaveChangesAsync();

        var menus = await GetUserMenusAsync(user.RoleId);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddHours(1),
            Username = user.Username,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            RoleName = user.Role?.Name,
            Menus = menus,
            Permissions = permissions
        };
    }

    public async Task LogoutAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private async Task<List<string>> GetUserPermissionsAsync(int? roleId)
    {
        if (!roleId.HasValue) return new();

        var permissionIds = (await _rolePermissionRepo.FindAsync(rp => rp.RoleId == roleId.Value))
            .Select(rp => rp.PermissionId).ToHashSet();

        return (await _permissionRepo.FindAsync(p => p.IsActive && permissionIds.Contains(p.Id)))
            .Select(p => p.Code).ToList();
    }

    private async Task<List<MenuDto>> GetUserMenusAsync(int? roleId)
    {
        if (!roleId.HasValue) return new();

        var roleMenuIds = (await _roleMenuRepo.FindAsync(rm => rm.RoleId == roleId.Value))
            .Select(rm => rm.MenuId).ToHashSet();

        var allMenus = (await _menuRepo.FindAsync(m => m.IsActive && roleMenuIds.Contains(m.Id)))
            .OrderBy(m => m.Order).ToList();

        return BuildMenuTree(allMenus, null);
    }

    private static List<MenuDto> BuildMenuTree(List<Menu> menus, int? parentId)
    {
        return menus
            .Where(m => m.ParentId == parentId)
            .Select(m => new MenuDto
            {
                Id = m.Id,
                Name = m.Name,
                Icon = m.Icon,
                Route = m.Route,
                Order = m.Order,
                ParentId = m.ParentId,
                Children = BuildMenuTree(menus, m.Id)
            })
            .ToList();
    }
}
