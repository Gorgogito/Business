namespace Business.Domain.Entities.Contabilidad;

using Business.Domain.Common;

/// <summary>Línea de un asiento: afecta una cuenta por el debe o por el haber.</summary>
public class AsientoContableDetalle : BaseEntity
{
    public int AsientoContableId { get; set; }
    public AsientoContable? AsientoContable { get; set; }
    public int CuentaContableId { get; set; }
    public CuentaContable? CuentaContable { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public string? Glosa { get; set; }
}
