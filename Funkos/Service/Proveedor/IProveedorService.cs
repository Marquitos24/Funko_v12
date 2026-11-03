using CSharpFunctionalExtensions;
using Funkos.Dto.Proveedor;
using Funkos.Errors;

namespace Funkos.Service.Proveedor;

public interface IProveedorService
{
    Task<Result<List<ProveedorDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<ProveedorConFunkosDtoResponse, DomainErrors>> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<Result<ProveedorDtoResponse, DomainErrors>> AddAsync(ProveedorCreateDto newProv, CancellationToken cancellationToken);
    Task<Result<ProveedorDtoResponse, DomainErrors>> UpdateAsync(long id, ProveedorUpdateDto updatedProv, CancellationToken cancellationToken);
    Task<Result<bool, DomainErrors>> DeleteAsync(long id, CancellationToken cancellationToken);
}