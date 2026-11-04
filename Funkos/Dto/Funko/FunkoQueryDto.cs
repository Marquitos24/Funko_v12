using System.ComponentModel.DataAnnotations;
using Funkos.Models;
using Funkos.Validations;

namespace Funkos.Dto.Funko;

public class FunkoQueryDto
{
    [Required (ErrorMessage =  "El nombre del funko es obligatorio")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del Funko no puede superar los 100 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Name { get; set; } = string.Empty;
    
    [Required (ErrorMessage =  "El precio del funko es obligatorio")]
    [Range(1, 10000, ErrorMessage = "El precio debe ser mayor a 0 con limite de 10000 €")]
    [NoLink]
    public required decimal Precio  { get; set; } = 0;
 
    [Required(ErrorMessage = "La categoria es obligatoria.")]
    [Range(1, long.MaxValue, ErrorMessage = "Inserte un id mayor que 0")]
    public required long CategoriaId { get; set; }
}