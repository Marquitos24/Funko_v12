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
    
    public static Categoria ToEntityUpdate(this CategoriaUpdateDto request) => new Categoria
    {
        Id = request.Id,
        Name = request.Name,
        Descripcion = request.Descripcion
    };


    public static CategoriaDtoResponse ToDtoResponse(this Categoria cat) => new CategoriaDtoResponse(
        cat.Id,
        cat.Name, 
        cat.Descripcion,
        cat.CreateAt,
        cat.UpdateAt
    );
    
    public static CategoriaWithFunkosResponseDto ToDtoResponseWithFunkos(this Categoria cat) => new CategoriaWithFunkosResponseDto(
        cat.Id,
        cat.Name, 
        cat.Descripcion,
        cat.Funkos.Select(f => f.ToDtoParaCategoria()).ToList(),
        cat.CreateAt,
        cat.UpdateAt
    );
}