using Funkos.Models;

namespace Funkos.Dto.Categoria;

public record CategoriaDtoResponse(
    long Id,
    string Name,
    string Descripcion,
    DateTime CreateAt,
    DateTime? UpdateAt
);

    
