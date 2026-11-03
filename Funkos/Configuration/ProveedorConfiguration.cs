using Funkos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Funkos.Configuration;

public class ProveedorConfiguration: IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("Proveedor");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd(); 
        
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(p => p.Name).IsUnique();
        
        builder.Property(p => p.Descripcion).HasMaxLength(300).IsRequired();
        
        builder.HasMany(c => c.Funkos)     
            .WithOne(f => f.Proveedor)           
            .HasForeignKey(f => f.ProveedorId)  
            .OnDelete(DeleteBehavior.Restrict);         // No borrar el proveedor si tiene funkos dentro
        
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
    
}