using System.ComponentModel.DataAnnotations;
using Funkos.Models;
using Funkos.Validations;

namespace Funkos.Dto.Proveedor;

public record ProveedorCreateDto
{
    [Required (ErrorMessage =  "El nombre del proveedor es obligatoria")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "El nombre del proveedor no puede superar los 200 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Name { get; set; } = string.Empty;
    
    [Required (ErrorMessage =  "La descripción del proveedor es obligatorio")]
    [StringLength(300, MinimumLength = 1, ErrorMessage = "La descripción del proveedor no puede superar los 300 caracteres y debe ser superior a 1")]
    [NoLink]
    public required string Descripcion  { get; set; }
}