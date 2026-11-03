namespace Funkos.Models;

public record Categoria
{
    public long Id { get; init; }

    public required string Name { get; set; } = string.Empty;
    
    public required string Descripcion  { get; set; } = string.Empty;
    
    public ICollection<Funko> Funkos { get; set; } = [];
    
    public DateTime CreateAt {get; set; }
    public DateTime? UpdateAt {get; set; } 
}