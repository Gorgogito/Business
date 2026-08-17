namespace Business.Application.DTOs.Ventas;

public class GuiaRemisionDto
{
    public int Id { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public DateTime FechaTraslado { get; set; }
    public int? FacturaId { get; set; }
    public string? FacturaNumero { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public string DireccionPartida { get; set; } = string.Empty;
    public string DireccionLlegada { get; set; } = string.Empty;
    public string? Transportista { get; set; }
    public string? TransportistaRuc { get; set; }
    public string? Placa { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public List<GuiaRemisionDetalleDto> Detalles { get; set; } = new();
}

public class GuiaRemisionDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public string? Descripcion { get; set; }
}

public class CreateGuiaRemisionDto
{
    public int? FacturaId { get; set; }       // opcional; si se indica, arrastra cliente y (si no hay detalles) los ítems
    public int ClienteId { get; set; }
    public DateTime FechaTraslado { get; set; } = DateTime.UtcNow;
    public string DireccionPartida { get; set; } = string.Empty;
    public string DireccionLlegada { get; set; } = string.Empty;
    public string? Transportista { get; set; }
    public string? TransportistaRuc { get; set; }
    public string? Placa { get; set; }
    public string Motivo { get; set; } = "VENTA";
    public List<CreateGuiaRemisionDetalleDto> Detalles { get; set; } = new();
}

public class CreateGuiaRemisionDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public string? Descripcion { get; set; }
}
