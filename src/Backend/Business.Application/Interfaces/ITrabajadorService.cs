namespace Business.Application.Interfaces;

using Business.Application.DTOs.Rrhh;

public interface ITrabajadorService
{
    Task<IEnumerable<TrabajadorDto>> GetAllAsync();
    Task<IEnumerable<TrabajadorDto>> GetActivosAsync();
    Task<TrabajadorDto?> GetByIdAsync(int id);
    Task<TrabajadorDto> CreateAsync(CreateTrabajadorDto dto, string userName);
    Task<TrabajadorDto?> UpdateAsync(int id, CreateTrabajadorDto dto, string userName);
    Task<TrabajadorDto?> CesarAsync(int id, DateTime fechaCese, string userName);
    Task<bool> DeleteAsync(int id);
}
