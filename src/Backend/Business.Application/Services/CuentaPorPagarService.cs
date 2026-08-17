namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Finanzas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Compras;
using Business.Domain.Entities.Finanzas;
using Business.Domain.Interfaces;

public class CuentaPorPagarService : ICuentaPorPagarService
{
    private readonly IRepository<CuentaPorPagar> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public CuentaPorPagarService(IRepository<CuentaPorPagar> repo, IUnitOfWork unitOfWork, IAsientoContableService asientos, IConfiguracionContableService configContable)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<CuentaPorPagarDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(c => c.Proveedor).Include(c => c.RecepcionCompra).Include(c => c.Pagos).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<CuentaPorPagarDto>> GetPendientesAsync()
    {
        var items = await _repo.Query().Include(c => c.Proveedor).Include(c => c.RecepcionCompra).Include(c => c.Pagos)
            .Where(c => c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorPagar.Anulada).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<CuentaPorPagarDto>> GetByProveedorAsync(int proveedorId)
    {
        var items = await _repo.Query().Include(c => c.Proveedor).Include(c => c.RecepcionCompra).Include(c => c.Pagos)
            .Where(c => c.ProveedorId == proveedorId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<CuentaPorPagarDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(c => c.Proveedor).Include(c => c.RecepcionCompra).Include(c => c.Pagos)
            .FirstOrDefaultAsync(c => c.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task GenerarDesdeRecepcionAsync(RecepcionCompra recepcion, int proveedorId, decimal montoTotal, int diasCredito, string? userName, CancellationToken ct = default)
    {
        var cuenta = new CuentaPorPagar
        {
            ProveedorId = proveedorId,
            RecepcionCompra = recepcion, // relación por navegación: EF resuelve el FK al guardar
            FechaEmision = recepcion.Fecha,
            FechaVencimiento = recepcion.Fecha.AddDays(diasCredito < 0 ? 0 : diasCredito),
            MontoTotal = montoTotal,
            SaldoPendiente = montoTotal,
            Estado = EstadoCuentaPorPagar.Pendiente,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userName
        };
        await _repo.AddAsync(cuenta);
    }

    public async Task<PagoDto?> RegistrarPagoAsync(int cuentaPorPagarId, CreatePagoDto dto, string userName, CancellationToken ct = default)
    {
        var cuenta = await _repo.Query().FirstOrDefaultAsync(c => c.Id == cuentaPorPagarId, ct);
        if (cuenta == null) return null;

        if (cuenta.Estado == EstadoCuentaPorPagar.Anulada)
            throw new BusinessRuleException("La cuenta por pagar está anulada.");
        if (cuenta.SaldoPendiente <= 0)
            throw new BusinessRuleException("La cuenta por pagar ya está saldada.");
        if (dto.Monto <= 0)
            throw new BusinessRuleException("El monto del pago debe ser mayor a cero.");
        if (dto.Monto > cuenta.SaldoPendiente)
            throw new BusinessRuleException($"El pago ({dto.Monto}) excede el saldo pendiente ({cuenta.SaldoPendiente}).");

        var pago = new Pago
        {
            CuentaPorPagarId = cuenta.Id,
            Fecha = DateTime.UtcNow,
            Monto = dto.Monto,
            MedioPago = string.IsNullOrWhiteSpace(dto.MedioPago) ? "EFECTIVO" : dto.MedioPago,
            Referencia = dto.Referencia,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userName
        };
        cuenta.Pagos.Add(pago);

        cuenta.SaldoPendiente -= dto.Monto;
        cuenta.Estado = cuenta.SaldoPendiente <= 0 ? EstadoCuentaPorPagar.Pagada : EstadoCuentaPorPagar.Parcial;
        cuenta.UpdatedAt = DateTime.UtcNow;

        // Asiento contable de pago (cuentas por pagar / caja-bancos).
        var cuentas = await _configContable.ObtenerMapaAsync(ct);
        var cuentaCaja = string.Equals(pago.MedioPago, "EFECTIVO", StringComparison.OrdinalIgnoreCase)
            ? cuentas[ConceptosContables.TesoreriaEfectivo] : cuentas[ConceptosContables.TesoreriaBancos];
        await _asientos.GenerarAsientoAsync(
            pago.Fecha, $"Pago CxP #{cuenta.Id}", TipoAsiento.Pago, $"CxP-{cuenta.Id}",
            new[] { (cuentas[ConceptosContables.CompraProveedor], dto.Monto, 0m), (cuentaCaja, 0m, dto.Monto) }, userName, ct);

        await _unitOfWork.SaveChangesAsync(ct);
        return MapToPagoDto(pago);
    }

    private static CuentaPorPagarDto MapToDto(CuentaPorPagar c) => new()
    {
        Id = c.Id, ProveedorId = c.ProveedorId, ProveedorNombre = c.Proveedor?.RazonSocial,
        RecepcionCompraId = c.RecepcionCompraId, RecepcionNumero = c.RecepcionCompra?.Numero,
        FechaEmision = c.FechaEmision, FechaVencimiento = c.FechaVencimiento,
        MontoTotal = c.MontoTotal, SaldoPendiente = c.SaldoPendiente, Estado = c.Estado,
        Vencida = c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorPagar.Anulada && c.FechaVencimiento.Date < DateTime.UtcNow.Date,
        Pagos = c.Pagos.Select(MapToPagoDto).ToList()
    };

    private static PagoDto MapToPagoDto(Pago p) => new()
    {
        Id = p.Id, CuentaPorPagarId = p.CuentaPorPagarId, Fecha = p.Fecha,
        Monto = p.Monto, MedioPago = p.MedioPago, Referencia = p.Referencia
    };
}
