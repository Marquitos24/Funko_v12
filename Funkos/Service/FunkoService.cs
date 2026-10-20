using CSharpFunctionalExtensions;
using FluentValidation;
using Funkos.Dto;
using Funkos.Errors;
using Funkos.Mappers;
using Funkos.Models;
using Funkos.Repositories;
using Funkos.Validations;

namespace Funkos.Service;

public class FunkoService(IFunkoRepository repository, IValidator<FunkoCreateDto> _validatorCreate, IValidator<FunkoUpdateDto> _validatorUpdate, ILogger<FunkoService> logger ) : IFunkoService
{
    public Result<List<FunkosDtoResponse>, DomainErrors>  GetAll(string? categoria, decimal? precioMin, decimal? precioMax)
    {
        List<Funko> funkos = repository.GetAll();
       // IEnumerable<FunkoQueryDto> funkosDto = service.GetAll();
       logger.LogInformation("Listar Funkos");
        if (!string.IsNullOrEmpty(categoria) && Enum.TryParse<Categoria>(categoria, true, out var categoriaReal)) 
            funkos = funkos.Where(f => f.Categoria == categoriaReal).OrderByDescending(f => f.Categoria).ToList();
        if (precioMin.HasValue) funkos = funkos.Where(f => f.Precio >= precioMin).OrderByDescending(f => f.Precio).ToList();
        if (precioMax.HasValue) funkos = funkos.Where(f => f.Precio <= precioMax).OrderByDescending(f => f.Precio).ToList();
        
        List<FunkosDtoResponse> funkoResponse = funkos.Select(f => FunkoMapper.ToDtoResponse(f)).ToList();
        return Result.Success<List<FunkosDtoResponse>, DomainErrors>(funkoResponse);
    }
    
    public Result<FunkosDtoResponse, DomainErrors> GetById(long id)
    {
        logger.LogInformation("Buscando Funko {Id}", id);
        if (id <= 0) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound(id));
        Funko? funko = repository.GetById(id); logger.LogWarning("Funko {Id} NO encontrado ", id);
        if(funko == null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound()); logger.LogWarning("Funko {Id} NO encontrado ", id);
       
        FunkosDtoResponse funkoResponse =  FunkoMapper.ToDtoResponse(funko);
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }
    
    public Result<FunkosDtoResponse, DomainErrors> Add(FunkoCreateDto newFunko)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var validationResult = _validatorCreate.Validate(newFunko);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.BadRequest(errorsDescription));
        }
        
        var model = FunkoMapper.ToEntityCreate(newFunko);
        if (model == null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound()); logger.LogWarning("Funko NO creado ");
        
        //Comprobamos que el funko nuevo NO tiene un nombre igual a otro Funko
        List<Funko> listaFunkos = repository.GetAll();
        bool yaExiste = listaFunkos.Any(f => f.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase));
        if (yaExiste) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.BadRequest(model.Name)); logger.LogWarning("Ya existe el funko");
        
        Funko funko = repository.Create(model);
        FunkosDtoResponse funkoResponse = FunkoMapper.ToDtoResponse(funko);
        logger.LogInformation("Funko Creado correctamente");
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }
    
    public Result<FunkosDtoResponse, DomainErrors> Update(long id, FunkoUpdateDto updatedFunko)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var validationResult = _validatorUpdate.Validate(updatedFunko);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.BadRequest(errorsDescription));
        }
        
        //Buscamos el funko por Id y comprobamos si existe, si no salta error
        var funkoId = repository.GetById(id);
        if (id <= 0 || funkoId is null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound(id)); logger.LogWarning("No se encontro el Funko {id}", id);
        
        //Convertimos el RequestDTO al Modelo y validamos, si es null salta error
        var funkoModel = FunkoMapper.ToEntityUpdate(updatedFunko);
        if (funkoModel is null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound()); logger.LogWarning("No guardo el Funko {id}", id);

        //Comprobamos que el funko actualizado NO tiene un nombre igual a otro Funko
        List<Funko> listaFunkos = repository.GetAll();
        bool yaExiste = listaFunkos.Any(f => f.Name == funkoModel.Name && f.Id != id);
        if (yaExiste) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.BadRequest(funkoModel.Name)); logger.LogWarning("Funko {id} con nombre duplicado", id);
        
        //Actualizamos Funko y comprobamos que no sea null, si lo es salta error
        Funko? funko = repository.Update(id, funkoModel);
        if (funko == null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound());
        
        //Transformamos a ResponseDTO
        FunkosDtoResponse funkoResponse = FunkoMapper.ToDtoResponse(funko);
        logger.LogInformation("Funko {id} actualizdo correctamente", id);
        //Devolvemos
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }

    public Result<bool, DomainErrors> Delete(long id)
    {
        //Comprobamos que el Id existe, si no salta error
        var funkoId = repository.GetById(id);
        if (id <= 0 || funkoId is null) return Result.Failure<bool, DomainErrors>(FunkoError.NotFound(id));
        
        var eliminado = repository.Delete(id);
        if (eliminado == false) return Result.Failure<bool, DomainErrors>(FunkoError.NotFound(funkoId.Name));
        logger.LogWarning("Funko {id} eliminado correctamente", id);
        return Result.Success<bool, DomainErrors>(true);
    }
    
}