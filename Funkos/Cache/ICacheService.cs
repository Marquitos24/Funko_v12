namespace Funkos.Cache;

/// <summary>
/// Operaciones a realizar sobre la caché (ya sea con Redis o MemoryCache)
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Obtener un dato de la cache
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <typeparam name="T">tipo de dato a recuperar</typeparam>
    /// <returns>Devuelve dato almacenado</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    
    /// <summary>
    /// Guardar un dato en la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="value">Objeto a almacenar</param>
    /// <param name="expiration">Tiempo durante el que permanecerá almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <typeparam name="T">tipo de dato a recuperar</typeparam>
    /// <returns>No devuelve nada</returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);
    
    /// <summary>
    /// Eliminar un dato de la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <returns>No devuelve nada</returns>
    Task RemoveAsync(string key, CancellationToken ct = default);
    
    /// <summary>
    /// Comprobar si existe un dato en la caché
    /// </summary>
    /// <param name="key">Identificador del elemento almacenado</param>
    /// <param name="ct">Para cancelar la operación</param>
    /// <returns>Devuelve true --> Si existe. false --> Si no existe</returns>
    Task<bool> ExistsAsync(string key, CancellationToken ct = default);
    
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
    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken ct = default);
}