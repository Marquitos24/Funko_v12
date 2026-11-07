namespace Funkos.Errors;

public class CategoriaError
{
    public static NotFoundError NotFound(long id) => NotFoundError.FromId(id, "Categoria");
    public static NotFoundError NotFound(string message) => NotFoundError.Messagee(message);
    public static NotFoundError NotFound() => NotFoundError.ListaVacia("Categoria");
    public static ValidationError BadRequest(string message) => ValidationError.Create(message);
    public static ConflictError Duplicate(string nombre) => ConflictError.Duplicate("Categoria", nombre);
    public static ConflictError Concurrency(string model, string id, string accion) => ConflictError.Concurrency(model, id, accion);

    public static BusinessRuleError BusinessRule(int num) => BusinessRuleError.DeleteCount("Categoria", num);
}