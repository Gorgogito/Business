namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class NotaVentaService : INotaVentaService
{
    private readonly IRepository<NotaVenta> _repo;
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IInventarioService _inventario;
    private readonly IParametroService _parametros;
    private readonly ICuentaPorCobrarService _cuentasPorCobrar;

    public NotaVentaService(
        IRepository<NotaVenta> repo,
        IRepository<Factura> facturaRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos,
        IInventarioService inventario,
        IParametroService parametros,
        ICuentaPorCobrarService cuentasPorCobrar)
    {
        _repo = repo;
        _facturaRepo = facturaRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _inventario = inventario;
        _parametros = parametros;
        _cuentasPorCobrar = cuentasPorCobrar;
    }

    public async Task<IEnumerable<NotaVentaDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(n => n.Cliente).Include(n => n.Factura)
            .Include(n => n.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<NotaVentaDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(n => n.Cliente).Include(n => n.Factura)
            .Include(n => n.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(n => n.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<NotaVentaDto>> GetByFacturaAsync(int facturaId)
    {
        var items = await _repo.Query().Include(n => n.Cliente).Include(n => n.Factura)
            .Include(n => n.Detalles).ThenInclude(d => d.Producto).Where(n => n.FacturaId == facturaId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<NotaVentaDto?> CreateAsync(CreateNotaVentaDto dto, string userName)
    {
        var factura = await _facturaRepo.Query().FirstOrDefaultAsync(f => f.Id == dto.FacturaId);
        if (factura == null) return null;

        if (string.Equals(factura.Estado, EstadoFactura.Anulada, StringComparison.OrdinalIgnoreCase))
            throw new BusinessRuleException("No se puede emitir una nota sobre una factura anulada.");
        if (dto.Detalles.Count == 0)
            throw new BusinessRuleException("La nota debe tener al menos un detalle.");

        var esCredito = string.Equals(dto.Tipo, TipoNota.Credito, StringComparison.OrdinalIgnoreCase);
        var tipoCorrelativo = esCredito ? "NOTA_CREDITO" : "NOTA_DEBITO";
        var serie = esCredito ? "FC01" : "FD01";
        var numero = await _correlativos.SiguienteAsync(tipoCorrelativo, serie);

        var nota = new NotaVenta
        {
            Serie = serie, Numero = numero, Tipo = esCredito ? TipoNota.Credito : TipoNota.Debito,
            Fecha = DateTime.UtcNow, ClienteId = factura.ClienteId, Factura = factura,
            Motivo = dto.Motivo, Estado = EstadoNotaVenta.Emitida,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        decimal subTotal = 0;
        foreach (var d in dto.Detalles)
        {
            var sub = (d.Cantidad * d.PrecioUnitario) - d.Descuento;
            subTotal += sub;
            nota.Detalles.Add(new NotaVentaDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento, SubTotal = sub, IsActive = true, CreatedAt = DateTime.UtcNow
            });

            // Solo la nota de crédito reingresa mercadería (devolución).
            if (esCredito && d.Cantidad > 0)
            {
                await _inventario.RegistrarMovimientoAsync(
                    InventarioService.Entrada, d.ProductoId, factura.AlmacenId, d.Cantidad, d.PrecioUnitario,
                    $"{serie}-{numero}", $"Devolución nota de crédito s/ {factura.Serie}-{factura.Numero}", userName, validarDisponibilidad: false);
            }
        }

        var igvRate = await _parametros.GetIgvRateAsync();
        nota.SubTotal = subTotal;
        nota.Igv = subTotal * igvRate;
        nota.Total = subTotal + nota.Igv;

        // Ajusta la cuenta por cobrar de la factura según el tipo de nota.
        if (esCredito)
            await _cuentasPorCobrar.AplicarNotaCreditoAsync(factura.Id, nota.Total);
        else
            await _cuentasPorCobrar.AplicarNotaDebitoAsync(factura.Id, nota.Total);

        await _repo.AddAsync(nota);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(nota);
    }

    private static NotaVentaDto MapToDto(NotaVenta n) => new()
    {
        Id = n.Id, Serie = n.Serie, Numero = n.Numero, Tipo = n.Tipo, Fecha = n.Fecha,
        FacturaId = n.FacturaId, FacturaNumero = n.Factura == null ? null : $"{n.Factura.Serie}-{n.Factura.Numero}",
        ClienteId = n.ClienteId, ClienteNombre = n.Cliente?.RazonSocial, Motivo = n.Motivo,
        SubTotal = n.SubTotal, Igv = n.Igv, Total = n.Total, Estado = n.Estado,
        Detalles = n.Detalles.Select(d => new NotaVentaDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento, SubTotal = d.SubTotal
        }).ToList()
    };
}
