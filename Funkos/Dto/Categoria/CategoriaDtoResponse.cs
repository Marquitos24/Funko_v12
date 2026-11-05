using Funkos.Models;

namespace Funkos.Dto.Categoria;

public record CategoriaDtoResponse(
    string IdY,
    string Name,
    string Descripcion,
    DateTime CreateAt,
    DateTime? UpdateAt
);

    
