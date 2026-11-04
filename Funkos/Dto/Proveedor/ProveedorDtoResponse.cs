using Funkos.Models;
using MongoDB.Bson;

namespace Funkos.Dto.Proveedor;

public record ProveedorDtoResponse(
    string Id,
    string Name,
    string Descripcion,
    DateTime CreateAt,
    DateTime? UpdateAt,
    DateTime? DeleteAt
);

    
