using Funkos.DbContext;
using Microsoft.EntityFrameworkCore;
using CategoriaModel = Funkos.Models.Categoria;

namespace Funkos.Repositories.Categoria;

public class CategoriaRepository  (AppDbContext db) : ICategoriaRepository
    {
    
        public async Task<List<CategoriaModel>>  FindAllAsync(CancellationToken cancellationToken = default)
        {
            var categorias = await db.Categorias.AsNoTracking().ToListAsync(cancellationToken);
            return categorias;
        }

        public async Task<CategoriaModel?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.Include(c => c.Funkos).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return categoria;
        }

        public async Task<CategoriaModel> CreateAsync(CategoriaModel newC, CancellationToken cancellationToken = default)
        {
            var categoria = new CategoriaModel
            {
                Name = newC.Name,
                Descripcion = newC.Descripcion,
                CreateAt = DateTime.UtcNow
            };
            await db.Categorias.AddAsync(categoria);
            await db.SaveChangesAsync(cancellationToken);
            return categoria;
        }

        public async Task<CategoriaModel?> UpdateAsync(long id, CategoriaModel updatedCategoria, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(f => f.Id == id,  cancellationToken);

            if (categoria is null) return null;
        
            categoria.Name = updatedCategoria.Name;
            categoria.Descripcion = updatedCategoria.Descripcion;
            categoria.UpdateAt = DateTime.UtcNow;
        
            await db.SaveChangesAsync(cancellationToken);
            return categoria;
        }
        public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
        {
            return await db.Categorias.AnyAsync(f => f.Id == id, cancellationToken);
        }
        
        public async Task<bool> ExistsByNameAsync(string name, long? id = null, CancellationToken cancellationToken = default)
        {
            return await db.Categorias.AnyAsync(f => f.Name == name && (id == null || f.Id != id), cancellationToken);
        }

    
        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
            if (categoria is null) return false;

            db.Categorias.Remove(categoria);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
}