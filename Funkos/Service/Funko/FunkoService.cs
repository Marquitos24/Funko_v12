using CSharpFunctionalExtensions;
using FluentValidation;
using Funkos.Dto;
using Funkos.Dto.Funko;
using Funkos.Errors;
using Funkos.Mappers;
using Funkos.Models;
using Funkos.Repositories;
using Funkos.Validations;

namespace Funkos.Service;

public class FunkoService(IFunkoRepository repository, IValidator<FunkoCreateDto> _validatorCreateAsync, IValidator<FunkoUpdateDto> _validatorUpdateAsync, ILogger<FunkoService> logger ) : IFunkoService
{
    public async Task<Result<List<FunkosConCategoriaDtoResponse>, DomainErrors>>  GetAllAsync(string? categoria, decimal? precioMin, decimal? precioMax, CancellationToken cancellationToken)
    {
        List<Funko> funkos = await repository.FindAllAsync(cancellationToken);
       // IEnumerable<FunkoQueryDto> funkosDto = service.GetAll();
       
       logger.LogInformation("Listar Funkos");
        if (!string.IsNullOrEmpty(categoria)) 
            funkos = funkos.Where(f => f.Categoria.Name == categoria).OrderByDescending(f => f.Categoria).ToList();
        if (precioMin.HasValue) funkos = funkos.Where(f => f.Precio >= precioMin).OrderByDescending(f => f.Precio).ToList();
        if (precioMax.HasValue) funkos = funkos.Where(f => f.Precio <= precioMax).OrderByDescending(f => f.Precio).ToList();
        
        List<FunkosConCategoriaDtoResponse> funkoResponse = funkos.Select(f => FunkoMapper.ToListDtoResponse(f)).ToList();
        return Result.Success<List<FunkosConCategoriaDtoResponse>, DomainErrors>(funkoResponse);
    }
    
    public async Task<Result<FunkosDtoResponse, DomainErrors>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Buscando Funko {Id}", id);
        if (id <= 0) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound(id));
        Funko? funko = await repository.FindByIdAsync(id, cancellationToken); 
        logger.LogWarning("Funko {Id} NO encontrado ", id);
        if(funko == null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound()); logger.LogWarning("Funko {Id} NO encontrado ", id);
       
        FunkosDtoResponse funkoResponse =  FunkoMapper.ToDtoResponse(funko);
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }
    
    public async Task<Result<FunkosDtoResponse, DomainErrors>> AddAsync(FunkoCreateDto newFunko, CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var validationResult = await _validatorCreateAsync.ValidateAsync(newFunko);
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
        bool yaExiste = await repository.ExistsByNameAsync(newFunko.Name, null, cancellationToken);
        if (yaExiste) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.Duplicate(model.Name)); logger.LogWarning("Ya existe el funko");
        
        Funko funko = await repository.CreateAsync(model, cancellationToken);
        FunkosDtoResponse funkoResponse = FunkoMapper.ToDtoResponse(funko);
        logger.LogInformation("Funko Creado correctamente");
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }
    
    public async Task<Result<FunkosDtoResponse, DomainErrors>> UpdateAsync(long id, FunkoUpdateDto updatedFunko, CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var context = new ValidationContext<FunkoUpdateDto>(updatedFunko);
        context.RootContextData["FunkoId"] = id;
        var validationResult = await _validatorUpdateAsync.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.BadRequest(errorsDescription));
        }
        
        //Buscamos el funko por Id y comprobamos si existe, si no salta error
        var funkoId = await repository.FindByIdAsync(id, cancellationToken);
        if (id <= 0 || funkoId is null) { logger.LogWarning("No se encontro el Funko {id}", id); return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound(id));}
        
        //Convertimos el RequestDTO al Modelo y validamos, si es null salta error
        var funkoModel = FunkoMapper.ToEntityUpdate(updatedFunko);
        if (funkoModel is null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound()); logger.LogWarning("No guardo el Funko {id}", id);
        //Comprobamos que el Categoria actualizado NO tiene un nombre igual a otro Categoria
     
        //Comprobamos que el funko actualizado NO tiene un nombre igual a otro Funko
        //bool yaExiste = await repository.ExistsByNameAsync(funkoModel.Name, id, cancellationToken);
       // if (yaExiste) { logger.LogWarning("Funko {id} con nombre duplicado", id); return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.Duplicate(funkoModel.Name));} 
        
        //Actualizamos Funko y comprobamos que no sea null, si lo es salta error
        Funko? funko = await repository.UpdateAsync(id, funkoModel, cancellationToken);
        if (funko == null) return Result.Failure<FunkosDtoResponse, DomainErrors>(FunkoError.NotFound());
        
        //Transformamos a ResponseDTO
        FunkosDtoResponse funkoResponse = FunkoMapper.ToDtoResponse(funko);
        logger.LogInformation("Funko {id} actualizdo correctamente", id);
        //Devolvemos
        return Result.Success<FunkosDtoResponse, DomainErrors>(funkoResponse);
    }

    public async Task<Result<bool, DomainErrors>> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        //Comprobamos que el Id existe, si no salta error
        var funkoId = await repository.FindByIdAsync(id, cancellationToken);
        if (id <= 0 || funkoId is null) return Result.Failure<bool, DomainErrors>(FunkoError.NotFound(id));
        
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        
        if (!deleted) return Result.Failure<bool, DomainErrors>(FunkoError.NotFound(funkoId.Name));
        logger.LogWarning("Funko {id} eliminado correctamente", id);
        return Result.Success<bool, DomainErrors>(true);
    }
    
}