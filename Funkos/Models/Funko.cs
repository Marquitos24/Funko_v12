using System.ComponentModel.DataAnnotations;
using Funkos.Validations;

namespace Funkos.Models;

public record Funko
{
    public long Id { get; init; }
    
    [Required (ErrorMessage =  "El nombre del funko es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre del Funko no puede superar los 100 caracteres")]
    [MinLength(1, ErrorMessage = "El nombre del Funko debe tener más de 1 caracteres")]
    [NoLink]
    public required string Name { get; set; }
    
    [Required (ErrorMessage =  "El precio del funko es obligatorio")]
    [NoLink]
    public required decimal Precio  { get; set; }
    
    [Required(ErrorMessage = "La categoría es obligatoria")]
    [EnumDataType(typeof(Categoria), ErrorMessage = "La categoría no es válida")]
    [NoLink]
    public required Categoria Categoria { get; set; }
    
    
    public DateTime CreateAt {get; init; }
    public DateTime? UpdateAt {get; set; } 
    public DateTime? DeleteAt {get; init; }
}