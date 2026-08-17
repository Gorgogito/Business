namespace Business.Domain.Entities.Contabilidad;

using Business.Domain.Common;

/// <summary>
/// Comprobante de diario: registro contable de partida doble. La suma del debe debe
/// igualar la del haber (el asiento "cuadra"). Puede ser manual o generado por el
/// sistema desde una operación (venta, compra, cobro, pago).
/// </summary>
public class AsientoContable : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Glosa { get; set; } = string.Empty;
    /// <summary>DIARIO (manual), VENTA, COMPRA, COBRANZA, PAGO.</summary>
    public string Tipo { get; set; } = "DIARIO";
    /// <summary>Documento que originó el asiento (p. ej. "F001-00000001").</summary>
    public string? Referencia { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public string Estado { get; set; } = "REGISTRADO"; // REGISTRADO, ANULADO
    public ICollection<AsientoContableDetalle> Detalles { get; set; } = new List<AsientoContableDetalle>();
}
