namespace Business.Application.Services;

using Business.Application.Common;
using Business.Application.Interfaces;
using Business.Domain.Entities.Inventario;
using Business.Domain.Interfaces;

/// <summary>
/// Lógica central de movimientos de inventario, reutilizada por movimientos manuales,
/// facturación (salidas) y recepciones de compra (entradas). No persiste cambios.
/// </summary>
public class InventarioService : IInventarioService
{
    public const string Entrada = "ENTRADA";
    public const string Salida = "SALIDA";

    private readonly IRepository<MovimientoInventario> _movRepo;
    private readonly IRepository<Stock> _stockRepo;

    public InventarioService(IRepository<MovimientoInventario> movRepo, IRepository<Stock> stockRepo)
    {
        _movRepo = movRepo;
        _stockRepo = stockRepo;
    }

    public async Task<MovimientoInventario> RegistrarMovimientoAsync(
        string tipo,
        int productoId,
        int almacenId,
        decimal cantidad,
        decimal precioUnitario,
        string? referencia,
        string? observacion,
        string? userName,
        bool validarDisponibilidad,
        CancellationToken ct = default)
    {
        var esSalida = string.Equals(tipo, Salida, StringComparison.OrdinalIgnoreCase);

        var stock = (await _stockRepo.FindAsync(s => s.ProductoId == productoId && s.AlmacenId == almacenId))
            .FirstOrDefault();

        if (esSalida && validarDisponibilidad)
        {
            var disponible = stock?.CantidadActual ?? 0m;
            if (disponible < cantidad)
            {
                throw new BusinessRuleException(
                    $"Stock insuficiente para el producto {productoId} en el almacén {almacenId}: disponible {disponible}, requerido {cantidad}.");
            }
        }

        if (stock == null)
        {
            stock = new Stock
            {
                ProductoId = productoId,
                AlmacenId = almacenId,
                CantidadActual = 0m,
                CostoPromedio = 0m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _stockRepo.AddAsync(stock);
        }

        // Valorización por costo promedio ponderado móvil.
        decimal costoUnitario;
        if (esSalida)
        {
            // La salida se valoriza al costo promedio vigente; el promedio no cambia.
            costoUnitario = stock.CostoPromedio;
            stock.CantidadActual -= cantidad;
        }
        else
        {
            // La entrada recalcula el costo promedio combinando existencia y nuevo ingreso.
            var valorPrevio = stock.CantidadActual * stock.CostoPromedio;
            var valorIngreso = cantidad * precioUnitario;
            var nuevaCantidad = stock.CantidadActual + cantidad;
            stock.CostoPromedio = nuevaCantidad > 0 ? (valorPrevio + valorIngreso) / nuevaCantidad : precioUnitario;
            stock.CantidadActual = nuevaCantidad;
            costoUnitario = precioUnitario;
        }
        stock.UpdatedAt = DateTime.UtcNow;

        var movimiento = new MovimientoInventario
        {
            Tipo = esSalida ? Salida : Entrada,
            ProductoId = productoId,
            AlmacenId = almacenId,
            Cantidad = cantidad,
            PrecioUnitario = precioUnitario,
            CostoUnitario = costoUnitario,
            CostoTotal = cantidad * costoUnitario,
            Referencia = referencia,
            Observacion = observacion,
            FechaMovimiento = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userName
        };
        await _movRepo.AddAsync(movimiento);

        return movimiento;
    }
}
