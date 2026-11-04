using MongoDB.Bson;
using MongoDB.Driver;
using ProveedorModel = Funkos.Models.Proveedor;

namespace Funkos.Repositories.Proveedor;

public interface IProveedorRepository
{
    Task<IEnumerable<ProveedorModel>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<ProveedorModel?> FindByIdAsync(ObjectId id, CancellationToken cancellationToken = default);
    Task<ProveedorModel> CreateAsync(ProveedorModel newCategoria, CancellationToken cancellationToken = default);
    Task<ProveedorModel?> UpdateAsync(ObjectId id, ProveedorModel updatedCategoria, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, ObjectId? id = null, CancellationToken cancellationToken = default);
}