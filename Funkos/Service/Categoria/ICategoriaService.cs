using CSharpFunctionalExtensions;
using Funkos.Dto.Categoria;
using Funkos.Errors;

namespace Funkos.Service.Categoria;

public interface ICategoriaService
{
    Task<Result<List<CategoriaDtoResponse>, DomainErrors>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<CategoriaWithFunkosResponseDto, DomainErrors>> GetByIdAsync(long id,  CancellationToken cancellationToken);
    Task<Result<CategoriaDtoResponse, DomainErrors>> AddAsync(CategoriaCreateDto newCat,  CancellationToken cancellationToken);
    Task<Result<CategoriaDtoResponse, DomainErrors>> UpdateAsync(long id, CategoriaUpdateDto updatedCat,  CancellationToken cancellationToken);
    Task<Result<bool, DomainErrors>> DeleteAsync(long id,  CancellationToken cancellationToken);
}