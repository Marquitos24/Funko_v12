using CSharpFunctionalExtensions;
using Funkos.Dto.Proveedor;
using Funkos.Errors;
using MongoDB.Bson;

namespace Funkos.Service.Proveedor;

public interface IProveedorService
{
    Task<Result<IEnumerable<ProveedorDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<ProveedorDtoResponse, DomainErrors>> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);
    Task<Result<ProveedorDtoResponse, DomainErrors>> AddAsync(ProveedorCreateDto newProv, CancellationToken cancellationToken);
    Task<Result<ProveedorDtoResponse, DomainErrors>> UpdateAsync(ObjectId id, ProveedorUpdateDto updatedProv, CancellationToken cancellationToken);
    Task<Result<bool, DomainErrors>> DeleteAsync(ObjectId id, CancellationToken cancellationToken);
}