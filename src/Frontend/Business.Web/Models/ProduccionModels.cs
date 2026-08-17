namespace Business.Web.Models;

public class RecetaModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal CantidadProducida { get; set; } = 1;
    public string Estado { get; set; } = string.Empty;
    public List<RecetaDetalleModel> Detalles { get; set; } = new();
}

public class RecetaDetalleModel
{
    public int InsumoId { get; set; }
    public string? InsumoNombre { get; set; }
    public decimal Cantidad { get; set; }
}

public class OrdenFabricacionModel
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
    public List<OrdenFabricacionDetalleModel> Detalles { get; set; } = new();
}

public class OrdenFabricacionDetalleModel
{
    public int InsumoId { get; set; }
    public string? InsumoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal CostoTotal { get; set; }
}
