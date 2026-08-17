namespace Business.Application.DTOs.Ventas;

public class CreateFacturaDto
{
    public int ClienteId { get; set; }
    public int? PedidoId { get; set; }
    public int AlmacenId { get; set; } = 1; // Almacén del que se descuenta el stock
    public int DiasCredito { get; set; } = 0; // Plazo de pago; 0 = contado (vence el mismo día)
    public string TipoDocumento { get; set; } = "FACTURA";
    public string? Observaciones { get; set; }
    public List<CreateFacturaDetalleDto> Detalles { get; set; } = new();
}

public class CreateFacturaDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
}
