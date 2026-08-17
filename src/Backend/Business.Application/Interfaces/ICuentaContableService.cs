namespace Business.Application.Interfaces;

using Business.Application.DTOs.Contabilidad;

public interface ICuentaContableService
{
    Task<IEnumerable<CuentaContableDto>> GetAllAsync();
    Task<CuentaContableDto?> GetByIdAsync(int id);
    Task<CuentaContableDto?> GetByCodigoAsync(string codigo);
    Task<CuentaContableDto> CreateAsync(CreateCuentaContableDto dto, string userName);
    Task<CuentaContableDto?> UpdateAsync(int id, CreateCuentaContableDto dto, string userName);
    Task<bool> DeleteAsync(int id);
}
