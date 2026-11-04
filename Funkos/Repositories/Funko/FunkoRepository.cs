using Funkos.DbContext;
using Funkos.Models;
using Microsoft.EntityFrameworkCore;

namespace Funkos.Repositories;

public class FunkoRepository(AppDbContext db) : IFunkoRepository
{
    #region CRUD
    public async Task<List<Funko>>  FindAllAsync(CancellationToken cancellationToken = default)
    {
        var funkos = await db.Funkos.AsNoTracking().Include(f => f.Categoria).ToListAsync(cancellationToken);
        return funkos;
    }

    public async Task<Funko?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var funko = await db.Funkos.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        return funko;
    }

    public async Task<Funko> CreateAsync(Funko newFunko, CancellationToken cancellationToken = default)
    {
        var funko = new Funko
        {
            Name = newFunko.Name,
            Precio = newFunko.Precio,
            CategoriaId = newFunko.CategoriaId,
            ProveedorId = newFunko.ProveedorId,
            Stock =  newFunko.Stock,
            CreateAt = DateTime.UtcNow
        };
        await db.Funkos.AddAsync(funko);
        await db.SaveChangesAsync(cancellationToken);
        return funko;
    }

    public async Task<Funko?> UpdateAsync(long id, Funko updatedFunko, CancellationToken cancellationToken = default)
    {
        var funko = await db.Funkos.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        if (funko is null) return null;
        
        funko.Name = updatedFunko.Name;
        funko.Precio = updatedFunko.Precio;
        funko.CategoriaId = updatedFunko.CategoriaId;
        funko.ProveedorId = updatedFunko.ProveedorId;
        funko.Stock = updatedFunko.Stock;
        funko.UpdateAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(cancellationToken);
        return funko;
    }
    
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var prov = await db.Funkos.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (prov is null) return false;

        prov.IsDeleted = true;
        prov.DeleteAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
    #endregion

    #region Consultas adicionales
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="can"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken can = default)
    {
        return await db.Funkos.AnyAsync(f => f.Name == name && !f.IsDeleted && (id == null || f.Id != id), can);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> ExistsFunkosCAsync(long id, CancellationToken cancellationToken = default)
    {
        return await db.Funkos.AnyAsync(f => f.CategoriaId == id, cancellationToken);
    }
    
    public async Task<int> ExistsFunkosPAsync(string proveedorId, CancellationToken cancellationToken = default)
    {
        return await db.Funkos.CountAsync(f => f.ProveedorId == proveedorId && !f.IsDeleted, cancellationToken);
    }
    #endregion
    
    
}