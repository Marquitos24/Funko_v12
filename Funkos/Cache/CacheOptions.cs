namespace Funkos.Cache;

public class CacheOptions
{
    public const string SectionName = "Cache";

    /// <summary>
    /// Tiempo de vida por defecto de las entradas en caché (en minutos).
    /// </summary>
    public int DefaultExpirationMinutes { get; set; } = 30;

    /// <summary>
    /// Tiempo de expiración deslizante (en minutos) ==> Cada vez que se accede al dato, se renueva su tiempo de expiración.
    /// Se renueva en cada acceso.
    /// </summary>
    public int SlidingExpirationMinutes { get; set; } = 10;

    public TimeSpan DefaultExpiration => TimeSpan.FromMinutes(DefaultExpirationMinutes);
    public TimeSpan SlidingExpiration => TimeSpan.FromMinutes(SlidingExpirationMinutes);
}