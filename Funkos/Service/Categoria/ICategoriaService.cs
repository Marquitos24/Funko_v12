using CSharpFunctionalExtensions;
using Funkos.Dto.Categoria;
using Funkos.Errors;

namespace Funkos.Service.Categoria;

public interface ICategoriaService
{
    Task<Result<List<CategoriaDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<CategoriaWithFunkosResponseDto, DomainErrors>> GetByIdAsync(string id,  CancellationToken cancellationToken);
    Task<Result<CategoriaDtoResponse, DomainErrors>> AddAsync(CategoriaCreateDto newCat,  CancellationToken cancellationToken);
    Task<Result<CategoriaDtoResponse, DomainErrors>> UpdateAsync(string id, CategoriaUpdateDto updatedCat,  CancellationToken cancellationToken);
    Task<Result<bool, DomainErrors>> DeleteAsync(string id,  CancellationToken cancellationToken);
}