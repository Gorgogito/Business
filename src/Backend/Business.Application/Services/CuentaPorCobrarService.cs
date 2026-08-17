namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Finanzas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Finanzas;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class CuentaPorCobrarService : ICuentaPorCobrarService
{
    private readonly IRepository<CuentaPorCobrar> _repo;
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public CuentaPorCobrarService(IRepository<CuentaPorCobrar> repo, IRepository<Factura> facturaRepo, IUnitOfWork unitOfWork, IAsientoContableService asientos, IConfiguracionContableService configContable)
    {
        _repo = repo;
        _facturaRepo = facturaRepo;
        _unitOfWork = unitOfWork;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<CuentaPorCobrarDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(c => c.Cliente).Include(c => c.Factura).Include(c => c.Cobros).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<CuentaPorCobrarDto>> GetPendientesAsync()
    {
        var items = await _repo.Query().Include(c => c.Cliente).Include(c => c.Factura).Include(c => c.Cobros)
            .Where(c => c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorCobrar.Anulada).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<CuentaPorCobrarDto>> GetByClienteAsync(int clienteId)
    {
        var items = await _repo.Query().Include(c => c.Cliente).Include(c => c.Factura).Include(c => c.Cobros)
            .Where(c => c.ClienteId == clienteId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<CuentaPorCobrarDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(c => c.Cliente).Include(c => c.Factura).Include(c => c.Cobros)
            .FirstOrDefaultAsync(c => c.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task GenerarDesdeFacturaAsync(Factura factura, int diasCredito, string? userName, CancellationToken ct = default)
    {
        var cuenta = new CuentaPorCobrar
        {
            ClienteId = factura.ClienteId,
            Factura = factura, // relación por navegación: EF resuelve el FacturaId al guardar
            FechaEmision = factura.Fecha,
            FechaVencimiento = factura.Fecha.AddDays(diasCredito < 0 ? 0 : diasCredito),
            MontoTotal = factura.Total,
            SaldoPendiente = factura.Total,
            Estado = EstadoCuentaPorCobrar.Pendiente,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userName
        };
        await _repo.AddAsync(cuenta);
    }

    public async Task AnularPorFacturaAsync(int facturaId, CancellationToken ct = default)
    {
        var cuenta = await _repo.Query().FirstOrDefaultAsync(c => c.FacturaId == facturaId, ct);
        if (cuenta == null) return;

        if (cuenta.SaldoPendiente < cuenta.MontoTotal)
            throw new BusinessRuleException("No se puede anular la factura: la cuenta por cobrar ya tiene cobros aplicados.");

        cuenta.Estado = EstadoCuentaPorCobrar.Anulada;
        cuenta.SaldoPendiente = 0;
        cuenta.UpdatedAt = DateTime.UtcNow;
    }

    public async Task<CobroDto?> RegistrarCobroAsync(int cuentaPorCobrarId, CreateCobroDto dto, string userName, CancellationToken ct = default)
    {
        var cuenta = await _repo.Query().FirstOrDefaultAsync(c => c.Id == cuentaPorCobrarId, ct);
        if (cuenta == null) return null;

        if (cuenta.Estado == EstadoCuentaPorCobrar.Anulada)
            throw new BusinessRuleException("La cuenta por cobrar está anulada.");
        if (cuenta.SaldoPendiente <= 0)
            throw new BusinessRuleException("La cuenta por cobrar ya está saldada.");
        if (dto.Monto <= 0)
            throw new BusinessRuleException("El monto del cobro debe ser mayor a cero.");
        if (dto.Monto > cuenta.SaldoPendiente)
            throw new BusinessRuleException($"El cobro ({dto.Monto}) excede el saldo pendiente ({cuenta.SaldoPendiente}).");

        var cobro = new Cobro
        {
            CuentaPorCobrarId = cuenta.Id,
            Fecha = DateTime.UtcNow,
            Monto = dto.Monto,
            MedioPago = string.IsNullOrWhiteSpace(dto.MedioPago) ? "EFECTIVO" : dto.MedioPago,
            Referencia = dto.Referencia,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userName
        };
        cuenta.Cobros.Add(cobro);

        cuenta.SaldoPendiente -= dto.Monto;
        cuenta.Estado = cuenta.SaldoPendiente <= 0 ? EstadoCuentaPorCobrar.Pagada : EstadoCuentaPorCobrar.Parcial;
        cuenta.UpdatedAt = DateTime.UtcNow;

        // Si la cuenta queda saldada, la factura pasa a PAGADA.
        if (cuenta.Estado == EstadoCuentaPorCobrar.Pagada)
        {
            var factura = await _facturaRepo.GetByIdAsync(cuenta.FacturaId);
            if (factura != null)
            {
                factura.Estado = EstadoFactura.Pagada;
                factura.UpdatedAt = DateTime.UtcNow;
            }
        }

        // Asiento contable de cobranza (caja/bancos / cuentas por cobrar).
        var cuentas = await _configContable.ObtenerMapaAsync(ct);
        var cuentaCaja = string.Equals(cobro.MedioPago, "EFECTIVO", StringComparison.OrdinalIgnoreCase)
            ? cuentas[ConceptosContables.TesoreriaEfectivo] : cuentas[ConceptosContables.TesoreriaBancos];
        await _asientos.GenerarAsientoAsync(
            cobro.Fecha, $"Cobranza CxC #{cuenta.Id}", TipoAsiento.Cobranza, $"CxC-{cuenta.Id}",
            new[] { (cuentaCaja, dto.Monto, 0m), (cuentas[ConceptosContables.VentaCliente], 0m, dto.Monto) }, userName, ct);

        await _unitOfWork.SaveChangesAsync(ct);
        return MapToCobroDto(cobro);
    }

    public async Task AplicarNotaCreditoAsync(int facturaId, decimal monto, CancellationToken ct = default)
    {
        var cuenta = await _repo.Query().FirstOrDefaultAsync(c => c.FacturaId == facturaId && c.Estado != EstadoCuentaPorCobrar.Anulada, ct);
        if (cuenta == null)
            throw new BusinessRuleException("La factura no tiene una cuenta por cobrar activa para aplicar la nota de crédito.");
        if (monto > cuenta.SaldoPendiente)
            throw new BusinessRuleException($"La nota de crédito ({monto}) excede el saldo pendiente de la factura ({cuenta.SaldoPendiente}).");

        cuenta.SaldoPendiente -= monto;
        if (cuenta.SaldoPendiente <= 0)
            cuenta.Estado = EstadoCuentaPorCobrar.Pagada; // saldada
        cuenta.UpdatedAt = DateTime.UtcNow;
    }

    public async Task AplicarNotaDebitoAsync(int facturaId, decimal monto, CancellationToken ct = default)
    {
        var cuenta = await _repo.Query().FirstOrDefaultAsync(c => c.FacturaId == facturaId && c.Estado != EstadoCuentaPorCobrar.Anulada, ct);
        if (cuenta == null)
            throw new BusinessRuleException("La factura no tiene una cuenta por cobrar activa para aplicar la nota de débito.");

        cuenta.MontoTotal += monto;
        cuenta.SaldoPendiente += monto;
        cuenta.Estado = cuenta.SaldoPendiente < cuenta.MontoTotal ? EstadoCuentaPorCobrar.Parcial : EstadoCuentaPorCobrar.Pendiente;
        cuenta.UpdatedAt = DateTime.UtcNow;
    }

    private static CuentaPorCobrarDto MapToDto(CuentaPorCobrar c) => new()
    {
        Id = c.Id, ClienteId = c.ClienteId, ClienteNombre = c.Cliente?.RazonSocial,
        FacturaId = c.FacturaId, FacturaNumero = c.Factura == null ? null : $"{c.Factura.Serie}-{c.Factura.Numero}",
        FechaEmision = c.FechaEmision, FechaVencimiento = c.FechaVencimiento,
        MontoTotal = c.MontoTotal, SaldoPendiente = c.SaldoPendiente, Estado = c.Estado,
        Vencida = c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorCobrar.Anulada && c.FechaVencimiento.Date < DateTime.UtcNow.Date,
        Cobros = c.Cobros.Select(MapToCobroDto).ToList()
    };

    private static CobroDto MapToCobroDto(Cobro c) => new()
    {
        Id = c.Id, CuentaPorCobrarId = c.CuentaPorCobrarId, Fecha = c.Fecha,
        Monto = c.Monto, MedioPago = c.MedioPago, Referencia = c.Referencia
    };
}
