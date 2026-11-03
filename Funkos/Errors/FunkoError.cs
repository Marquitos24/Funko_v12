namespace Funkos.Errors;

public static class FunkoError
{
    public static NotFoundError NotFound(long id) => NotFoundError.FromId(id, "Funko");
    public static NotFoundError NotFound() => NotFoundError.ListaVacia("Funkos");
    public static NotFoundError NotFound(string nombre) => NotFoundError.Delete(nombre, "Funko");
    public static ValidationError BadRequest(string message) => ValidationError.Create(message);
    public static ConflictError Duplicate(string nombre) => ConflictError.Duplicate("Funko", nombre);
}