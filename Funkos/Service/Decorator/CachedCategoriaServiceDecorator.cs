using CSharpFunctionalExtensions;
using Funkos.Cache;
using Funkos.Dto.Categoria;
using Funkos.Errors;
using Funkos.Service.Categoria;

namespace Funkos.Service.Decorator;

public class CachedCategoriaServiceDecorator(ICategoriaService service, ICacheService cache, ILogger<CachedCategoriaServiceDecorator> logger) : ICategoriaService
{
    public async Task<Result<List<CategoriaDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = "categoria:all";
        var cached = await cache.GetAsync<List<CategoriaDtoResponse>>(cacheKey, cancellationToken);
        if (cached is not null) return Result.Success<List<CategoriaDtoResponse>, DomainErrors>(cached);

        var result = await service.GetAllAsync(cancellationToken);
        if (result.IsSuccess) await cache.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(30), cancellationToken);
        return result;
    }

    
    public async Task<Result<CategoriaWithFunkosResponseDto, DomainErrors>> GetByIdAsync(long id,  CancellationToken cancellationToken)
    {
        var cacheKey = $"categoria:{id}";
        var cached = await cache.GetAsync<CategoriaWithFunkosResponseDto>(cacheKey, cancellationToken);
        if (cached is not null) return Result.Success<CategoriaWithFunkosResponseDto, DomainErrors>(cached);

        var result = await service.GetByIdAsync(id, cancellationToken);
        if (result.IsSuccess) await cache.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(30), cancellationToken);
        
        return result;
    }

    public async Task<Result<CategoriaDtoResponse, DomainErrors>> AddAsync(CategoriaCreateDto newCat, CancellationToken cancellationToken)
    {
        var result = await service.AddAsync(newCat, cancellationToken);
        if (result.IsSuccess)  { logger.LogInformation("CACHE HIT: Registro GetAll Invalidado"); await cache.RemoveAsync("categoria:all", cancellationToken);}
        return result;
    }

    public async Task<Result<CategoriaDtoResponse, DomainErrors>> UpdateAsync(long id, CategoriaUpdateDto updatedCat, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, updatedCat, cancellationToken);
        if (result.IsSuccess)
        {
            //GetAll
            await cache.RemoveAsync("categoria:all", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetAll Invalidado");
            //GetId
            await cache.RemoveAsync($"categoria:{id}", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetById Invalidado");
        }
        return result;
    }

    public async Task<Result<bool, DomainErrors>> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            //GetAll
            await cache.RemoveAsync("categoria:all", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetAll Invalidado");
            //GetId
            await cache.RemoveAsync($"categoria:{id}", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetById Invalidado");
        }
        return result;
    }
}