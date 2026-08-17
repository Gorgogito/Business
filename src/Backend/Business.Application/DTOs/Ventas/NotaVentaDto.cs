namespace Business.Application.DTOs.Ventas;

public class NotaVentaDto
{
    public int Id { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int FacturaId { get; set; }
    public string? FacturaNumero { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public string? Motivo { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<NotaVentaDetalleDto> Detalles { get; set; } = new();
}

public class NotaVentaDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}

public class CreateNotaVentaDto
{
    public int FacturaId { get; set; }
    public string Tipo { get; set; } = "CREDITO"; // CREDITO, DEBITO
    public string? Motivo { get; set; }
    public List<CreateNotaVentaDetalleDto> Detalles { get; set; } = new();
}

public class CreateNotaVentaDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
}
