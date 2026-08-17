namespace Business.Application.DTOs.Produccion;

public class OrdenFabricacionDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int RecetaId { get; set; }
    public decimal CantidadProducir { get; set; }
    public int AlmacenId { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal CostoMateriaPrima { get; set; }
    public decimal CostoManoObra { get; set; }
    public decimal CostoIndirecto { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal CostoUnitario { get; set; }
    public List<OrdenFabricacionDetalleDto> Detalles { get; set; } = new();
}

public class OrdenFabricacionDetalleDto
{
    public int Id { get; set; }
    public int InsumoId { get; set; }
    public string? InsumoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal CostoTotal { get; set; }
}

public class CreateOrdenFabricacionDto
{
    public int ProductoId { get; set; }
    public decimal CantidadProducir { get; set; }
    public int AlmacenId { get; set; } = 1;
}

public class ProcesarOrdenFabricacionDto
{
    public decimal CostoManoObra { get; set; } // MOD
    public decimal CostoIndirecto { get; set; } // CIF
}
