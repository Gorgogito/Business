namespace Business.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Business.Application.Interfaces;
using Business.Domain.Interfaces;
using Business.Persistence.Context;
using Business.Persistence.Repositories;
using Business.Persistence.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICorrelativoService, CorrelativoService>();
        services.AddScoped<ICatalogoEmpresaService, CatalogoEmpresaService>();

        return services;
    }
}
