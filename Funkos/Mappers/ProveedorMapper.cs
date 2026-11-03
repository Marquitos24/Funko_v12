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
        Id = request.Id,
        Name = request.Name,
        Descripcion = request.Descripcion
    };
    
    public static ProveedorDtoResponse ToDtoResponse(this Proveedor prov) => new ProveedorDtoResponse(
        prov.Id,
        prov.Name, 
        prov.Descripcion,
        prov.CreateAt,
        prov.UpdateAt,
        prov.DeleteAt
    );
    public static ProveedorConFunkosDtoResponse ToDtoResponseWithFunkos(this Proveedor prov) => new ProveedorConFunkosDtoResponse(
        prov.Id,
        prov.Name, 
        prov.Descripcion,
        prov.Funkos.Select(f => f.ToDtoParaCategoria()).ToList(),
        prov.CreateAt,
        prov.UpdateAt,
        prov.DeleteAt
    );
}