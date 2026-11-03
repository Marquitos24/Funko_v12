namespace Funkos.Errors;

public class ProveedorError
{
    public static NotFoundError NotFound(long id) => NotFoundError.FromId(id, "Proveedor");
    public static NotFoundError NotFound() => NotFoundError.ListaVacia("Proveedor");
    public static NotFoundError NotFound(string nombre) => NotFoundError.Delete(nombre, "Proveedor");
    public static ValidationError BadRequest(string message) => ValidationError.Create(message);
    public static ConflictError Duplicate(string nombre) => ConflictError.Duplicate("Proveedor", nombre);
    
    public static BusinessRuleError BusinessRule(int num) => BusinessRuleError.DeleteCount("Proveedor", num);

}