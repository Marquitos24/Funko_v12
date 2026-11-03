using CSharpFunctionalExtensions;
using FluentValidation;
using Funkos.Dto.Proveedor;
using Funkos.Errors;
using Funkos.Mappers;
using Proveedorr = Funkos.Models.Proveedor;
using Funkos.Repositories.Proveedor;

namespace Funkos.Service.Proveedor;

public class ProveedorService(IProveedorRepository repository, IValidator<ProveedorCreateDto> validatorCreateAsync, IValidator<ProveedorUpdateDto> validatorUpdateAsync, ILogger<ProveedorService> logger ) : IProveedorService
{
    public async Task<Result<List<ProveedorDtoResponse>, DomainErrors>>  GetAllAsync(CancellationToken cancellationToken)
    {
        List<Proveedorr> proveedores = await repository.FindAllAsync(cancellationToken);
       logger.LogInformation("Listar Proveedores");
        List<ProveedorDtoResponse> proveedorResponse = proveedores.Select(f => ProveedorMapper.ToDtoResponse(f)).ToList();
        return Result.Success<List<ProveedorDtoResponse>, DomainErrors>(proveedorResponse);
    }
    
    public async Task<Result<ProveedorConFunkosDtoResponse, DomainErrors>> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Buscando Proveedor {Id}", id);
        if (id <= 0) return Result.Failure<ProveedorConFunkosDtoResponse, DomainErrors>(ProveedorError.NotFound(id));
        Proveedorr? proveedor = await repository.FindByIdAsync(id, cancellationToken); 
        logger.LogWarning("Proveedor {Id} NO encontrado ", id);
        if(proveedor == null) {logger.LogWarning("Proveedor {Id} NO encontrado ", id); return Result.Failure<ProveedorConFunkosDtoResponse, DomainErrors>(ProveedorError.NotFound()); }
       
        ProveedorConFunkosDtoResponse proveedorResponse =  ProveedorMapper.ToDtoResponseWithFunkos(proveedor);
        return Result.Success<ProveedorConFunkosDtoResponse, DomainErrors>(proveedorResponse);
    }
    
    public async Task<Result<ProveedorDtoResponse, DomainErrors>> AddAsync(ProveedorCreateDto newProv, CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var validationResult = await validatorCreateAsync.ValidateAsync(newProv);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.BadRequest(errorsDescription));
        }
        
        var model = ProveedorMapper.ToEntityCreate(newProv);
        if (model == null)
        {
            logger.LogWarning("Proveedor NO creado ");
            return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.NotFound()); 
        }
        
        //Comprobamos que el Proveedor nuevo NO tiene un nombre igual a otro Proveedor
        bool yaExiste = await repository.ExistsByNameAsync(newProv.Name, null, cancellationToken);
        if (yaExiste)
        {
            logger.LogWarning("Ya existe el Proveedor");
            return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.Duplicate(model.Name)); 
        }
        Proveedorr proveedor = await repository.CreateAsync(model, cancellationToken);
        ProveedorDtoResponse provResponse = ProveedorMapper.ToDtoResponse(proveedor);
        logger.LogInformation("Proveedor Creado correctamente");
        return Result.Success<ProveedorDtoResponse, DomainErrors>(provResponse);
    }
    
    public async Task<Result<ProveedorDtoResponse, DomainErrors>> UpdateAsync(long id, ProveedorUpdateDto updatedProveedor, CancellationToken cancellationToken)
    {
        // Si la validación falla, ModelState.IsValid será false automáticamente
        var context = new ValidationContext<ProveedorUpdateDto>(updatedProveedor);
        context.RootContextData["ProvId"] = id;
        var validationResult = await validatorUpdateAsync.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Hay errores de validación");
            // Si falla, mapeamos los errores de FluentValidation a tu sistema de DomainErrors
            var errorsDescription = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.BadRequest(errorsDescription));
        }
        
        //Buscamos el Proveedor por Id y comprobamos si existe, si no salta error
        var proveedorId = await repository.FindByIdAsync(id,cancellationToken );
        if (id <= 0 || proveedorId is null)
        {
            logger.LogWarning("No se encontro el Proveedor {id}", id);
            return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.NotFound(id)); 
        }
        
        //Convertimos el RequestDTO al Modelo y validamos, si es null salta error
        var proveedorModel = ProveedorMapper.ToEntityUpdate(updatedProveedor);
        if (proveedorModel is null) {logger.LogWarning("No guardo el Proveedor {id}", id);  return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.NotFound());} 
        
        //Actualizamos Proveedor y comprobamos que no sea null, si lo es salta error
        Proveedorr? proveedorr = await repository.UpdateAsync(id, proveedorModel, cancellationToken);
        if (proveedorr == null) return Result.Failure<ProveedorDtoResponse, DomainErrors>(ProveedorError.NotFound());
        
        //Transformamos a ResponseDTO
        ProveedorDtoResponse provResponse = ProveedorMapper.ToDtoResponse(proveedorr);
        logger.LogInformation("Proveedor {id} actualizdo correctamente", id);
        //Devolvemos
        return Result.Success<ProveedorDtoResponse, DomainErrors>(provResponse);
    }

    public async Task<Result<bool, DomainErrors>> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        //Comprobamos que el Id existe, si no salta error
        var proveedorId = await repository.FindByIdAsync(id, cancellationToken);
        if (id <= 0 || proveedorId is null) return Result.Failure<bool, DomainErrors>(ProveedorError.NotFound(id));
        
        //Que no haya funkos asociados al proveedor
        if(proveedorId.Funkos.Any())
        { 
            logger.LogInformation("Proveedor {id} tiene funkos asociados", id); 
            return Result.Failure<bool, DomainErrors>(ProveedorError.BusinessRule(proveedorId.Funkos.Count()));
        }
        
        var deleted = await repository.DeleteAsync(id, cancellationToken);

        if (!deleted) return Result.Failure<bool, DomainErrors>(ProveedorError.NotFound(proveedorId.Name));
        logger.LogInformation("Proveedor {id} eliminado correctamente", id);
        return Result.Success<bool, DomainErrors>(true);
    }
}