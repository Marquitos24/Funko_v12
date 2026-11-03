using CSharpFunctionalExtensions;
using FluentValidation;
using Funkos.Dto.Categoria;
using Funkos.Errors;
using Funkos.Mappers;
using Funkos.Repositories.Categoria;
using Categoriaa = Funkos.Models.Categoria;

namespace Funkos.Service.Categoria;

public class CategoriaService(ICategoriaRepository repository, IValidator<CategoriaCreateDto> validatorCreateAsync, IValidator<CategoriaUpdateDto> validatorUpdateAsync, ILogger<CategoriaService> logger ) : ICategoriaService
{
    public async Task<Result<List<CategoriaDtoResponse>, DomainErrors>>  GetAllAsync(CancellationToken cancellationToken)
    {
        List<Categoriaa> cats = await repository.FindAllAsync(cancellationToken);
       logger.LogInformation("Listar Categorias");
        List<CategoriaDtoResponse> catResponse = cats.Select(f => CategoriaMapper.ToDtoResponse(f)).ToList();
        return Result.Success<List<CategoriaDtoResponse>, DomainErrors>(catResponse);
    }
    
    public async Task<Result<CategoriaWithFunkosResponseDto, DomainErrors>> GetByIdAsync(long id,  CancellationToken cancellationToken)
    {
        logger.LogInformation("Buscando Categoria {Id}", id);
        if (id <= 0) return Result.Failure<CategoriaWithFunkosResponseDto, DomainErrors>(CategoriaError.NotFound(id));
        Categoriaa? cat = await repository.FindByIdAsync(id, cancellationToken); 
        logger.LogWarning("Categoria {Id} NO encontradao ", id);
        if(cat == null) return Result.Failure<CategoriaWithFunkosResponseDto, DomainErrors>(CategoriaError.NotFound()); 
        logger.LogWarning("Categooria {Id} NO encontrada ", id);
       
        CategoriaWithFunkosResponseDto catResponse =  CategoriaMapper.ToDtoResponseWithFunkos(cat);
        return Result.Success<CategoriaWithFunkosResponseDto, DomainErrors>(catResponse);
    }
    
    public async Task<Result<CategoriaDtoResponse, DomainErrors>> AddAsync(CategoriaCreateDto newProv,  CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var validationResult = await validatorCreateAsync.ValidateAsync(newProv);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.BadRequest(errorsDescription));
        }
        
        var model = CategoriaMapper.ToEntityCreate(newProv);
        if (model == null) return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.NotFound()); logger.LogWarning("Categoria NO creada ");
        
        //Comprobamos que el Categoria nuevo NO tiene un nombre igual a otro Funko
        bool yaExiste = await repository.ExistsByNameAsync(newProv.Name, null, cancellationToken);
        if (yaExiste) return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.Duplicate(model.Name)); logger.LogWarning("Ya existe la categoria");
        
        Categoriaa cat = await repository.CreateAsync(model, cancellationToken);
        CategoriaDtoResponse catResponse = CategoriaMapper.ToDtoResponse(cat);
        logger.LogInformation("Categoria Creadoacorrectamente");
        return Result.Success<CategoriaDtoResponse, DomainErrors>(catResponse);
    }
    
    public async Task<Result<CategoriaDtoResponse, DomainErrors>> UpdateAsync(long id, CategoriaUpdateDto updatedCat,  CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var context = new ValidationContext<CategoriaUpdateDto>(updatedCat);
        context.RootContextData["CatId"] = id;
        var validationResult = await validatorUpdateAsync.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.BadRequest(errorsDescription));
        }
        
        //Buscamos el Categoria por Id y comprobamos si existe, si no salta error
        var proveedorId = await repository.FindByIdAsync(id, cancellationToken);
        if (id <= 0 || proveedorId is null) {logger.LogWarning("No se encontro el Proveedor {id}", id); return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.NotFound(id));} 

        
        //Convertimos el RequestDTO al Modelo y validamos, si es null salta error
        var proveedorModel = CategoriaMapper.ToEntityUpdate(updatedCat);
        if (proveedorModel is null) {logger.LogWarning("No guardo el Proveedor {id}", id);  return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.NotFound());} 

        
        //Comprobamos que el Categoria actualizado NO tiene un nombre igual a otro Categoria
        bool yaExiste = await repository.ExistsByNameAsync(updatedCat.Name, id, cancellationToken);
        if (yaExiste) {logger.LogWarning("Proveedor {id} con nombre duplicado", id); return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.Duplicate(proveedorModel.Name));} 
        

        //Actualizamos Categoria y comprobamos que no sea null, si lo es salta error
        Categoriaa? cat = await repository.UpdateAsync(id, proveedorModel,  cancellationToken);
        if (cat == null) return Result.Failure<CategoriaDtoResponse, DomainErrors>(CategoriaError.NotFound());
        
        //Transformamos a ResponseDTO
        CategoriaDtoResponse catResponse = CategoriaMapper.ToDtoResponse(cat);
        logger.LogInformation("Proveedor {id} actualizdo correctamente", id);
        //Devolvemos
        return Result.Success<CategoriaDtoResponse, DomainErrors>(catResponse);
    }

    public async Task<Result<bool, DomainErrors>> DeleteAsync(long id,  CancellationToken cancellationToken)
    {
        //Comprobamos que el Id existe, si no salta error
        var catId = await repository.FindByIdAsync(id, cancellationToken);
        if (id <= 0 || catId is null) return Result.Failure<bool, DomainErrors>(CategoriaError.NotFound(id));
        
        if(catId.Funkos.Any()) return Result.Failure<bool, DomainErrors>(CategoriaError.BusinessRule(catId.Funkos.Count()));
        
        await repository.DeleteAsync(id, cancellationToken);
        
        if (catId != null) return Result.Failure<bool, DomainErrors>(CategoriaError.NotFound(catId.Name));
        logger.LogWarning("Categoria {id} eliminada correctamente", id);
        return Result.Success<bool, DomainErrors>(true);
    }
}