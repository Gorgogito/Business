namespace Business.Application.Interfaces;

using Business.Application.DTOs.Maestros;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(int categoriaId);
    Task<ProductoDto> CreateAsync(CreateProductoDto dto);
    Task<ProductoDto?> UpdateAsync(int id, CreateProductoDto dto);
    Task<bool> DeleteAsync(int id);
}
