namespace Business.Application.Interfaces;

using Business.Application.DTOs.Configuration;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaDto>> GetAllAsync();
    Task<EmpresaDto?> GetByIdAsync(int id);
    Task<EmpresaDto> CreateAsync(CreateEmpresaDto dto);
    Task<EmpresaDto?> UpdateAsync(int id, CreateEmpresaDto dto);
    Task<bool> DeleteAsync(int id);

    /// <summary>Clona el plan de cuentas y conceptos de planilla base a una empresa que aún no los tiene.</summary>
    Task<bool> AprovisionarCatalogoAsync(int id);
}
