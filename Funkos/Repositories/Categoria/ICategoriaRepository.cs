using CategoriaModel = Funkos.Models.Categoria;

namespace Funkos.Repositories.Categoria;

public interface ICategoriaRepository
{
    Task<List<CategoriaModel>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<CategoriaModel?> FindByIdAsync(string idY, CancellationToken cancellationToken = default);
    Task<CategoriaModel> CreateAsync(CategoriaModel newCategoria, CancellationToken cancellationToken = default);
    Task<CategoriaModel?> UpdateAsync(string idY, CategoriaModel updatedCategoria, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string idY, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, string? idY = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string idY, CancellationToken cancellationToken = default);
}