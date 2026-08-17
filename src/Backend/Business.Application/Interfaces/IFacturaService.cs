namespace Business.Application.Interfaces;

using Business.Application.DTOs.Ventas;

public interface IFacturaService
{
    Task<IEnumerable<FacturaDto>> GetAllAsync();
    Task<FacturaDto?> GetByIdAsync(int id);
    Task<IEnumerable<FacturaDto>> GetByClienteAsync(int clienteId);
    Task<FacturaDto> CreateAsync(CreateFacturaDto dto, string userName);
    /// <summary>Genera una factura/boleta a partir de un pedido, arrastrando su detalle, enlazando el origen y descontando stock.</summary>
    Task<FacturaDto?> CrearDesdePedidoAsync(int pedidoId, string tipoDocumento, int almacenId, string userName);
    Task<FacturaDto?> UpdateEstadoAsync(int id, string estado);
}
