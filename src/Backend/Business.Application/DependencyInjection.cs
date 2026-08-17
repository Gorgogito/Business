namespace Business.Application;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Business.Application.Interfaces;
using Business.Application.Services;
using System.Reflection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<IParametroService, ParametroService>();
        services.AddScoped<ISucursalService, SucursalService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IAlmacenService, AlmacenService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IInventarioService, InventarioService>();
        services.AddScoped<IMovimientoInventarioService, MovimientoInventarioService>();
        services.AddScoped<ICotizacionService, CotizacionService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IFacturaService, FacturaService>();
        services.AddScoped<INotaVentaService, NotaVentaService>();
        services.AddScoped<IGuiaRemisionService, GuiaRemisionService>();
        services.AddScoped<IOrdenCompraService, OrdenCompraService>();
        services.AddScoped<IRecepcionCompraService, RecepcionCompraService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAnaliticaService, AnaliticaService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<ICuentaPorCobrarService, CuentaPorCobrarService>();
        services.AddScoped<ICuentaPorPagarService, CuentaPorPagarService>();
        services.AddScoped<IReporteService, ReporteService>();
        services.AddScoped<ICuentaContableService, CuentaContableService>();
        services.AddScoped<IAsientoContableService, AsientoContableService>();
        services.AddScoped<IConfiguracionContableService, ConfiguracionContableService>();
        services.AddScoped<ILibroContableService, LibroContableService>();
        services.AddScoped<IEstadoFinancieroService, EstadoFinancieroService>();
        services.AddScoped<ITrabajadorService, TrabajadorService>();
        services.AddScoped<IConceptoPlanillaService, ConceptoPlanillaService>();
        services.AddScoped<ITasaAfpService, TasaAfpService>();
        services.AddScoped<IPlanillaService, PlanillaService>();
        services.AddScoped<IBeneficioSocialService, BeneficioSocialService>();
        services.AddScoped<IRecetaService, RecetaService>();
        services.AddScoped<IOrdenFabricacionService, OrdenFabricacionService>();

        return services;
    }
}
