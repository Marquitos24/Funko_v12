using Funkos.Dto;

namespace Funkos.Helper;

public static class LinkHelper
{
    public static string GenerateLinks(long id, string baseUrl)
    {
        var links = new List<string>
        {
            $"<{baseUrl}>; rel=\"collection\"; method=\"GET\"",
            $"<{baseUrl}/{id}>; rel=\"selfId\"; method=\"GET\"",
            $"<{baseUrl}/{id}>; rel=\"update\"; method=\"PUT\"",
            $"<{baseUrl}/{id}>; rel=\"delete\"; method=\"DELETE\"",
        };
        return string.Join(", ", links);
    }
// Para Update (quizá ya no necesitas el enlace de update, solo ver, borrar y la lista)
    public static string GenerateUpdateLinks(long id, string baseUrl)
    {
        return string.Join(", ", new List<string>
        {
            $"<{baseUrl}/{id}>; rel=\"self\"; method=\"GET\"",
            $"<{baseUrl}/{id}>; rel=\"delete\"; method=\"DELETE\"",
            $"<{baseUrl}>; rel=\"collection\"; method=\"GET\""
        });
    }

    // Para Delete (como ya se ha borrado, el recurso ya no existe; solo puedes volver a la lista o crear uno nuevo)
    public static string GenerateDeleteLinks(long id, string baseUrl)
    {
        return string.Join(", ", new List<string>
        {
            $"<{baseUrl}/{id}>; rel=\"self\"; method=\"GET\"",
            $"<{baseUrl}>; rel=\"collection\"; method=\"GET\"",
            $"<{baseUrl}>; rel=\"create\"; method=\"POST\""
        });
    }
}