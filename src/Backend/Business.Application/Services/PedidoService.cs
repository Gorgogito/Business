namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class PedidoService : IPedidoService
{
    private readonly IRepository<Pedido> _repo;
    private readonly IRepository<Cotizacion> _cotizacionRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IParametroService _parametros;

    public PedidoService(IRepository<Pedido> repo, IRepository<Cotizacion> cotizacionRepo, IUnitOfWork unitOfWork, ICorrelativoService correlativos, IParametroService parametros)
    {
        _repo = repo;
        _cotizacionRepo = cotizacionRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _parametros = parametros;
    }

    public async Task<IEnumerable<PedidoDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(p => p.Cliente).Include(p => p.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<PedidoDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(p => p.Cliente).Include(p => p.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(p => p.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<PedidoDto>> GetByClienteAsync(int clienteId)
    {
        var items = await _repo.Query().Include(p => p.Cliente).Include(p => p.Detalles).ThenInclude(d => d.Producto).Where(p => p.ClienteId == clienteId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<PedidoDto> CreateAsync(CreatePedidoDto dto, string userName)
    {
        var numero = await _correlativos.SiguienteAsync("PEDIDO", "PED");
        var pedido = new Pedido
        {
            Numero = numero, Fecha = DateTime.UtcNow, ClienteId = dto.ClienteId,
            CotizacionId = dto.CotizacionId, Estado = EstadoPedido.Pendiente, Observaciones = dto.Observaciones,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        decimal subTotal = 0;
        foreach (var d in dto.Detalles)
        {
            var sub = (d.Cantidad * d.PrecioUnitario) - d.Descuento;
            subTotal += sub;
            pedido.Detalles.Add(new PedidoDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento, SubTotal = sub, IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }
        var igvRate = await _parametros.GetIgvRateAsync();
        pedido.SubTotal = subTotal;
        pedido.Igv = subTotal * igvRate;
        pedido.Total = subTotal + pedido.Igv;

        await _repo.AddAsync(pedido);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(pedido);
    }

    public async Task<PedidoDto?> CrearDesdeCotizacionAsync(int cotizacionId, string userName)
    {
        var cot = await _cotizacionRepo.Query().Include(c => c.Detalles).FirstOrDefaultAsync(c => c.Id == cotizacionId);
        if (cot == null) return null;

        if (string.Equals(cot.Estado, EstadoCotizacion.Rechazada, StringComparison.OrdinalIgnoreCase))
            throw new BusinessRuleException("No se puede generar un pedido desde una cotización rechazada.");

        var yaConvertida = await _repo.Query().AnyAsync(p => p.CotizacionId == cotizacionId && p.IsActive);
        if (yaConvertida)
            throw new BusinessRuleException("La cotización ya fue convertida a un pedido.");

        // La cotización queda como aprobada; el cambio (tracked) se persiste junto con el pedido.
        cot.Estado = EstadoCotizacion.Aprobada;
        cot.UpdatedAt = DateTime.UtcNow;

        var dto = new CreatePedidoDto
        {
            ClienteId = cot.ClienteId,
            CotizacionId = cot.Id,
            Observaciones = cot.Observaciones,
            Detalles = cot.Detalles.Where(d => d.IsActive).Select(d => new CreatePedidoDetalleDto
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento
            }).ToList()
        };

        return await CreateAsync(dto, userName);
    }

    public async Task<PedidoDto?> UpdateEstadoAsync(int id, string estado)
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

    private static PedidoDto MapToDto(Pedido p) => new()
    {
        Id = p.Id, Numero = p.Numero, Fecha = p.Fecha, ClienteId = p.ClienteId,
        ClienteNombre = p.Cliente?.RazonSocial, CotizacionId = p.CotizacionId, Estado = p.Estado,
        SubTotal = p.SubTotal, Igv = p.Igv, Total = p.Total, Observaciones = p.Observaciones,
        Detalles = p.Detalles.Select(d => new PedidoDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento, SubTotal = d.SubTotal
        }).ToList()
    };
}
