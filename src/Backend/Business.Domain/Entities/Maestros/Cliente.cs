namespace Business.Domain.Entities.Maestros;

using Business.Domain.Common;

public class Cliente : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "RUC";
    public decimal LimiteCredito { get; set; }
    public ICollection<Ventas.Cotizacion> Cotizaciones { get; set; } = new List<Ventas.Cotizacion>();
    public ICollection<Ventas.Pedido> Pedidos { get; set; } = new List<Ventas.Pedido>();
    public ICollection<Ventas.Factura> Facturas { get; set; } = new List<Ventas.Factura>();
}
