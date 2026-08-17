namespace Business.Application.Interfaces;

using Business.Application.DTOs.Inventario;

public interface IAlmacenService
{
    Task<IEnumerable<AlmacenDto>> GetAllAsync();
    Task<AlmacenDto?> GetByIdAsync(int id);
    Task<AlmacenDto> CreateAsync(CreateAlmacenDto dto);
    Task<AlmacenDto?> UpdateAsync(int id, CreateAlmacenDto dto);
    Task<bool> DeleteAsync(int id);
}
