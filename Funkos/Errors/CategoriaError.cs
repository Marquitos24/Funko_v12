namespace Funkos.Errors;

public class CategoriaError
{
    public static NotFoundError NotFound(long id) => NotFoundError.FromId(id, "Categoria");
    public static NotFoundError NotFound(string message) => NotFoundError.Messagee(message);
    public static NotFoundError NotFound() => NotFoundError.ListaVacia("Categoria");
    public static ValidationError BadRequest(string message) => ValidationError.Create(message);
    public static ConflictError Duplicate(string nombre) => ConflictError.Duplicate("Categoria", nombre);

    public static BusinessRuleError BusinessRule(int num) => BusinessRuleError.DeleteCount("Categoria", num);
}