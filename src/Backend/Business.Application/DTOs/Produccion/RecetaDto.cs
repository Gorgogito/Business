namespace Business.Application.DTOs.Produccion;

public class RecetaDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal CantidadProducida { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<RecetaDetalleDto> Detalles { get; set; } = new();
}

public class RecetaDetalleDto
{
    public int Id { get; set; }
    public int InsumoId { get; set; }
    public string? InsumoNombre { get; set; }
    public decimal Cantidad { get; set; }
}

public class CreateRecetaDto
{
    public int ProductoId { get; set; }
    public string? Descripcion { get; set; }
    public decimal CantidadProducida { get; set; } = 1;
    public List<CreateRecetaDetalleDto> Detalles { get; set; } = new();
}

public class CreateRecetaDetalleDto
{
    public int InsumoId { get; set; }
    public decimal Cantidad { get; set; }
}
