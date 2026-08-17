namespace Business.Web.Models;

public class UserSession
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? RoleName { get; set; }
    public List<MenuModel> Menus { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
}

public class ClienteModel
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "RUC";
    public decimal LimiteCredito { get; set; }
    public bool IsActive { get; set; }
}

public class ProveedorModel
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class ProductoModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string Unidad { get; set; } = "UND";
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public bool IsActive { get; set; }
}

public class CategoriaModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool IsActive { get; set; }
}

public class StockModel
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public string? ProductoCodigo { get; set; }
    public int AlmacenId { get; set; }
    public string? AlmacenNombre { get; set; }
    public decimal CantidadActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal StockMaximo { get; set; }
}

public class DashboardModel
{
    public int TotalClientes { get; set; }
    public int TotalProveedores { get; set; }
    public int TotalProductos { get; set; }
    public decimal VentasMes { get; set; }
    public decimal ComprasMes { get; set; }
    public int PedidosPendientes { get; set; }
    public int OrdenesCompraPendientes { get; set; }
    public List<VentaMensualModel> VentasMensuales { get; set; } = new();
}

public class VentaMensualModel
{
    public string Mes { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class AlmacenModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool IsActive { get; set; }
}

public class MovimientoInventarioModel
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int AlmacenId { get; set; }
    public string? AlmacenNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string? Referencia { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaMovimiento { get; set; }
}

public class CotizacionModel
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<CotizacionDetalleModel> Detalles { get; set; } = new();
}

public class CotizacionDetalleModel
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}

public class PedidoModel
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int? CotizacionId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<PedidoDetalleModel> Detalles { get; set; } = new();
}

public class PedidoDetalleModel
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}

public class FacturaModel
{
    public int Id { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int? PedidoId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<FacturaDetalleModel> Detalles { get; set; } = new();
}

public class FacturaDetalleModel
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}

public class OrdenCompraModel
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public DateTime FechaEntrega { get; set; }
    public int ProveedorId { get; set; }
    public string? ProveedorNombre { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<OrdenCompraDetalleModel> Detalles { get; set; } = new();
}

public class OrdenCompraDetalleModel
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal SubTotal { get; set; }
}

public class RecepcionCompraModel
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int OrdenCompraId { get; set; }
    public string? OrdenCompraNumero { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RoleModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<int> PermissionIds { get; set; } = new();
    public List<int> MenuIds { get; set; } = new();
}

public class EmpresaModel
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class SucursalModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public string? EmpresaNombre { get; set; }
    public bool IsActive { get; set; }
}
