namespace Business.Application.Interfaces;

using Business.Domain.Entities.Inventario;

/// <summary>
/// Aplica movimientos de inventario (entradas/salidas) ajustando el stock del producto
/// en un almacén. Los cambios se agregan al contexto pero NO se persisten: el llamador
/// decide cuándo guardar, permitiendo que el movimiento sea atómico junto con el
/// documento que lo origina (factura, recepción, etc.).
/// </summary>
public interface IInventarioService
{
    /// <summary>
    /// Registra un movimiento de inventario y ajusta el stock. Para salidas puede validar
    /// que exista disponibilidad suficiente. No llama a SaveChanges.
    /// </summary>
    /// <param name="tipo">ENTRADA o SALIDA.</param>
    /// <param name="validarDisponibilidad">Si es true y no hay stock suficiente para una salida, lanza excepción.</param>
    Task<MovimientoInventario> RegistrarMovimientoAsync(
        string tipo,
        int productoId,
        int almacenId,
        decimal cantidad,
        decimal precioUnitario,
        string? referencia,
        string? observacion,
        string? userName,
        bool validarDisponibilidad,
        CancellationToken ct = default);
}
