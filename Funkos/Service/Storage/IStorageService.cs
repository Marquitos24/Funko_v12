using CSharpFunctionalExtensions;
using Funkos.Errors;

namespace Funkos.Service.Storage;

public interface IStorageService
{
    /// <summary>
    /// Inicializa el almacenamiento
    /// </summary>
    Task InitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Almacena un archivo y devuelve el nombre generado
    /// </summary>
    Task<Result<string, DomainErrors>> StoreAsync(IFormFile file, string? folder = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Almacena un archivo desde un stream
    /// </summary>
    Task<Result<string, DomainErrors>> StoreAsync(Stream stream, string fileName, string? folder = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Carga un archivo como stream
    /// </summary>
    Task<Result<Stream, DomainErrors>> LoadAsStreamAsync(string fileName, string? folder = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la ruta completa del archivo
    /// </summary>
    string GetFilePath(string fileName, string? folder = null);

    /// <summary>
    /// Obtiene la URL pública del archivo
    /// </summary>
    string GetUrl(string fileName, string? folder = null);

    /// <summary>
    /// Verifica si un archivo existe
    /// </summary>
    bool Exists(string fileName, string? folder = null);

    /// <summary>
    /// Elimina un archivo
    /// </summary>
    Task<bool> DeleteAsync(string fileName, string? folder = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista todos los archivos de una carpeta
    /// </summary>
    Task<IEnumerable<string>> ListFilesAsync(string? folder = null);
}