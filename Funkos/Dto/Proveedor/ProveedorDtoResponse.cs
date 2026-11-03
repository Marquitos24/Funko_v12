using Funkos.Models;

namespace Funkos.Dto.Proveedor;

public record ProveedorDtoResponse(
    long Id,
    string Name,
    string Descripcion,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
