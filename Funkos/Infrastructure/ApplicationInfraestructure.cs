using Funkos.Cache;
using Funkos.Mongo;
using Funkos.Repositories;
using Funkos.Repositories.Categoria;
using Funkos.Repositories.Proveedor;
using Funkos.Service;
using Funkos.Service.Categoria;
using Funkos.Service.Decorator;
using Funkos.Service.Proveedor;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Funkos.Infrastructure;

public static class ApplicationInfraestructure
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // registramos repos, servicios y los Scoped/Singleton/Transient
        services.AddScoped<IFunkoRepository, FunkoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        
        services.AddScoped<FunkoService>();
        services.AddScoped<ProveedorService>();
        services.AddScoped<CategoriaService>();
        
        // Decorator como servicio que recibirá el Controller
        services.AddScoped<IFunkoService>(sp => new CachedFunkoServiceDecorator(sp.GetRequiredService<FunkoService>(), sp.GetRequiredService<ICacheService>(), sp.GetRequiredService<ILogger<CachedFunkoServiceDecorator>>()));
        services.AddScoped<ICategoriaService>(sp => new CachedCategoriaServiceDecorator(sp.GetRequiredService<CategoriaService>(), sp.GetRequiredService<ICacheService>(), sp.GetRequiredService<ILogger<CachedCategoriaServiceDecorator>>()));
        services.AddScoped<IProveedorService>(sp => new CachedProveedorServiceDecorator(sp.GetRequiredService<ProveedorService>(), sp.GetRequiredService<ICacheService>(), sp.GetRequiredService<ILogger<CachedProveedorServiceDecorator>>()));

        
        //MONGO
        // MongoClient es thread-safe, es decir, se reutiliza durante toda la app. MongoDbContext solo tiene referencias a la BD y a las colecciones. No tiene el comportamiento de seguimiento de entidades de un DbContext de EF Core.
        var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()!;
        // Registrar configuración
        services.AddSingleton(settings);
        // Crear una única instancia de MongoClient
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));
        // Registrar contexto
        services.AddSingleton<MongoDbContext>();
        
        //REDIS - CACHE
        //IConnectionMultiplexer ==> Objeto principal de StackExchange.Redis que gestiona las conexiones con el servidor Redis.
        // Desde él se puede obtener una BD Redis
            
        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(
                configuration["Redis:ConnectionString"]!));

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}