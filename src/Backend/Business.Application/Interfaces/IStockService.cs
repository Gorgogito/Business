namespace Business.Application.Interfaces;

using Business.Application.DTOs.Inventario;

public interface IStockService
{
    Task<IEnumerable<StockDto>> GetAllAsync();
    Task<IEnumerable<StockDto>> GetByProductoAsync(int productoId);
    Task<IEnumerable<StockDto>> GetByAlmacenAsync(int almacenId);
    Task<StockDto?> GetByProductoAlmacenAsync(int productoId, int almacenId);
}
