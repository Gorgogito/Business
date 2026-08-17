namespace Business.Application.Interfaces;

using Business.Application.DTOs.Produccion;

public interface IRecetaService
{
    Task<IEnumerable<RecetaDto>> GetAllAsync();
    Task<RecetaDto?> GetByIdAsync(int id);
    Task<RecetaDto?> GetByProductoAsync(int productoId);
    Task<RecetaDto> CreateAsync(CreateRecetaDto dto, string userName);
    Task<RecetaDto?> UpdateAsync(int id, CreateRecetaDto dto, string userName);
    Task<bool> DeleteAsync(int id);
}
