using System.ComponentModel.DataAnnotations;
using Funkos.idGenerators;
using Funkos.Models;
using Funkos.Validations;

namespace Funkos.Dto;

public record FunkoUpdateDto
{
    public long Id { get; set; }
    
    [Required (ErrorMessage =  "El nombre del funko es obligatorio")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del Funko no puede superar los 100 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Name { get; set; } = string.Empty;
    
    [Required (ErrorMessage =  "El precio del funko es obligatorio")]
    [Range(1, 10000, ErrorMessage = "El precio debe ser mayor a 0 con limite de 10000 €")]
    [NoLink]
    public required decimal Precio  { get; set; } = 0;
    
    [IdYoutube]
    public required string CategoriaId { get; set; }
    
    [RegularExpression(@"^[0-9a-fA-F]{24}$", ErrorMessage =  "Formato invalido de ID")]
    public required string ProveedorId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Inserte un stock mayor que 0")]
    public required int Stock { get; set; }
}