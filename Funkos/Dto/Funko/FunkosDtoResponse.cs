using Funkos.Models;

namespace Funkos.Dto.Funko;

public record FunkosDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    string? CategoriaId,
    string ProveedorId,
    int Stock,
    string? image,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
