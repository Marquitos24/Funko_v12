namespace Funkos.Helper;

public static class CategoriaLinkHelper
{
    public static string GenerateLinks(long id, string baseUrl)
    {
        var links = new List<string>
        {
            $"<{baseUrl}/{id}>; rel=\"Comprobar\"; method=\"GET\"",
            $"<{baseUrl}/{id}>; rel=\"Actualizar\"; method=\"PUT\"",
            $"<{baseUrl}/{id}>; rel=\"Eliminar\"; method=\"DELETE\"",
            $"<{baseUrl}>; rel=\"Lista\"; method=\"GET\""
        };
        return string.Join(", ", links);
    }

// Para Update (quizá ya no necesitas el enlace de update, solo ver, borrar y la lista)
    public static string GenerateUpdateLinks(long id, string baseUrl)
    {
        return string.Join(", ", new List<string>
        {
            $"<{baseUrl}/{id}>; rel=\"Comprobar\"; method=\"GET\"",
            $"<{baseUrl}/{id}>; rel=\"Eliminar\"; method=\"DELETE\"",
            $"<{baseUrl}>; rel=\"Lista\"; method=\"GET\""
        });
    }

    // Para Delete (como ya se ha borrado, el recurso ya no existe; solo puedes volver a la lista o crear uno nuevo)
    public static string GenerateDeleteLinks(long id, string baseUrl)
    {
        return string.Join(", ", new List<string>
        {
            $"<{baseUrl}/{id}>; rel=\"Comprobar\"; method=\"GET\"",
            $"<{baseUrl}>; rel=\"Lista\"; method=\"GET\"",
            $"<{baseUrl}>; rel=\"Crear nuevo\"; method=\"POST\""
        });
    }
}