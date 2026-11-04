using Funkos.Models;

namespace Funkos.Dto;

public record FunkosDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    long CategoriaId,
    string ProveedorId,
    int Stock,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
