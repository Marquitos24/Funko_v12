using Funkos.Dto.Categoria;
using Funkos.Models;

namespace Funkos.Mappers;

public static class CategoriaMapper
{
    public static Categoria ToEntityCreate(this CategoriaCreateDto request) => new Categoria
    {
        Name = request.Name,
        Descripcion = request.Descripcion,
    };
    
    public static Categoria ToEntityUpdate(this CategoriaUpdateDto request)
    {
        var cat = new Categoria
        {
        Name = request.Name,
        Descripcion = request.Descripcion
        };
        
        if (!string.IsNullOrWhiteSpace(request.IdY)) cat.IdY = request.IdY;
        
        return cat;
    }


    public static CategoriaDtoResponse ToDtoResponse(this Categoria cat) => new CategoriaDtoResponse(
        cat.IdY,
        cat.Name, 
        cat.Descripcion,
        cat.CreateAt,
        cat.UpdateAt
    );
    
    public static CategoriaWithFunkosResponseDto ToDtoResponseWithFunkos(this Categoria cat) => new CategoriaWithFunkosResponseDto(
        cat.IdY,
        cat.Name, 
        cat.Descripcion,
        cat.Funkos.Select(f => f.ToDtoParaCategoria()).ToList(),
        cat.CreateAt,
        cat.UpdateAt
    );
}