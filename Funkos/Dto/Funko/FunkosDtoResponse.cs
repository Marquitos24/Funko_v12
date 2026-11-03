using Funkos.Models;

namespace Funkos.Dto;

public record FunkosDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    long CategoriaId,
    long ProveedorId,
    int Stock,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
