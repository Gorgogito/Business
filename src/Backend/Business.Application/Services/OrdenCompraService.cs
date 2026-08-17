namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Compras;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Compras;
using Business.Domain.Interfaces;

public class OrdenCompraService : IOrdenCompraService
{
    private readonly IRepository<OrdenCompra> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IParametroService _parametros;

    public OrdenCompraService(IRepository<OrdenCompra> repo, IUnitOfWork unitOfWork, ICorrelativoService correlativos, IParametroService parametros)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _parametros = parametros;
    }

    public async Task<IEnumerable<OrdenCompraDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(o => o.Proveedor).Include(o => o.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<OrdenCompraDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(o => o.Proveedor).Include(o => o.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(o => o.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<OrdenCompraDto>> GetByProveedorAsync(int proveedorId)
    {
        var items = await _repo.Query().Include(o => o.Proveedor).Include(o => o.Detalles).ThenInclude(d => d.Producto).Where(o => o.ProveedorId == proveedorId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<OrdenCompraDto> CreateAsync(CreateOrdenCompraDto dto, string userName)
    {
        var numero = await _correlativos.SiguienteAsync("ORDEN_COMPRA", "OC");
        var orden = new OrdenCompra
        {
            Numero = numero, Fecha = DateTime.UtcNow, FechaEntrega = dto.FechaEntrega,
            ProveedorId = dto.ProveedorId, Estado = EstadoOrdenCompra.Pendiente, Observaciones = dto.Observaciones,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        decimal subTotal = 0;
        foreach (var d in dto.Detalles)
        {
            var sub = d.Cantidad * d.PrecioUnitario;
            subTotal += sub;
            orden.Detalles.Add(new OrdenCompraDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario, SubTotal = sub, IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }
        var igvRate = await _parametros.GetIgvRateAsync();
        orden.SubTotal = subTotal;
        orden.Igv = subTotal * igvRate;
        orden.Total = subTotal + orden.Igv;

        await _repo.AddAsync(orden);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(orden);
    }

    public async Task<OrdenCompraDto?> UpdateEstadoAsync(int id, string estado)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Estado = estado; entity.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static OrdenCompraDto MapToDto(OrdenCompra o) => new()
    {
        Id = o.Id, Numero = o.Numero, Fecha = o.Fecha, FechaEntrega = o.FechaEntrega,
        ProveedorId = o.ProveedorId, ProveedorNombre = o.Proveedor?.RazonSocial, Estado = o.Estado,
        SubTotal = o.SubTotal, Igv = o.Igv, Total = o.Total, Observaciones = o.Observaciones,
        Detalles = o.Detalles.Select(d => new OrdenCompraDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, CantidadRecibida = d.CantidadRecibida, PrecioUnitario = d.PrecioUnitario, SubTotal = d.SubTotal
        }).ToList()
    };
}
