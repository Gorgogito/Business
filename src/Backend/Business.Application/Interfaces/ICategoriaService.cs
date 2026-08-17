namespace Business.Application.Interfaces;

using Business.Application.DTOs.Maestros;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> GetByIdAsync(int id);
    Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto);
    Task<CategoriaDto?> UpdateAsync(int id, CreateCategoriaDto dto);
    Task<bool> DeleteAsync(int id);
}
