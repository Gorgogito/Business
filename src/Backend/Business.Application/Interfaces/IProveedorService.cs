namespace Business.Application.Interfaces;

using Business.Application.DTOs.Maestros;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorDto>> GetAllAsync();
    Task<ProveedorDto?> GetByIdAsync(int id);
    Task<ProveedorDto> CreateAsync(CreateProveedorDto dto);
    Task<ProveedorDto?> UpdateAsync(int id, CreateProveedorDto dto);
    Task<bool> DeleteAsync(int id);
}
