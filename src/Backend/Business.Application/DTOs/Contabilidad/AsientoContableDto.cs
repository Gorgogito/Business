namespace Business.Application.DTOs.Contabilidad;

public class AsientoContableDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Glosa { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<AsientoDetalleDto> Detalles { get; set; } = new();
}

public class AsientoDetalleDto
{
    public int Id { get; set; }
    public int CuentaContableId { get; set; }
    public string? CuentaCodigo { get; set; }
    public string? CuentaNombre { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public string? Glosa { get; set; }
}

public class CreateAsientoContableDto
{
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Glosa { get; set; } = string.Empty;
    public List<CreateAsientoDetalleDto> Detalles { get; set; } = new();
}

public class CreateAsientoDetalleDto
{
    public int CuentaContableId { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public string? Glosa { get; set; }
}
