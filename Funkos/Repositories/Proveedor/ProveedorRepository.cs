using Funkos.DbContext;
using Microsoft.EntityFrameworkCore;
using ProveedorModel = Funkos.Models.Proveedor;

namespace Funkos.Repositories.Proveedor;

public class ProveedorRepository(AppDbContext db) : IProveedorRepository
{
    public async Task<List<ProveedorModel>>  FindAllAsync(CancellationToken cancellationToken = default)
    {
        var proveedores = await db.Proveedores.AsNoTracking().ToListAsync(cancellationToken);
        return proveedores;
    }

    public async Task<ProveedorModel?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await db.Proveedores.Include(p => p.Funkos).AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<ProveedorModel> CreateAsync(ProveedorModel newProv, CancellationToken cancellationToken = default)
    {
        var prov = new ProveedorModel
        {
            Name = newProv.Name,
            Descripcion = newProv.Descripcion,
            CreateAt = DateTime.UtcNow
        };
        await db.Proveedores.AddAsync(prov);
        await db.SaveChangesAsync(cancellationToken);
        return prov;
    }

    public async Task<ProveedorModel?> UpdateAsync(long id, ProveedorModel updatedProv, CancellationToken cancellationToken = default)
    {
        var prov = await db.Proveedores.FirstOrDefaultAsync(f => f.Id == id,  cancellationToken);

        if (prov is null) return null;
        
        prov.Name = updatedProv.Name;
        prov.Descripcion = updatedProv.Descripcion;
        prov.UpdateAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(cancellationToken);
        return prov;
    }
/// <summary>
/// 
/// </summary>
/// <param name="id"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await db.Proveedores.AnyAsync(f => f.Id == id, cancellationToken);
    }
    
    public async Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken cancellationToken = default)
    {
        return await db.Proveedores.AnyAsync(f => f.Name == name && !f.IsDeleted && (id == null || f.Id != id) , cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var prov = await db.Proveedores.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (prov is null) return false;

        prov.IsDeleted = true;
        prov.DeleteAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}