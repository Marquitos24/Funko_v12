namespace Funkos.Models;

public record Proveedor
{
    public long Id { get; init; }
    
    public required string Name { get; set; }
    
    public required string Descripcion  { get; set; }
    
    public ICollection<Funko> Funkos { get; set; } = [];
        
    public DateTime CreateAt {get; set; }
    public DateTime? UpdateAt {get; set; } 
    
    public bool IsDeleted {get; set; }
    public DateTime? DeleteAt {get; set; }
}