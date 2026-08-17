namespace Business.Application.DTOs.Compras;

public class CreateRecepcionCompraDto
{
    public int OrdenCompraId { get; set; }
    public int AlmacenId { get; set; } = 1; // Almacén al que ingresa la mercadería
    public int DiasCredito { get; set; } = 0; // Plazo de pago al proveedor; 0 = contado
    public string? Observaciones { get; set; }
    public List<CreateRecepcionDetalleDto> Detalles { get; set; } = new();
}

public class CreateRecepcionDetalleDto
{
    public int ProductoId { get; set; }
    public decimal CantidadEsperada { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
}
