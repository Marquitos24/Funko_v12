using Funkos.Models;

namespace Funkos.Repositories;

public interface IFunkoRepository
{
    Task<List<Funko>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<Funko?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Funko> CreateAsync(Funko newFunko, CancellationToken cancellationToken = default);
    Task<Funko?> UpdateAsync(long id, Funko updatedFunko, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken can = default);
    Task<bool> ExistsFunkosCAsync(long id, CancellationToken cancellationToken = default);
    Task<int> ExistsFunkosPAsync(string proveedorId, CancellationToken cancellationToken = default);

}   