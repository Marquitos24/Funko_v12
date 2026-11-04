using CSharpFunctionalExtensions;
using Funkos.Cache;
using Funkos.Dto.Proveedor;
using Funkos.Errors;
using Funkos.Service.Proveedor;
using MongoDB.Bson;

namespace Funkos.Service.Decorator;

public class CachedProveedorServiceDecorator(IProveedorService service, ICacheService cache, ILogger<CachedProveedorServiceDecorator> logger) : IProveedorService
{
    public async Task<Result<IEnumerable<ProveedorDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = "proveedor:all";
        var cached = await cache.GetAsync<List<ProveedorDtoResponse>>(cacheKey, cancellationToken);
        if (cached is not null) {logger.LogInformation("CACHE HIT: {CacheKey}", cacheKey); return Result.Success<IEnumerable<ProveedorDtoResponse>, DomainErrors>(cached);}

        var result = await service.GetAllAsync(cancellationToken);
        if (result.IsSuccess) await cache.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(30), cancellationToken);
        return result;
    }

    
    public async Task<Result<ProveedorDtoResponse, DomainErrors>> GetByIdAsync(ObjectId id,  CancellationToken cancellationToken)
    {
        var cacheKey = $"proveedor:{id}";
        var cached = await cache.GetAsync<ProveedorDtoResponse>(cacheKey, cancellationToken);
        if (cached is not null) {logger.LogInformation("CACHE HIT: {CacheKey}", cacheKey); return Result.Success<ProveedorDtoResponse, DomainErrors>(cached);}

        var result = await service.GetByIdAsync(id, cancellationToken);
        if (result.IsSuccess) await cache.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(30), cancellationToken);
        
        return result;
    }

    public async Task<Result<ProveedorDtoResponse, DomainErrors>> AddAsync(ProveedorCreateDto newProv, CancellationToken cancellationToken)
    {
        var result = await service.AddAsync(newProv, cancellationToken);
        if (result.IsSuccess) {logger.LogInformation("CACHE HIT: Registro GetAll Invalidado"); await cache.RemoveAsync("proveedor:all", cancellationToken);}
        return result;
    }

    public async Task<Result<ProveedorDtoResponse, DomainErrors>> UpdateAsync(ObjectId id, ProveedorUpdateDto updatedProv, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, updatedProv, cancellationToken);
        if (result.IsSuccess)
        {
            //GetAll
            await cache.RemoveAsync("proveedor:all", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetAll Invalidado");
            //GetId
            await cache.RemoveAsync($"proveedor:{id}", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetById Invalidado");
        }
        return result;
    }

    public async Task<Result<bool, DomainErrors>> DeleteAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            //GetAll
            await cache.RemoveAsync("proveedor:all", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetAll Invalidado");
            //GetId
            await cache.RemoveAsync($"proveedor:{id}", cancellationToken);
            logger.LogInformation("CACHE HIT: Registro GetById Invalidado");
        }
        return result;
    }
}