using Funkos.idGenerators;

namespace Funkos.Models;

public record Categoria
{
    public string IdY { get; set; } = null!; // ! en null ==> No mostrar advertencias por esto.

    public required string Name { get; set; } = string.Empty;
    
    public required string Descripcion  { get; set; } = string.Empty;
    
    public ICollection<Funko> Funkos { get; set; } = [];
    
    public DateTime CreateAt {get; set; }
    public DateTime? UpdateAt {get; set; } 
}