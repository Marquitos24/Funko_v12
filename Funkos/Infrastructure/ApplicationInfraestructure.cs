using Funkos.Repositories;
using Funkos.Repositories.Categoria;
using Funkos.Repositories.Proveedor;
using Funkos.Service;
using Funkos.Service.Categoria;
using Funkos.Service.Proveedor;

namespace Funkos.Infrastructure;

public static class ApplicationInfraestructure
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // registramos repos, servicios y los Scoped/Singleton/Transient
        services.AddScoped<IFunkoRepository, FunkoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        
        services.AddScoped<IFunkoService, FunkoService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<ICategoriaService, CategoriaService>();

        return services;
    }
}