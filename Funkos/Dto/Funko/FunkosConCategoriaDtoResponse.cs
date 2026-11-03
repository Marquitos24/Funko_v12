namespace Funkos.Dto.Funko;

public record FunkosConCategoriaDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    long CategoriaId,
    string CategoriaName,
    long ProveedorId,
    int Stock,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);
