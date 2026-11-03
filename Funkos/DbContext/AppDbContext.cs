using Funkos.Models;
using Microsoft.EntityFrameworkCore;

namespace Funkos.DbContext;

public class AppDbContext (DbContextOptions<AppDbContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Funko> Funkos => Set<Funko>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}