namespace Business.Application.Interfaces;

using Business.Application.DTOs.Configuration;

public interface ISucursalService
{
    Task<IEnumerable<SucursalDto>> GetAllAsync();
    Task<SucursalDto?> GetByIdAsync(int id);
    Task<IEnumerable<SucursalDto>> GetByEmpresaAsync(int empresaId);
    Task<SucursalDto> CreateAsync(CreateSucursalDto dto);
    Task<SucursalDto?> UpdateAsync(int id, CreateSucursalDto dto);
    Task<bool> DeleteAsync(int id);
}
