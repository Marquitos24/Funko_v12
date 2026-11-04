using Funkos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Funkos.Configuration;

public class CategoriaConfiguration: IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categoria");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd(); 
        
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();
        
        builder.Property(p => p.Descripcion).HasMaxLength(300).IsRequired();
        
        builder.HasMany(c => c.Funkos)          // Una categoría tiene muchos Funkos
            .WithOne(f => f.Categoria)           // Un Funko tiene una única Categoria
            .HasForeignKey(f => f.CategoriaId)   // Usando la clave ajena CategoriaId
            .OnDelete(DeleteBehavior.Restrict);         // Evita borrar la categoría si tiene funkos dentro
    }
    
}