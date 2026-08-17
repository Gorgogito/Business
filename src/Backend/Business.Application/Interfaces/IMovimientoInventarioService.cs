namespace Business.Application.Interfaces;

using Business.Application.DTOs.Inventario;

public interface IMovimientoInventarioService
{
    Task<IEnumerable<MovimientoInventarioDto>> GetAllAsync();
    Task<MovimientoInventarioDto?> GetByIdAsync(int id);
    Task<IEnumerable<MovimientoInventarioDto>> GetByProductoAsync(int productoId);
    Task<MovimientoInventarioDto> CreateAsync(CreateMovimientoDto dto, string userName);
}
