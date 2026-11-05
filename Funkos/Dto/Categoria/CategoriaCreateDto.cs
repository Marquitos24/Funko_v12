using System.ComponentModel.DataAnnotations;
using Funkos.Validations;

namespace Funkos.Dto.Categoria;

public record CategoriaCreateDto
{
    [Required (ErrorMessage =  "El nombre de la categoria es obligatoria")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "El nombre de la categoria no puede superar los 200 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Name { get; set; }
    
    [Required (ErrorMessage =  "La descripción de la categoria es obligatoria")]
    [StringLength(300, MinimumLength = 1, ErrorMessage = "La descripción de la categoria no puede superar los 300 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Descripcion  { get; set; }
}