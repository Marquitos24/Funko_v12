using Funkos.Models;

namespace Funkos.Dto;

public record FunkosDtoResponse(
    long Id,
    string Name,
    decimal Precio,
    Categoria Categoria,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
