using MongoDB.Bson;

namespace Funkos.Errors;

public class ProveedorError
{
    public static NotFoundError NotFound(ObjectId id) => NotFoundError.FromId(id, "Proveedor");
    public static NotFoundError NotFound() => NotFoundError.ListaVacia("Proveedor");
    public static NotFoundError NotFound(string message) => NotFoundError.Messagee(message);

    public static ValidationError BadRequest(string message) => ValidationError.Create(message);
    public static ConflictError Duplicate(string nombre) => ConflictError.Duplicate("Proveedor", nombre);
    public static ConflictError Concurrency(string model, string id, string accion) => ConflictError.Concurrency(model, id, accion);

    public static BusinessRuleError BusinessRule(int num) => BusinessRuleError.DeleteCount("Proveedor", num);

}