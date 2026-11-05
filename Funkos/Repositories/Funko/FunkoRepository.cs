using System.Linq.Expressions;
using Funkos.DbContext;
using Funkos.Dto.Funko;
using Funkos.Models;
using Microsoft.EntityFrameworkCore;

namespace Funkos.Repositories;

public class FunkoRepository(AppDbContext db) : IFunkoRepository
{
    #region CRUD
    public async Task<List<Funko>>  FindAllAsync(CancellationToken cancellationToken = default)
    {
        var funkos = await db.Funkos.AsNoTracking().Include(f => f.Categoria).OrderBy(f => f.Name).ToListAsync(cancellationToken);
        return funkos;
    }

    public async Task<(List<Funko> Items, int TotalCount)> FindAllPagedAsync(FunkoFilterDto dto, CancellationToken cancellationToken = default)
    {
        var query = dto.IsDeleted.HasValue
            ? db.Funkos.IgnoreQueryFilters().Include(f => f.Categoria).AsQueryable()
            : db.Funkos.Include(f => f.Categoria).AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(f => EF.Functions.Like(f.Name, $"%{dto.Name}%"));

        if (!string.IsNullOrWhiteSpace(dto.Categoria))
            query = query.Where(p => EF.Functions.Like(p.Categoria.Name, $"%{dto.Categoria}%"));

        if (dto.IsDeleted.HasValue)
            query = query.Where(p => p.IsDeleted == dto.IsDeleted.Value);

        if (dto.PrecioMax.HasValue)
            query = query.Where(p => p.Precio <= dto.PrecioMax.Value);
        
        if (dto.PrecioMin.HasValue)
            query = query.Where(p => p.Precio >= dto.PrecioMin.Value);

        if (dto.StockMin.HasValue)
            query = query.Where(p => p.Stock >= dto.StockMin.Value);

        var totalCount = await query.CountAsync();
        query = ApplySorting(query, dto.SortBy, dto.Direction);
        
        var size = Math.Clamp(dto.Size, 1, 100);
        var page = Math.Max(dto.Page, 0);

        var items = await query
            .AsNoTracking().Skip(page * size).Take(size).ToListAsync(cancellationToken);

        return (items, totalCount);
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
    public async Task<bool> ExistsFunkosCAsync(string id, CancellationToken cancellationToken = default)
    {
        return await db.Funkos.AnyAsync(f => f.CategoriaId == id, cancellationToken);
    }
    
    public async Task<int> ExistsFunkosPAsync(string proveedorId, CancellationToken cancellationToken = default)
    {
        return await db.Funkos.CountAsync(f => f.ProveedorId == proveedorId && !f.IsDeleted, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <param name="sortBy"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private static IQueryable<Funko> ApplySorting(IQueryable<Funko> query, string sortBy, string direction)
    {
        var isDescending = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
        Expression<Func<Funko, object>> keySelector = sortBy.ToLower() switch
        {
            "nombre" => f => f.Name,
            "precio" => f => f.Precio,
            "stock" => f => f.Stock,
            "createdat" => f => f.CreateAt,
            "categoria" => f => f.Categoria.Name,
            _ => f => f.Id
        };
        return isDescending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }

    #endregion
    
    
    
}