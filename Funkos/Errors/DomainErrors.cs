using MongoDB.Bson;

namespace Funkos.Errors;

public abstract record DomainErrors(string Message)
{
    public override string ToString() => $"{GetType().Name}: {Message}";
}

// 404 Not Found
public sealed record NotFoundError(string Message) : DomainErrors(Message)
{
    public static NotFoundError FromId(long id, string resourceType = "Recurso") => new($"{resourceType} con ID {id} no encontrado");
    
    //Metodo Para MONGO y su id
    public static NotFoundError FromId(ObjectId id, string resourceType = "Recurso") => new($"{resourceType} con ID {id.ToString()} no encontrado");
    
    public static NotFoundError ListaVacia(string resourceType = "Recurso") => new($"No se han encontrado {resourceType}");
    
    public static NotFoundError Delete(string recurso) => new($"No se ha podido eliminar el {recurso}");
    
    public static NotFoundError Messagee(string message) => new(message);
    
}
// 400 Bad Request - Validación de datos
public sealed record ValidationError(string Message, Dictionary<string, string[]>? Errors = null) : DomainErrors(Message)
{
    public static ValidationError Create(string message) => new(message);
    public static ValidationError Update(string message) => new(message);

    public static ValidationError WithFieldErrors(Dictionary<string, string[]> errors) => new("Errores de validación", errors);
}

// 409 Conflict
public sealed record ConflictError(string Message) : DomainErrors(Message)
{
    public static ConflictError Duplicate(string resourceType, string value) => new($"Ya existe un {resourceType} con el nombre '{value}'");
    
}

// 403 Forbidden
public sealed record ForbiddenError(string Message) : DomainErrors(Message);

// 422 Unprocessable Entity - Regla de negocio
public sealed record BusinessRuleError(string Message) : DomainErrors(Message)
{
    public static BusinessRuleError DeleteCount(string nombre, int num) => new($"No se puede eliminar {nombre} debido a que esa asociado a {num} Funkos");

}

// 500 Internal Server Error
    public sealed record InternalError(string Message): DomainErrors(Message);
