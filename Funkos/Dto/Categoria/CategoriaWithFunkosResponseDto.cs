using Funkos.Dto.Funko;

namespace Funkos.Dto.Categoria;

public record CategoriaWithFunkosResponseDto(
    string IdY,
    string Name,
    string Descripcion,
    ICollection<FunkoParaCategoriaDto> Funkos,
    DateTime CreateAt,
    DateTime? UpdateAt
    );