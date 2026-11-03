using CategoriaModel = Funkos.Models.Categoria;

namespace Funkos.Repositories.Categoria;

public interface ICategoriaRepository
{
    Task<List<CategoriaModel>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<CategoriaModel?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<CategoriaModel> CreateAsync(CategoriaModel newCategoria, CancellationToken cancellationToken = default);
    Task<CategoriaModel?> UpdateAsync(long id, CategoriaModel updatedCategoria, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}