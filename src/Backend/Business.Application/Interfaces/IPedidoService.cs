namespace Business.Application.Interfaces;

using Business.Application.DTOs.Ventas;

public interface IPedidoService
{
    Task<IEnumerable<PedidoDto>> GetAllAsync();
    Task<PedidoDto?> GetByIdAsync(int id);
    Task<IEnumerable<PedidoDto>> GetByClienteAsync(int clienteId);
    Task<PedidoDto> CreateAsync(CreatePedidoDto dto, string userName);
    /// <summary>Genera un pedido a partir de una cotización, arrastrando su detalle y enlazando el origen.</summary>
    Task<PedidoDto?> CrearDesdeCotizacionAsync(int cotizacionId, string userName);
    Task<PedidoDto?> UpdateEstadoAsync(int id, string estado);
    Task<bool> DeleteAsync(int id);
}
