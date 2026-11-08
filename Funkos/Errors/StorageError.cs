namespace Funkos.Errors;

public class StorageError
{
    public static ValidationError BadRequest(string message) => ValidationError.Create(message);

}