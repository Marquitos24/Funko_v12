namespace Funkos.Dto.Common;

public record PagedResult<T>
{
    /// <summary>
    /// Colección de Elementos de la página solicitada según los parámetros de paginación.
    /// </summary>
    /// <remarks>
    /// Propiedad == NUNCA  null --> Si no hay elementos, devuelve una colección vacía.
    /// Nº elementos coincide con PageSize (excepto en la última página).
    /// </remarks>
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();

    /// <summary>
    /// Cantidad total de registros que coinciden con los filtros aplicados, sin considerar la paginación.
    /// </summary>
    /// <remarks>
    /// Utilizado para:
    /// - Calcular número total de páginas
    /// - Mostrar información del tipo "Mostrando 1-20 de 150 elementos"
    /// - Determinar si hay más datos disponibles
    /// </remarks>
    public int TotalCount { get; init; }

    /// <summary>
    /// Nº de la página devuelta, comenzando desde 1 (primera página de los resultados)
    /// </summary>
    /// <remarks>
    /// Comportamiento según valor:
    /// - Page = 1: Primera página (siempre tiene HasPreviousPage = false)
    /// - Page > 1: Páginas intermedias o finales
    /// - Page > TotalPages: Devuelve página vacía
    /// </remarks>
    public int Page { get; init; }

    /// <summary>
    /// Número máximo de elementos que hay en una página. (Configurable por el cliente según sus necesidades)
    /// </summary>
    /// <remarks>
    /// Valores típicos:
    /// - 10: Vistas detalladas con mucha información
    /// - 20: Balance entre información y rendimiento
    /// - 50-100: Listados densos o exports
    ///
    /// Límites recomendados:
    /// - Mínimo: 1 elemento
    /// - Máximo: 100 elementos (para evitar problemas de rendimiento)
    /// </remarks>
    public int PageSize { get; init; }

    /// <summary>
    /// Número total de páginas que existen para los resultados ==> Techo de TotalCount / PageSize.
    /// </summary>
    /// <remarks>
    /// Fórmula: Math.Ceiling(TotalCount / PageSize)
    /// Math.Ceiling ==> Redondear numero hacia arriva
    /// Casos especiales:
    /// - PageSize = 0: Retorna 0 para evitar división por cero
    /// - TotalCount = 0: Retorna 0 (no hay páginas)
    /// </remarks>
    /// <example>
    /// TotalCount = 150, PageSize = 20 → 150/20 = 7.5 ceil(7.5) = 8 ==> TotalPages = 8
    /// </example>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>
    /// Indica SI hay una página siguiente.
    /// True cuando Page es menor que TotalPages.
    /// </summary>
    /// <remarks>
    /// Uso típico en UI:
    /// - Habilitar botón "Siguiente" cuando HasNextPage = true
    /// - Deshabilitar cuando HasNextPage = false
    /// - Mostrar indicador visual "Página X de Y"
    /// </remarks>
    /// <example>
    /// Page = 2, TotalPages = 8 → HasNextPage = true
    /// Page = 8, TotalPages = 8 → HasNextPage = false
    /// </example>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Indica si hay una página anterior.
    /// True cuando Page es mayor que 1.
    /// </summary>
    /// <remarks>
    /// Uso típico en UI:
    /// - Habilitar botón "Anterior" cuando HasPreviousPage = true
    /// - Deshabilitar cuando HasPreviousPage = false
    /// - Ocultar completamente el botón en primera página
    /// </remarks>
    /// <example>
    /// Page = 1, TotalPages = 8 → HasPreviousPage = false
    /// Page = 2, TotalPages = 8 → HasPreviousPage = true
    /// </example>
    public bool HasPreviousPage => Page > 1;
}