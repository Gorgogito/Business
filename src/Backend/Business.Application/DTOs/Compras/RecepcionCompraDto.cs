namespace Business.Application.DTOs.Compras;

public class RecepcionCompraDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int OrdenCompraId { get; set; }
    public string? OrdenCompraNumero { get; set; }
    public int AlmacenId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public List<RecepcionCompraDetalleDto> Detalles { get; set; } = new();
}

public class RecepcionCompraDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal CantidadEsperada { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
}
