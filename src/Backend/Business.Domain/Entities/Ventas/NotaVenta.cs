namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>
/// Nota de crédito o débito ligada a una factura. La de crédito documenta devoluciones
/// o descuentos (reingresa stock y reduce la deuda); la de débito, cargos adicionales
/// (incrementa la deuda).
/// </summary>
public class NotaVenta : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Tipo { get; set; } = "CREDITO"; // CREDITO, DEBITO
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int FacturaId { get; set; }
    public Factura? Factura { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public string? Motivo { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "EMITIDA"; // EMITIDA, ANULADA
    public ICollection<NotaVentaDetalle> Detalles { get; set; } = new List<NotaVentaDetalle>();
}
