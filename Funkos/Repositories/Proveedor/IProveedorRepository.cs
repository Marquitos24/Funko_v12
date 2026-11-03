using ProveedorModel = Funkos.Models.Proveedor;

namespace Funkos.Repositories.Proveedor;

public interface IProveedorRepository
{
    Task<List<ProveedorModel>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<ProveedorModel?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ProveedorModel> CreateAsync(ProveedorModel newCategoria, CancellationToken cancellationToken = default);
    Task<ProveedorModel?> UpdateAsync(long id, ProveedorModel updatedCategoria, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}