namespace Business.Application.Interfaces;

using Business.Application.DTOs.Produccion;

public interface IOrdenFabricacionService
{
    Task<IEnumerable<OrdenFabricacionDto>> GetAllAsync();
    Task<OrdenFabricacionDto?> GetByIdAsync(int id);

    /// <summary>Crea una orden de fabricación (PENDIENTE) calculando los insumos requeridos según la receta.</summary>
    Task<OrdenFabricacionDto> CreateAsync(CreateOrdenFabricacionDto dto, string userName);

    /// <summary>
    /// Procesa la orden: consume la materia prima, incorpora MOD y CIF, valoriza el producto
    /// terminado al costo de producción y genera el asiento contable. Marca la orden TERMINADA.
    /// </summary>
    Task<OrdenFabricacionDto?> ProcesarAsync(int id, ProcesarOrdenFabricacionDto dto, string userName);

    /// <summary>
    /// Anula la orden. Si estaba terminada, revierte los movimientos: saca el producto
    /// terminado del stock, reingresa la materia prima consumida y genera el asiento inverso.
    /// </summary>
    Task<OrdenFabricacionDto?> AnularAsync(int id, string userName);
}
