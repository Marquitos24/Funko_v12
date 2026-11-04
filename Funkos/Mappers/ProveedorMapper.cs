using Funkos.Dto.Proveedor;
using Funkos.Models;

namespace Funkos.Mappers;

public static class ProveedorMapper
{
    public static Proveedor ToEntityCreate(this ProveedorCreateDto request) => new Proveedor
    {
        Name = request.Name,
        Descripcion = request.Descripcion,
    };
    
    public static Proveedor ToEntityUpdate(this ProveedorUpdateDto request) => new Proveedor
    {
        Name = request.Name,
        Descripcion = request.Descripcion
    };
    
    public static ProveedorDtoResponse ToDtoResponse(this Proveedor prov) => new ProveedorDtoResponse(
        prov.Id.ToString(),
        prov.Name, 
        prov.Descripcion,
        prov.CreateAt,
        prov.UpdateAt,
        prov.DeleteAt
    );
}