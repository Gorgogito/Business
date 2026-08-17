namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class GuiaRemisionService : IGuiaRemisionService
{
    private readonly IRepository<GuiaRemision> _repo;
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;

    public GuiaRemisionService(
        IRepository<GuiaRemision> repo,
        IRepository<Factura> facturaRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos)
    {
        _repo = repo;
        _facturaRepo = facturaRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
    }

    public async Task<IEnumerable<GuiaRemisionDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(g => g.Cliente).Include(g => g.Factura)
            .Include(g => g.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<GuiaRemisionDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(g => g.Cliente).Include(g => g.Factura)
            .Include(g => g.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(g => g.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<GuiaRemisionDto>> GetByFacturaAsync(int facturaId)
    {
        var items = await _repo.Query().Include(g => g.Cliente).Include(g => g.Factura)
            .Include(g => g.Detalles).ThenInclude(d => d.Producto).Where(g => g.FacturaId == facturaId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<GuiaRemisionDto?> CreateAsync(CreateGuiaRemisionDto dto, string userName)
    {
        Factura? factura = null;
        if (dto.FacturaId.HasValue)
        {
            factura = await _facturaRepo.Query().Include(f => f.Detalles)
                .FirstOrDefaultAsync(f => f.Id == dto.FacturaId.Value);
            if (factura == null) return null;
            if (string.Equals(factura.Estado, EstadoFactura.Anulada, StringComparison.OrdinalIgnoreCase))
                throw new BusinessRuleException("No se puede emitir una guía sobre una factura anulada.");
        }

        var numero = await _correlativos.SiguienteAsync("GUIA_REMISION", "T001");

        var guia = new GuiaRemision
        {
            Serie = "T001", Numero = numero, Fecha = DateTime.UtcNow,
            FechaTraslado = dto.FechaTraslado == default ? DateTime.UtcNow : dto.FechaTraslado,
            FacturaId = dto.FacturaId,
            ClienteId = factura?.ClienteId ?? dto.ClienteId,
            DireccionPartida = dto.DireccionPartida,
            DireccionLlegada = dto.DireccionLlegada,
            Transportista = dto.Transportista,
            TransportistaRuc = dto.TransportistaRuc,
            Placa = dto.Placa,
            Motivo = string.IsNullOrWhiteSpace(dto.Motivo) ? MotivoTraslado.Venta : dto.Motivo,
            Estado = EstadoGuiaRemision.Emitida,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        // Detalles explícitos; si vienen vacíos y hay factura, se arrastran sus ítems.
        var detalles = dto.Detalles.Count > 0
            ? dto.Detalles.Select(d => (d.ProductoId, d.Cantidad, d.Descripcion))
            : factura?.Detalles.Where(d => d.IsActive).Select(d => (d.ProductoId, d.Cantidad, (string?)null))
              ?? Enumerable.Empty<(int, decimal, string?)>();

        foreach (var (productoId, cantidad, descripcion) in detalles)
        {
            guia.Detalles.Add(new GuiaRemisionDetalle
            {
                ProductoId = productoId, Cantidad = cantidad, Descripcion = descripcion,
                IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }

        if (guia.Detalles.Count == 0)
            throw new BusinessRuleException("La guía de remisión debe tener al menos un ítem a trasladar.");

        await _repo.AddAsync(guia);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(guia);
    }

    private static GuiaRemisionDto MapToDto(GuiaRemision g) => new()
    {
        Id = g.Id, Serie = g.Serie, Numero = g.Numero, Fecha = g.Fecha, FechaTraslado = g.FechaTraslado,
        FacturaId = g.FacturaId, FacturaNumero = g.Factura == null ? null : $"{g.Factura.Serie}-{g.Factura.Numero}",
        ClienteId = g.ClienteId, ClienteNombre = g.Cliente?.RazonSocial,
        DireccionPartida = g.DireccionPartida, DireccionLlegada = g.DireccionLlegada,
        Transportista = g.Transportista, TransportistaRuc = g.TransportistaRuc, Placa = g.Placa,
        Motivo = g.Motivo, Estado = g.Estado,
        Detalles = g.Detalles.Select(d => new GuiaRemisionDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, Descripcion = d.Descripcion
        }).ToList()
    };
}
