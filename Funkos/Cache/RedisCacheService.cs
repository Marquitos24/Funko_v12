using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Funkos.Cache;

//IDistributedCache --> abstracción de .NET para trabajar con sistemas de caché distribuidos
// IOptions<CacheOptions> --> Para acceder a la configuración del caché
public class RedisCacheService(IDistributedCache cache, IOptions<CacheOptions> options, IConnectionMultiplexer multiplexer) : ICacheService
{
    //IOptions<T> = envoltorio para dar acceso a un objeto de configuración.
    // + .Value se obtiene  el objeto CacheOptions.
    private readonly CacheOptions _options = options.Value;

    // Acceso directo a Redis
    private readonly IDatabase _db = multiplexer.GetDatabase();
    
    // Candado por proceso (no distribuido): mitiga el stampede en esta instancia ==> consultas innecesarias a la BD
    // Para bloqueo entre instancias haría falta un lock en Redis (SET NX).
    //SemaphoreSlim ==> mecanismo de sincronización para controlar cuántas tareas pueden entrar simultáneamente en una sección del código (Aqui 1 max).
    private readonly SemaphoreSlim _factoryLock = new(1, 1);

    /// <summary>
    /// Obtener un dato de la cache
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <typeparam name="T">tipo de dato a recuperar</typeparam>
    /// <returns>Devuelve dato almacenado</returns>
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var data = await cache.GetStringAsync(key, ct); //Busca em Redis el valor asociado a la clave
        return data is null ? default : JsonSerializer.Deserialize<T>(data); // Si data es null, devuelve default - Si tiene contenido, se deserializa.
    }

    /// <summary>
    /// Guardar un dato en la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="value">Objeto a almacenar</param>
    /// <param name="expiration">Tiempo durante el que permanecerá almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <typeparam name="T">tipo de dato a recuperar</typeparam>
    /// <returns>No devuelve nada</returns>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        var entryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? _options.DefaultExpiration, //Permite consultar la duración predeterminada de la caché.
            //Si has proporcionado expiration, utiliza ese valor - Si es null, utiliza _options.DefaultExpiration
            SlidingExpiration = _options.SlidingExpiration
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), entryOptions, ct);
        //convierte el objeto C# en JSON y se almacena en Redis. entryOptions == establece las expiraciones
    }

    /// <summary>
    /// Eliminar un dato de la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <returns>No devuelve nada</returns>
    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await cache.RemoveAsync(key, ct);//elimina el dato
    }
    
    /// <summary>
    /// Comprobar si existe un dato en la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <returns>Devuelve true --> Si existe. false --> Si no existe</returns>
    public async Task<bool> ExistsAsync(string key, CancellationToken ct = default)
    {
        var data = await cache.GetStringAsync(key, ct); //cojemos el dato
        return data is not null; // devolvemos si existe es true, si no false
    }

    /// <summary>
    /// Obtener o guardar automáticamente.
    /// Combina GetAsync() y SetAsync() en una única operación.
    /// 1. Busca el elemento en la caché. --> 2. Si existe, lo devuelve directamente. / Si no existe, ejecuta factory para obtener el dato
    /// 3. Guarda el resultado en la caché. --> 4. Devuelve el resultado.
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="factory">Función que al ejecutarse, devuelve una tarea asíncrona con un resultado de tipo T</param>
    /// <param name="expiration">Tiempo durante el que permanecerá almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <typeparam name="T">tipo de dato a recuperar</typeparam>
    /// <returns>Devuelve dato almacenado o nada si se a enviado</returns>
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory,
        TimeSpan? expiration = null, CancellationToken ct = default)
    {
        var cached = await GetAsync<T>(key, ct);// intenta recuperar el dato
        if (cached is not null) return cached; // Si existe, lo devuelve directamente.

        //await _factoryLock.WaitAsync(ct);  //Si no existe el dato, intenta conseguir el permiso del SemaphoreSlim.

        // 1. Intentar ganar el candado: SET clave 1 NX EX 10 (solo uno gana)
        var lockKey = $"lock:{key}";
        var ganado = await _db.StringSetAsync(lockKey, "1",
            TimeSpan.FromSeconds(10), When.NotExists);
        if (!ganado)
        {
            // 2. Otro proceso está cargando: esperar un poco y releer
            await Task.Delay(100, ct);
            return await GetAsync<T>(key, ct);
        }
        
        try
        {
            // Doble comprobación tras esperar el candado
            //volvemos a comprobar por si otra petición ha obtenido el dato
            cached = await GetAsync<T>(key, ct);
            if (cached is not null) return cached;

            var value = await factory();//Si sigue sin existir el dato, ejecuta la función de comprobar la BD
            if (value is not null)
                await SetAsync(key, value, expiration, ct); //Si la consulta ha devuelto un objeto --> lo guarda en Redis.
                                                            // Si ha devuelto null --> NO guarda nada.
            return value;
        }
        finally
        {
            //finally ==> ejecuta tanto si sale bien como si hay una excepción o una cancelación
            _factoryLock.Release();
            //Release() ==> devuelve el permiso al SemaphoreSlim para que otra petición pueda entrar.
            //Sin esto, se podria dejar bloqueadas las demás peticiones.
        }
    }
}