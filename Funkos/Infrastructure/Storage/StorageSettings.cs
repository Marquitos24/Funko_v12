namespace Funkos.Infrastructure.Storage;

public class StorageSettings
{
    /// <summary>
    /// Ruta base donde se guardan los archivos
    /// </summary>
    public string RootPath { get; set; } = "wwwroot/uploads";
    
    /// <summary>
    /// Tamaño máximo en bytes (10 MB por defecto)
    /// </summary>
    public long MaxFileSize { get; set; } = 5 * 1024 * 1024;
    
    /// <summary>
    /// Extensiones permitidas
    /// </summary>
    public string[] AllowedExtensions { get; set; } = 
        { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    
    /// <summary>
    /// Tipos MIME permitidos
    /// </summary>
    public string[] AllowedContentTypes { get; set; } = 
        { "image/jpeg", "image/png", "image/gif", "image/webp" };
    
    /// <summary>
    /// Subdirectorio para imágenes
    /// </summary>
    public string ImagesFolder { get; set; } = "images";
    
    /// <summary>
    /// Subdirectorio para documentos
    /// </summary>
    public string DocumentsFolder { get; set; } = "documents";
}