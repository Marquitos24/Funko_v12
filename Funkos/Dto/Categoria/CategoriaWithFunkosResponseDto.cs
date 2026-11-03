using Funkos.Dto.Funko;

namespace Funkos.Dto.Categoria;

public record CategoriaWithFunkosResponseDto(
    long Id,
    string Name,
    string Descripcion,
    ICollection<FunkoParaCategoriaDto> Funkos,
    DateTime CreateAt,
    DateTime? UpdateAt
    );