namespace Business.Persistence.Context;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Security;
using Business.Domain.Entities.Configuration;
using Business.Domain.Entities.Maestros;
using Business.Domain.Entities.Inventario;
using Business.Domain.Entities.Ventas;
using Business.Domain.Entities.Compras;
using Business.Domain.Entities.Finanzas;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Entities.Produccion;
using Business.Domain.Entities.Auditoria;
using Business.Persistence.Auditing;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUser;
    // Empresa del usuario actual; capturada por los filtros multiempresa (null = sin filtro).
    private readonly int? _empresaId;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService? currentUser = null)
        : base(options)
    {
        _currentUser = currentUser;
        _empresaId = currentUser?.EmpresaId;
    }

    // Security
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();

    // Configuration
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Parametro> Parametros => Set<Parametro>();
    public DbSet<Correlativo> Correlativos => Set<Correlativo>();

    // Finanzas
    public DbSet<CuentaPorCobrar> CuentasPorCobrar => Set<CuentaPorCobrar>();
    public DbSet<Cobro> Cobros => Set<Cobro>();
    public DbSet<CuentaPorPagar> CuentasPorPagar => Set<CuentaPorPagar>();
    public DbSet<Pago> Pagos => Set<Pago>();

    // Contabilidad
    public DbSet<CuentaContable> CuentasContables => Set<CuentaContable>();
    public DbSet<AsientoContable> AsientosContables => Set<AsientoContable>();
    public DbSet<AsientoContableDetalle> AsientoContableDetalles => Set<AsientoContableDetalle>();
    public DbSet<ConfiguracionCuentaContable> ConfiguracionesCuentasContables => Set<ConfiguracionCuentaContable>();

    // RR.HH.
    public DbSet<Trabajador> Trabajadores => Set<Trabajador>();
    public DbSet<ConceptoPlanilla> ConceptosPlanilla => Set<ConceptoPlanilla>();
    public DbSet<TasaAfp> TasasAfp => Set<TasaAfp>();
    public DbSet<Planilla> Planillas => Set<Planilla>();
    public DbSet<PlanillaDetalle> PlanillaDetalles => Set<PlanillaDetalle>();
    public DbSet<PlanillaConcepto> PlanillaConceptos => Set<PlanillaConcepto>();
    public DbSet<BeneficioSocial> BeneficiosSociales => Set<BeneficioSocial>();

    // Producción / Costos
    public DbSet<Receta> Recetas => Set<Receta>();
    public DbSet<RecetaDetalle> RecetaDetalles => Set<RecetaDetalle>();
    public DbSet<OrdenFabricacion> OrdenesFabricacion => Set<OrdenFabricacion>();
    public DbSet<OrdenFabricacionDetalle> OrdenFabricacionDetalles => Set<OrdenFabricacionDetalle>();

    // Auditoría
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Maestros
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();

    // Inventario
    public DbSet<Almacen> Almacenes => Set<Almacen>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();

    // Ventas
    public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
    public DbSet<CotizacionDetalle> CotizacionDetalles => Set<CotizacionDetalle>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoDetalle> PedidoDetalles => Set<PedidoDetalle>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<FacturaDetalle> FacturaDetalles => Set<FacturaDetalle>();
    public DbSet<NotaVenta> NotasVenta => Set<NotaVenta>();
    public DbSet<NotaVentaDetalle> NotaVentaDetalles => Set<NotaVentaDetalle>();
    public DbSet<GuiaRemision> GuiasRemision => Set<GuiaRemision>();
    public DbSet<GuiaRemisionDetalle> GuiaRemisionDetalles => Set<GuiaRemisionDetalle>();

    // Compras
    public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
    public DbSet<OrdenCompraDetalle> OrdenCompraDetalles => Set<OrdenCompraDetalle>();
    public DbSet<RecepcionCompra> RecepcionesCompra => Set<RecepcionCompra>();
    public DbSet<RecepcionCompraDetalle> RecepcionCompraDetalles => Set<RecepcionCompraDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // RolePermission composite key
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        // RoleMenu composite key
        modelBuilder.Entity<RoleMenu>()
            .HasKey(rm => new { rm.RoleId, rm.MenuId });

        // Indexes
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(e => e.RUC).IsUnique();
        modelBuilder.Entity<Cliente>().HasIndex(c => new { c.EmpresaId, c.RUC }).IsUnique();
        modelBuilder.Entity<Proveedor>().HasIndex(p => new { p.EmpresaId, p.RUC }).IsUnique();
        modelBuilder.Entity<Producto>().HasIndex(p => new { p.EmpresaId, p.Codigo }).IsUnique();
        modelBuilder.Entity<Almacen>().HasIndex(a => new { a.EmpresaId, a.Codigo }).IsUnique();
        modelBuilder.Entity<Sucursal>().HasIndex(s => new { s.EmpresaId, s.Codigo }).IsUnique();
        modelBuilder.Entity<Correlativo>().HasIndex(c => new { c.TipoDocumento, c.Serie, c.EmpresaId }).IsUnique();
        modelBuilder.Entity<AuditLog>().HasIndex(a => new { a.TableName, a.EntityId });
        modelBuilder.Entity<AuditLog>().HasIndex(a => a.Timestamp);
        modelBuilder.Entity<CuentaPorCobrar>().Property(c => c.MontoTotal).HasPrecision(18, 2);
        modelBuilder.Entity<CuentaPorCobrar>().Property(c => c.SaldoPendiente).HasPrecision(18, 2);
        modelBuilder.Entity<CuentaPorCobrar>().HasIndex(c => c.FacturaId);
        modelBuilder.Entity<CuentaPorCobrar>().HasOne(c => c.Cliente).WithMany().HasForeignKey(c => c.ClienteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CuentaPorCobrar>().HasOne(c => c.Factura).WithMany().HasForeignKey(c => c.FacturaId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Cobro>().Property(c => c.Monto).HasPrecision(18, 2);
        modelBuilder.Entity<CuentaPorPagar>().Property(c => c.MontoTotal).HasPrecision(18, 2);
        modelBuilder.Entity<CuentaPorPagar>().Property(c => c.SaldoPendiente).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVenta>().Property(n => n.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVenta>().Property(n => n.Igv).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVenta>().Property(n => n.Total).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVenta>().HasIndex(n => n.FacturaId);
        modelBuilder.Entity<NotaVenta>().HasOne(n => n.Factura).WithMany().HasForeignKey(n => n.FacturaId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<NotaVenta>().HasOne(n => n.Cliente).WithMany().HasForeignKey(n => n.ClienteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<NotaVentaDetalle>().Property(d => d.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<NotaVentaDetalle>().Property(d => d.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVentaDetalle>().Property(d => d.Descuento).HasPrecision(18, 2);
        modelBuilder.Entity<NotaVentaDetalle>().Property(d => d.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<GuiaRemision>().HasIndex(g => g.FacturaId);
        modelBuilder.Entity<GuiaRemision>().HasOne(g => g.Factura).WithMany().HasForeignKey(g => g.FacturaId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<GuiaRemision>().HasOne(g => g.Cliente).WithMany().HasForeignKey(g => g.ClienteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<GuiaRemisionDetalle>().Property(d => d.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<CuentaContable>().HasIndex(c => new { c.EmpresaId, c.Codigo }).IsUnique();
        modelBuilder.Entity<ConfiguracionCuentaContable>().HasIndex(c => new { c.EmpresaId, c.Concepto }).IsUnique();
        modelBuilder.Entity<ConfiguracionCuentaContable>().HasOne(c => c.CuentaContable).WithMany().HasForeignKey(c => c.CuentaContableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<AsientoContable>().Property(a => a.TotalDebe).HasPrecision(18, 2);
        modelBuilder.Entity<AsientoContable>().Property(a => a.TotalHaber).HasPrecision(18, 2);
        modelBuilder.Entity<AsientoContable>().HasIndex(a => a.Fecha);
        modelBuilder.Entity<AsientoContableDetalle>().Property(d => d.Debe).HasPrecision(18, 2);
        modelBuilder.Entity<AsientoContableDetalle>().Property(d => d.Haber).HasPrecision(18, 2);
        modelBuilder.Entity<AsientoContableDetalle>().HasOne(d => d.CuentaContable).WithMany().HasForeignKey(d => d.CuentaContableId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Trabajador>().HasIndex(t => new { t.EmpresaId, t.Codigo }).IsUnique();
        modelBuilder.Entity<Trabajador>().Property(t => t.SueldoBasico).HasPrecision(18, 2);
        modelBuilder.Entity<ConceptoPlanilla>().HasIndex(c => new { c.EmpresaId, c.Codigo }).IsUnique();
        modelBuilder.Entity<ConceptoPlanilla>().Property(c => c.Porcentaje).HasPrecision(9, 6);
        modelBuilder.Entity<ConceptoPlanilla>().Property(c => c.MontoFijo).HasPrecision(18, 2);
        modelBuilder.Entity<Planilla>().Property(p => p.TotalIngresos).HasPrecision(18, 2);
        modelBuilder.Entity<Planilla>().Property(p => p.TotalDescuentos).HasPrecision(18, 2);
        modelBuilder.Entity<Planilla>().Property(p => p.TotalAportes).HasPrecision(18, 2);
        modelBuilder.Entity<Planilla>().Property(p => p.TotalNeto).HasPrecision(18, 2);
        modelBuilder.Entity<Planilla>().HasIndex(p => new { p.EmpresaId, p.Anio, p.Mes });
        modelBuilder.Entity<PlanillaDetalle>().Property(d => d.SueldoBasico).HasPrecision(18, 2);
        modelBuilder.Entity<PlanillaDetalle>().Property(d => d.TotalIngresos).HasPrecision(18, 2);
        modelBuilder.Entity<PlanillaDetalle>().Property(d => d.TotalDescuentos).HasPrecision(18, 2);
        modelBuilder.Entity<PlanillaDetalle>().Property(d => d.TotalAportes).HasPrecision(18, 2);
        modelBuilder.Entity<PlanillaDetalle>().Property(d => d.NetoPagar).HasPrecision(18, 2);
        modelBuilder.Entity<PlanillaDetalle>().HasOne(d => d.Trabajador).WithMany().HasForeignKey(d => d.TrabajadorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PlanillaConcepto>().Property(c => c.Monto).HasPrecision(18, 2);
        modelBuilder.Entity<TasaAfp>().HasIndex(t => t.Nombre).IsUnique();
        modelBuilder.Entity<TasaAfp>().Property(t => t.AporteFondo).HasPrecision(9, 6);
        modelBuilder.Entity<TasaAfp>().Property(t => t.ComisionFlujo).HasPrecision(9, 6);
        modelBuilder.Entity<TasaAfp>().Property(t => t.PrimaSeguro).HasPrecision(9, 6);
        modelBuilder.Entity<BeneficioSocial>().Property(b => b.MesesComputables).HasPrecision(9, 4);
        modelBuilder.Entity<BeneficioSocial>().Property(b => b.RemuneracionComputable).HasPrecision(18, 2);
        modelBuilder.Entity<BeneficioSocial>().Property(b => b.Monto).HasPrecision(18, 2);
        modelBuilder.Entity<BeneficioSocial>().Property(b => b.BonificacionExtraordinaria).HasPrecision(18, 2);
        modelBuilder.Entity<BeneficioSocial>().HasIndex(b => new { b.EmpresaId, b.TrabajadorId, b.Tipo, b.Periodo });
        modelBuilder.Entity<BeneficioSocial>().HasOne(b => b.Trabajador).WithMany().HasForeignKey(b => b.TrabajadorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Receta>().Property(r => r.CantidadProducida).HasPrecision(18, 4);
        modelBuilder.Entity<Receta>().HasOne(r => r.Producto).WithMany().HasForeignKey(r => r.ProductoId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<RecetaDetalle>().Property(d => d.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<RecetaDetalle>().HasOne(d => d.Insumo).WithMany().HasForeignKey(d => d.InsumoId).OnDelete(DeleteBehavior.Restrict);
        foreach (var prop in new[] { "CantidadProducir", "CostoMateriaPrima", "CostoManoObra", "CostoIndirecto", "CostoTotal", "CostoUnitario" })
            modelBuilder.Entity<OrdenFabricacion>().Property(prop).HasPrecision(18, 4);
        modelBuilder.Entity<OrdenFabricacion>().HasOne(o => o.Producto).WithMany().HasForeignKey(o => o.ProductoId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OrdenFabricacion>().HasOne(o => o.Receta).WithMany().HasForeignKey(o => o.RecetaId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OrdenFabricacionDetalle>().Property(d => d.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<OrdenFabricacionDetalle>().Property(d => d.CostoUnitario).HasPrecision(18, 4);
        modelBuilder.Entity<OrdenFabricacionDetalle>().Property(d => d.CostoTotal).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenFabricacionDetalle>().HasOne(d => d.Insumo).WithMany().HasForeignKey(d => d.InsumoId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CuentaPorPagar>().HasIndex(c => c.RecepcionCompraId);
        modelBuilder.Entity<CuentaPorPagar>().HasOne(c => c.Proveedor).WithMany().HasForeignKey(c => c.ProveedorId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<CuentaPorPagar>().HasOne(c => c.RecepcionCompra).WithMany().HasForeignKey(c => c.RecepcionCompraId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Pago>().Property(p => p.Monto).HasPrecision(18, 2);

        // Decimal precision
        modelBuilder.Entity<Cliente>().Property(c => c.LimiteCredito).HasPrecision(18, 2);
        modelBuilder.Entity<Producto>().Property(p => p.PrecioCompra).HasPrecision(18, 2);
        modelBuilder.Entity<Producto>().Property(p => p.PrecioVenta).HasPrecision(18, 2);
        modelBuilder.Entity<Stock>().Property(s => s.CantidadActual).HasPrecision(18, 4);
        modelBuilder.Entity<Stock>().Property(s => s.StockMinimo).HasPrecision(18, 4);
        modelBuilder.Entity<Stock>().Property(s => s.StockMaximo).HasPrecision(18, 4);
        modelBuilder.Entity<Stock>().Property(s => s.CostoPromedio).HasPrecision(18, 4);
        modelBuilder.Entity<MovimientoInventario>().Property(m => m.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<MovimientoInventario>().Property(m => m.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<MovimientoInventario>().Property(m => m.CostoUnitario).HasPrecision(18, 4);
        modelBuilder.Entity<MovimientoInventario>().Property(m => m.CostoTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Cotizacion>().Property(c => c.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Cotizacion>().Property(c => c.Igv).HasPrecision(18, 2);
        modelBuilder.Entity<Cotizacion>().Property(c => c.Total).HasPrecision(18, 2);
        modelBuilder.Entity<CotizacionDetalle>().Property(c => c.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<CotizacionDetalle>().Property(c => c.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<CotizacionDetalle>().Property(c => c.Descuento).HasPrecision(18, 2);
        modelBuilder.Entity<CotizacionDetalle>().Property(c => c.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(p => p.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(p => p.Igv).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(p => p.Total).HasPrecision(18, 2);
        modelBuilder.Entity<PedidoDetalle>().Property(p => p.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<PedidoDetalle>().Property(p => p.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<PedidoDetalle>().Property(p => p.Descuento).HasPrecision(18, 2);
        modelBuilder.Entity<PedidoDetalle>().Property(p => p.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.Igv).HasPrecision(18, 2);
        modelBuilder.Entity<Factura>().Property(f => f.Total).HasPrecision(18, 2);
        modelBuilder.Entity<FacturaDetalle>().Property(f => f.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<FacturaDetalle>().Property(f => f.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<FacturaDetalle>().Property(f => f.Descuento).HasPrecision(18, 2);
        modelBuilder.Entity<FacturaDetalle>().Property(f => f.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompra>().Property(o => o.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompra>().Property(o => o.Igv).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompra>().Property(o => o.Total).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompraDetalle>().Property(o => o.Cantidad).HasPrecision(18, 4);
        modelBuilder.Entity<OrdenCompraDetalle>().Property(o => o.CantidadRecibida).HasPrecision(18, 4);
        modelBuilder.Entity<OrdenCompraDetalle>().Property(o => o.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompraDetalle>().Property(o => o.SubTotal).HasPrecision(18, 2);
        modelBuilder.Entity<RecepcionCompraDetalle>().Property(r => r.CantidadEsperada).HasPrecision(18, 4);
        modelBuilder.Entity<RecepcionCompraDetalle>().Property(r => r.CantidadRecibida).HasPrecision(18, 4);
        modelBuilder.Entity<RecepcionCompraDetalle>().Property(r => r.PrecioUnitario).HasPrecision(18, 2);

        // Filtro global de soft-delete para entidades NO multiempresa: las inactivas no
        // aparecen en ninguna consulta. Las multiempresa llevan un filtro combinado aparte.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                && !typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyAccess = Expression.Property(parameter, nameof(BaseEntity.IsActive));
                var lambda = Expression.Lambda(propertyAccess, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        // Filtro combinado (soft-delete + empresa) para entidades multiempresa.
        ConfigurarTenant<Cliente>(modelBuilder);
        ConfigurarTenant<Proveedor>(modelBuilder);
        ConfigurarTenant<Producto>(modelBuilder);
        ConfigurarTenant<Categoria>(modelBuilder);
        ConfigurarTenant<Almacen>(modelBuilder);
        ConfigurarTenant<Stock>(modelBuilder);
        ConfigurarTenant<MovimientoInventario>(modelBuilder);
        ConfigurarTenant<Cotizacion>(modelBuilder);
        ConfigurarTenant<Pedido>(modelBuilder);
        ConfigurarTenant<Factura>(modelBuilder);
        ConfigurarTenant<NotaVenta>(modelBuilder);
        ConfigurarTenant<GuiaRemision>(modelBuilder);
        ConfigurarTenant<OrdenCompra>(modelBuilder);
        ConfigurarTenant<RecepcionCompra>(modelBuilder);
        ConfigurarTenant<CuentaPorCobrar>(modelBuilder);
        ConfigurarTenant<CuentaPorPagar>(modelBuilder);
        ConfigurarTenant<AsientoContable>(modelBuilder);
        ConfigurarTenant<Trabajador>(modelBuilder);
        ConfigurarTenant<Planilla>(modelBuilder);
        ConfigurarTenant<BeneficioSocial>(modelBuilder);
        ConfigurarTenant<Receta>(modelBuilder);
        ConfigurarTenant<OrdenFabricacion>(modelBuilder);
        ConfigurarTenant<CuentaContable>(modelBuilder);
        ConfigurarTenant<ConceptoPlanilla>(modelBuilder);
        ConfigurarTenant<ConfiguracionCuentaContable>(modelBuilder);

        // Seed data
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Configura una entidad multiempresa: valor por defecto de EmpresaId = 1 y filtro que
    /// muestra solo registros activos de la empresa del usuario (o todos si no hay empresa).
    /// El acceso a _empresaId se re-evalúa por consulta (patrón de filtro dinámico de EF).
    /// </summary>
    private void ConfigurarTenant<T>(ModelBuilder modelBuilder) where T : BaseEntity, ITenantEntity
    {
        modelBuilder.Entity<T>().Property(e => e.EmpresaId).HasDefaultValue(1);
        modelBuilder.Entity<T>().HasQueryFilter(e => e.IsActive && (_empresaId == null || e.EmpresaId == _empresaId));
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Administrador", Description = "Acceso total al sistema", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Role { Id = 2, Name = "Vendedor", Description = "Acceso a módulos de ventas", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Role { Id = 3, Name = "Almacenero", Description = "Acceso a módulos de inventario", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Permissions (taxonomía por módulo: <modulo>.view / <modulo>.manage)
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1,  Name = "Ver Seguridad",        Code = "security.view",    Module = "Seguridad",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 2,  Name = "Gestionar Seguridad",  Code = "security.manage",  Module = "Seguridad",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 3,  Name = "Ver Configuración",    Code = "config.view",      Module = "Configuración", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 4,  Name = "Gestionar Configuración", Code = "config.manage", Module = "Configuración", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 5,  Name = "Ver Maestros",         Code = "masters.view",     Module = "Maestros",      IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 6,  Name = "Gestionar Maestros",   Code = "masters.manage",   Module = "Maestros",      IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 7,  Name = "Ver Inventario",       Code = "inventory.view",   Module = "Inventario",    IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 8,  Name = "Gestionar Inventario", Code = "inventory.manage", Module = "Inventario",    IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 9,  Name = "Ver Ventas",           Code = "sales.view",       Module = "Ventas",        IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 10, Name = "Gestionar Ventas",     Code = "sales.manage",     Module = "Ventas",        IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 11, Name = "Ver Compras",          Code = "purchases.view",   Module = "Compras",       IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 12, Name = "Gestionar Compras",    Code = "purchases.manage", Module = "Compras",       IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 13, Name = "Ver Contabilidad",       Code = "accounting.view",   Module = "Contabilidad", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 14, Name = "Gestionar Contabilidad", Code = "accounting.manage", Module = "Contabilidad", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 15, Name = "Ver RR.HH.",             Code = "hr.view",           Module = "RR.HH.",       IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 16, Name = "Gestionar RR.HH.",       Code = "hr.manage",         Module = "RR.HH.",       IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 17, Name = "Ver Producción",         Code = "production.view",   Module = "Producción",   IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Permission { Id = 18, Name = "Gestionar Producción",   Code = "production.manage", Module = "Producción",   IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Menus
        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = 1, Name = "Dashboard", Icon = "dashboard", Route = "/", Order = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 2, Name = "Seguridad", Icon = "security", Route = "#", Order = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 3, Name = "Usuarios", Icon = "people", Route = "/usuarios", Order = 1, ParentId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 4, Name = "Roles", Icon = "admin_panel_settings", Route = "/roles", Order = 2, ParentId = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 5, Name = "Configuración", Icon = "settings", Route = "#", Order = 3, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 6, Name = "Empresas", Icon = "business", Route = "/empresas", Order = 1, ParentId = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 7, Name = "Sucursales", Icon = "store", Route = "/sucursales", Order = 2, ParentId = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 8, Name = "Maestros", Icon = "library_books", Route = "#", Order = 4, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 9, Name = "Clientes", Icon = "person", Route = "/clientes", Order = 1, ParentId = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 10, Name = "Proveedores", Icon = "local_shipping", Route = "/proveedores", Order = 2, ParentId = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 11, Name = "Productos", Icon = "inventory_2", Route = "/productos", Order = 3, ParentId = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 12, Name = "Categorías", Icon = "category", Route = "/categorias", Order = 4, ParentId = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 13, Name = "Inventario", Icon = "warehouse", Route = "#", Order = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 14, Name = "Almacenes", Icon = "home_work", Route = "/almacenes", Order = 1, ParentId = 13, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 15, Name = "Stock", Icon = "storage", Route = "/stock", Order = 2, ParentId = 13, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 16, Name = "Movimientos", Icon = "swap_horiz", Route = "/movimientos", Order = 3, ParentId = 13, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 17, Name = "Ventas", Icon = "point_of_sale", Route = "#", Order = 6, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 18, Name = "Cotizaciones", Icon = "request_quote", Route = "/cotizaciones", Order = 1, ParentId = 17, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 19, Name = "Pedidos", Icon = "shopping_cart", Route = "/pedidos", Order = 2, ParentId = 17, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 20, Name = "Facturas", Icon = "receipt", Route = "/facturas", Order = 3, ParentId = 17, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 21, Name = "Compras", Icon = "shopping_bag", Route = "#", Order = 7, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 22, Name = "Órdenes de Compra", Icon = "assignment", Route = "/ordenes-compra", Order = 1, ParentId = 21, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 23, Name = "Recepciones", Icon = "move_to_inbox", Route = "/recepciones", Order = 2, ParentId = 21, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 24, Name = "Reportes", Icon = "bar_chart", Route = "/reportes", Order = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Menu { Id = 25, Name = "Analítica", Icon = "insights", Route = "/analitica", Order = 9, IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Empresa
        modelBuilder.Entity<Empresa>().HasData(
            new Empresa { Id = 1, RazonSocial = "Business SAC", RUC = "20123456789", Direccion = "Av. Principal 123", Telefono = "01-1234567", Email = "info@business.pe", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Admin User (password: Admin123!)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@business.pe",
                PasswordHash = "$2a$11$ooc/TqBVjkfgwfgJtmrU7ugYIsZN5T6piM8n.Qzn0dnzjCb1CJis.",
                FirstName = "Administrador",
                LastName = "Sistema",
                RoleId = 1,
                EmpresaId = 1,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );

        // Seed Role-Menu relationships (Admin gets all menus)
        var roleMenus = Enumerable.Range(1, 25).Select(menuId => new RoleMenu { RoleId = 1, MenuId = menuId }).ToArray();
        modelBuilder.Entity<RoleMenu>().HasData(roleMenus);

        // Seed Role-Permission relationships
        var rolePermissions = new List<RolePermission>();
        // Administrador (1): todos los permisos
        rolePermissions.AddRange(Enumerable.Range(1, 18).Select(permId => new RolePermission { RoleId = 1, PermissionId = permId }));
        // Vendedor (2): ver maestros, ver y gestionar ventas
        rolePermissions.AddRange(new[] { 5, 9, 10 }.Select(permId => new RolePermission { RoleId = 2, PermissionId = permId }));
        // Almacenero (3): ver maestros, ver/gestionar inventario y compras
        rolePermissions.AddRange(new[] { 5, 7, 8, 11, 12 }.Select(permId => new RolePermission { RoleId = 3, PermissionId = permId }));
        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);

        // Seed Categorias
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Electrónicos", Descripcion = "Equipos electrónicos", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Categoria { Id = 2, Nombre = "Oficina", Descripcion = "Materiales de oficina", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Almacen
        modelBuilder.Entity<Almacen>().HasData(
            new Almacen { Id = 1, Codigo = "ALM001", Nombre = "Almacén Principal", Ubicacion = "Planta Baja", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Parametros
        modelBuilder.Entity<Parametro>().HasData(
            new Parametro { Id = 1, Codigo = "IGV", Nombre = "Tasa IGV", Valor = "18", Descripcion = "Porcentaje de IGV", Modulo = "General", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Parametro { Id = 2, Codigo = "MONEDA", Nombre = "Moneda", Valor = "PEN", Descripcion = "Moneda del sistema", Modulo = "General", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Parametro { Id = 3, Codigo = "RMV", Nombre = "Remuneración Mínima Vital", Valor = "1025", Descripcion = "RMV vigente (base de asignación familiar)", Modulo = "RR.HH.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Parametro { Id = 4, Codigo = "TOPE_ASEGURABLE_AFP", Nombre = "Tope remuneración asegurable AFP", Valor = "10878", Descripcion = "Tope de la base de cálculo de la prima de seguro AFP (SBS)", Modulo = "RR.HH.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Correlativos (numeración por tipo de documento y serie, empresa 1)
        modelBuilder.Entity<Correlativo>().HasData(
            new Correlativo { Id = 1, TipoDocumento = "COTIZACION",   Serie = "COT",  EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "COT-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 2, TipoDocumento = "PEDIDO",       Serie = "PED",  EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "PED-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 3, TipoDocumento = "FACTURA",      Serie = "F001", EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 4, TipoDocumento = "BOLETA",       Serie = "B001", EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 5, TipoDocumento = "ORDEN_COMPRA", Serie = "OC",   EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "OC-",  IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 6, TipoDocumento = "RECEPCION",    Serie = "REC",  EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "REC-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 7, TipoDocumento = "NOTA_CREDITO", Serie = "FC01", EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 8, TipoDocumento = "NOTA_DEBITO",  Serie = "FD01", EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 9, TipoDocumento = "GUIA_REMISION", Serie = "T001", EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "",     IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 10, TipoDocumento = "ASIENTO",       Serie = "ASI",  EmpresaId = 1, UltimoNumero = 0, Longitud = 8, Prefijo = "ASI-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 11, TipoDocumento = "TRABAJADOR",    Serie = "T",    EmpresaId = 1, UltimoNumero = 0, Longitud = 6, Prefijo = "T-",   IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 12, TipoDocumento = "PLANILLA",      Serie = "PLA",  EmpresaId = 1, UltimoNumero = 0, Longitud = 6, Prefijo = "PLA-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 13, TipoDocumento = "RECETA",        Serie = "RCT",  EmpresaId = 1, UltimoNumero = 0, Longitud = 6, Prefijo = "RCT-", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new Correlativo { Id = 14, TipoDocumento = "ORDEN_FAB",     Serie = "OF",   EmpresaId = 1, UltimoNumero = 0, Longitud = 6, Prefijo = "OF-",  IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed Plan Contable (PCGE) — cuentas clave para los asientos automáticos
        CuentaContable Cuenta(int id, string codigo, string nombre, string clase, bool mov) => new()
        {
            Id = id, Codigo = codigo, Nombre = nombre, Clase = clase,
            Naturaleza = NaturalezaCuenta.DesdeClase(clase), Nivel = codigo.Length, EsMovimiento = mov,
            IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
        };
        modelBuilder.Entity<CuentaContable>().HasData(
            Cuenta(1,  "10",   "Efectivo y equivalentes de efectivo",              ClaseCuenta.Activo,     false),
            Cuenta(2,  "101",  "Caja",                                             ClaseCuenta.Activo,     true),
            Cuenta(3,  "104",  "Cuentas corrientes en instituciones financieras",  ClaseCuenta.Activo,     true),
            Cuenta(4,  "12",   "Cuentas por cobrar comerciales - Terceros",        ClaseCuenta.Activo,     false),
            Cuenta(5,  "121",  "Facturas, boletas y otros comprobantes por cobrar",ClaseCuenta.Activo,     true),
            Cuenta(6,  "20",   "Mercaderías",                                      ClaseCuenta.Activo,     false),
            Cuenta(7,  "201",  "Mercaderías manufacturadas",                       ClaseCuenta.Activo,     true),
            Cuenta(8,  "40",   "Tributos, contraprestaciones y aportes por pagar", ClaseCuenta.Pasivo,     false),
            Cuenta(9,  "401",  "Gobierno central",                                 ClaseCuenta.Pasivo,     false),
            Cuenta(10, "4011", "Impuesto general a las ventas (IGV)",              ClaseCuenta.Pasivo,     true),
            Cuenta(11, "42",   "Cuentas por pagar comerciales - Terceros",         ClaseCuenta.Pasivo,     false),
            Cuenta(12, "421",  "Facturas, boletas y otros comprobantes por pagar", ClaseCuenta.Pasivo,     true),
            Cuenta(13, "60",   "Compras",                                          ClaseCuenta.Gasto,      false),
            Cuenta(14, "601",  "Mercaderías (compras)",                            ClaseCuenta.Gasto,      true),
            Cuenta(15, "61",   "Variación de existencias",                         ClaseCuenta.Gasto,      false),
            Cuenta(16, "611",  "Mercaderías (variación)",                          ClaseCuenta.Gasto,      true),
            Cuenta(17, "69",   "Costo de ventas",                                  ClaseCuenta.Costo,      false),
            Cuenta(18, "691",  "Mercaderías (costo de ventas)",                    ClaseCuenta.Costo,      true),
            Cuenta(19, "70",   "Ventas",                                           ClaseCuenta.Ingreso,    false),
            Cuenta(20, "701",  "Mercaderías (ventas)",                             ClaseCuenta.Ingreso,    true),
            Cuenta(21, "62",   "Gastos de personal, directores y gerentes",        ClaseCuenta.Gasto,      false),
            Cuenta(22, "621",  "Remuneraciones",                                   ClaseCuenta.Gasto,      true),
            Cuenta(23, "627",  "Seguridad, previsión social y otras contribuciones", ClaseCuenta.Gasto,    true),
            Cuenta(24, "403",  "Instituciones públicas (aportes por pagar)",       ClaseCuenta.Pasivo,     true),
            Cuenta(25, "41",   "Remuneraciones y participaciones por pagar",       ClaseCuenta.Pasivo,     false),
            Cuenta(26, "411",  "Remuneraciones por pagar",                         ClaseCuenta.Pasivo,     true),
            Cuenta(27, "21",   "Productos terminados",                             ClaseCuenta.Activo,     true),
            Cuenta(28, "71",   "Variación de la producción almacenada",            ClaseCuenta.Ingreso,    true)
        );

        // Seed Conceptos de Planilla (base del sistema, porcentajes vigentes referenciales)
        ConceptoPlanilla Concepto(int id, string codigo, string nombre, string tipo, string metodo,
            decimal? pct, decimal? fijo, bool afAfp, bool afEs, int orden) => new()
        {
            Id = id, Codigo = codigo, Nombre = nombre, Tipo = tipo, MetodoCalculo = metodo,
            Porcentaje = pct, MontoFijo = fijo, AfectaAfp = afAfp, AfectaEssalud = afEs,
            EsSistema = true, Orden = orden, IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
        };
        modelBuilder.Entity<ConceptoPlanilla>().HasData(
            Concepto(1, "SUELDO",       "Sueldo básico",              "INGRESO",   "MANUAL",     null,      null,   true,  true,  1),
            Concepto(2, "ASIGFAM",      "Asignación familiar",        "INGRESO",   "FIJO",       null,      102.50m, true, true,  2),
            Concepto(3, "ONP",          "ONP (Sistema Nacional)",     "DESCUENTO", "PORCENTUAL", 0.130000m, null,   false, false, 10),
            Concepto(4, "AFP_FONDO",    "AFP - Aporte al fondo",      "DESCUENTO", "PORCENTUAL", 0.100000m, null,   false, false, 11),
            Concepto(5, "AFP_COMISION", "AFP - Comisión",             "DESCUENTO", "PORCENTUAL", 0.016000m, null,   false, false, 12),
            Concepto(6, "AFP_SEGURO",   "AFP - Prima de seguro",      "DESCUENTO", "PORCENTUAL", 0.013500m, null,   false, false, 13),
            Concepto(7, "ESSALUD",      "EsSalud (aporte empleador)", "APORTE",    "PORCENTUAL", 0.090000m, null,   false, false, 20)
        );

        // Seed Tasas AFP (porcentajes vigentes referenciales; el fondo obligatorio es 10%,
        // la comisión sobre flujo y la prima de seguro varían por administradora).
        TasaAfp Afp(int id, string nombre, decimal comision, decimal seguro) => new()
        {
            Id = id, Nombre = nombre, AporteFondo = 0.100000m, ComisionFlujo = comision, PrimaSeguro = seguro,
            IsActive = true, CreatedAt = new DateTime(2024, 1, 1)
        };
        modelBuilder.Entity<TasaAfp>().HasData(
            Afp(1, "Habitat",   0.014700m, 0.013500m),
            Afp(2, "Integra",   0.015500m, 0.013500m),
            Afp(3, "Prima",     0.016000m, 0.013500m),
            Afp(4, "Profuturo", 0.016900m, 0.013500m)
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AsignarEmpresa();
        var auditEntries = CaptureAuditEntries();
        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0)
        {
            foreach (var entry in auditEntries)
                AuditLogs.Add(entry.ToAuditLog());
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    public override int SaveChanges()
    {
        AsignarEmpresa();
        var auditEntries = CaptureAuditEntries();
        var result = base.SaveChanges();

        if (auditEntries.Count > 0)
        {
            foreach (var entry in auditEntries)
                AuditLogs.Add(entry.ToAuditLog());
            base.SaveChanges();
        }

        return result;
    }

    /// <summary>Asigna la empresa del usuario a los registros nuevos multiempresa.</summary>
    private void AsignarEmpresa()
    {
        if (!_empresaId.HasValue) return;

        foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.EmpresaId = _empresaId.Value;
        }
    }

    /// <summary>Recolecta los cambios auditables antes de guardar (las altas resuelven su Id después).</summary>
    private List<AuditEntry> CaptureAuditEntries()
    {
        ChangeTracker.DetectChanges();
        var entries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not BaseEntity) continue; // AuditLog no hereda de BaseEntity → excluido
            if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            {
                var auditEntry = new AuditEntry(entry, _currentUser?.UserName);
                if (auditEntry.IsMeaningful)
                    entries.Add(auditEntry);
            }
        }

        return entries;
    }
}
