namespace Business.Application.Interfaces;

using Business.Application.DTOs.Compras;

public interface IOrdenCompraService
{
    Task<IEnumerable<OrdenCompraDto>> GetAllAsync();
    Task<OrdenCompraDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrdenCompraDto>> GetByProveedorAsync(int proveedorId);
    Task<OrdenCompraDto> CreateAsync(CreateOrdenCompraDto dto, string userName);
    Task<OrdenCompraDto?> UpdateEstadoAsync(int id, string estado);
    Task<bool> DeleteAsync(int id);
}
