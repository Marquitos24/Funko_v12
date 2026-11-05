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

        public async Task<CategoriaModel?> FindByIdAsync(string idY, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.Include(c => c.Funkos).AsNoTracking().FirstOrDefaultAsync(c => c.IdY == idY, cancellationToken);
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

        public async Task<CategoriaModel?> UpdateAsync(string idY, CategoriaModel updatedCategoria, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(f => f.IdY == idY,  cancellationToken);

            if (categoria is null) return null;
        
            categoria.Name = updatedCategoria.Name;
            categoria.Descripcion = updatedCategoria.Descripcion;
            categoria.UpdateAt = DateTime.UtcNow;
        
            await db.SaveChangesAsync(cancellationToken);
            return categoria;
        }
        public async Task<bool> ExistsAsync(string idY, CancellationToken cancellationToken = default)
        {
            return await db.Categorias.AnyAsync(f => f.IdY == idY, cancellationToken);
        }
        
        public async Task<bool> ExistsByNameAsync(string name, string? idY = null, CancellationToken cancellationToken = default)
        {
            return await db.Categorias.AnyAsync(f => f.Name == name && (idY == null || f.IdY != idY), cancellationToken);
        }

    
        public async Task<bool> DeleteAsync(string idY, CancellationToken cancellationToken = default)
        {
            var categoria = await db.Categorias.FirstOrDefaultAsync(f => f.IdY == idY, cancellationToken);
            if (categoria is null) return false;

            db.Categorias.Remove(categoria);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
}