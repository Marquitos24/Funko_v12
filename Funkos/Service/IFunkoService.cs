using CSharpFunctionalExtensions;
using Funkos.Dto;
using Funkos.Errors;
using Funkos.Models;

namespace Funkos.Service;

public interface IFunkoService
{
    Result<List<FunkosDtoResponse>, DomainErrors> GetAll(string? categoria, decimal? precioMin, decimal? precioMax);
    Result<FunkosDtoResponse, DomainErrors> GetById(long id);
    Result<FunkosDtoResponse, DomainErrors> Add(FunkoCreateDto newFunko);
    Result<FunkosDtoResponse, DomainErrors> Update(long id, FunkoUpdateDto updatedFunko);
    Result<bool, DomainErrors> Delete(long id);
}