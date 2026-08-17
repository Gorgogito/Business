namespace Business.Application.Services;

using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Security;
using Business.Application.Interfaces;
using Business.Domain.Entities.Security;
using Business.Domain.Interfaces;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IRepository<User> userRepo, IUnitOfWork unitOfWork)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepo.Query().Include(u => u.Role).ToListAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepo.Query().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            RoleId = dto.RoleId,
            EmpresaId = dto.EmpresaId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepo.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return null;

        user.Email = dto.Email;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.RoleId = dto.RoleId;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepo.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return false;
        if (!BCrypt.Verify(currentPassword, user.PasswordHash)) return false;

        user.PasswordHash = BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        RoleId = user.RoleId,
        RoleName = user.Role?.Name,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
