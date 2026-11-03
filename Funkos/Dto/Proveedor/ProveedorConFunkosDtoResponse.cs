using Funkos.Dto.Funko;

namespace Funkos.Dto.Proveedor;

public class ProveedorConFunkosDtoResponse(
    long Id,
    string Name,
    string Descripcion,
    ICollection<FunkoParaCategoriaDto> Funkos,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);