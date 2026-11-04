using Funkos.Mongo;
using Funkos.Repositories;
using Funkos.Repositories.Categoria;
using Funkos.Repositories.Proveedor;
using Funkos.Service;
using Funkos.Service.Categoria;
using Funkos.Service.Proveedor;
using MongoDB.Driver;

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
        
        //MONGO
        // MongoClient es thread-safe, es decir, se reutiliza durante toda la app. MongoDbContext solo tiene referencias a la BD y a las colecciones. No tiene el comportamiento de seguimiento de entidades de un DbContext de EF Core.
        var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()!;
        // Registrar configuración
        services.AddSingleton(settings);
        // Crear una única instancia de MongoClient
        services.AddSingleton<IMongoClient>(_ => new MongoDB.Driver.MongoClient(settings.ConnectionString));
        // Registrar contexto
        services.AddSingleton<MongoDbContext>();

        return services;
    }
}