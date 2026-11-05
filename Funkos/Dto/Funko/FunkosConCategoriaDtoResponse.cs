namespace Funkos.Dto.Funko;

public record FunkosConCategoriaDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    string? CategoriaId,
    string CategoriaName,
    string ProveedorId,
    int Stock,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);
