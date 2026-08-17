namespace Business.Domain.Entities.Inventario;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class MovimientoInventario : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Tipo { get; set; } = string.Empty; // ENTRADA, SALIDA, TRASLADO
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int AlmacenId { get; set; }
    public Almacen? Almacen { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    /// <summary>Costo unitario aplicado: precio de compra en entradas, costo promedio en salidas.</summary>
    public decimal CostoUnitario { get; set; }
    /// <summary>Valorización del movimiento (Cantidad × CostoUnitario).</summary>
    public decimal CostoTotal { get; set; }
    public string? Referencia { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;
}
