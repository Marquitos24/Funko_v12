namespace Funkos.Models;

public record Funko
{
    public long Id { get; init; }
    
    public required string Name { get; set; } = string.Empty;
    
    public required decimal Precio  { get; set; }
    
    //Id Youtube
    public required string? CategoriaId { get; set; }
    
    public Categoria Categoria { get; set; } = null!;
    
    //MONGO
    public required string ProveedorId { get; set; }
    
    public required int Stock { get; set; }
    
    public DateTime CreateAt {get; init; }
    public DateTime? UpdateAt {get; set; } 
    
    public bool IsDeleted {get; set; }
    public DateTime? DeleteAt {get; set; }

    public string? Imagen { get; set; } = "default.png";
    
    public int Version { get; set; } = 1;
}