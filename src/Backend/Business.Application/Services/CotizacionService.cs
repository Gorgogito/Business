namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class CotizacionService : ICotizacionService
{
    private readonly IRepository<Cotizacion> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IParametroService _parametros;

    public CotizacionService(IRepository<Cotizacion> repo, IUnitOfWork unitOfWork, ICorrelativoService correlativos, IParametroService parametros)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _parametros = parametros;
    }

    public async Task<IEnumerable<CotizacionDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(c => c.Cliente).Include(c => c.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<CotizacionDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(c => c.Cliente).Include(c => c.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(c => c.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<CotizacionDto>> GetByClienteAsync(int clienteId)
    {
        var items = await _repo.Query().Include(c => c.Cliente).Include(c => c.Detalles).ThenInclude(d => d.Producto).Where(c => c.ClienteId == clienteId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<CotizacionDto> CreateAsync(CreateCotizacionDto dto, string userName)
    {
        var numero = await _correlativos.SiguienteAsync("COTIZACION", "COT");
        var cotizacion = new Cotizacion
        {
            Numero = numero, Fecha = DateTime.UtcNow, FechaVencimiento = dto.FechaVencimiento,
            ClienteId = dto.ClienteId, Estado = EstadoCotizacion.Borrador, Observaciones = dto.Observaciones,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        decimal subTotal = 0;
        foreach (var d in dto.Detalles)
        {
            var subTotalDetalle = (d.Cantidad * d.PrecioUnitario) - d.Descuento;
            subTotal += subTotalDetalle;
            cotizacion.Detalles.Add(new CotizacionDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento, SubTotal = subTotalDetalle, IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }
        var igvRate = await _parametros.GetIgvRateAsync();
        cotizacion.SubTotal = subTotal;
        cotizacion.Igv = subTotal * igvRate;
        cotizacion.Total = subTotal + cotizacion.Igv;

        await _repo.AddAsync(cotizacion);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(cotizacion);
    }

    public async Task<CotizacionDto?> UpdateEstadoAsync(int id, string estado)
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

    private static CotizacionDto MapToDto(Cotizacion c) => new()
    {
        Id = c.Id, Numero = c.Numero, Fecha = c.Fecha, FechaVencimiento = c.FechaVencimiento,
        ClienteId = c.ClienteId, ClienteNombre = c.Cliente?.RazonSocial, Estado = c.Estado,
        SubTotal = c.SubTotal, Igv = c.Igv, Total = c.Total, Observaciones = c.Observaciones,
        Detalles = c.Detalles.Select(d => new CotizacionDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento, SubTotal = d.SubTotal
        }).ToList()
    };
}
