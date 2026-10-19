using Funkos.Dto;
using Funkos.Models;

namespace Funkos.Mappers;

public static class FunkoMapper
{
    public static Funko ToEntityCreate(this FunkoCreateDto request) => new Funko
    {
        Name = request.Name,
        Precio = request.Precio,
        Categoria = request.Categoria
    };
    
    public static Funko ToEntityUpdate(this FunkoUpdateDto request) => new Funko
    {
        Id = request.Id,
        Name = request.Name,
        Precio = request.Precio,
        Categoria = request.Categoria
    };


    public static FunkosDtoResponse ToDtoResponse(this Funko funko) => new FunkosDtoResponse(

        funko.Id,
        funko.Name,
        funko.Precio,
        funko.Categoria,
        funko.CreateAt,
        funko.UpdateAt,
        funko.DeleteAt
    );
}