using System.ComponentModel.DataAnnotations;
using Funkos.Models;
using Funkos.Validations;

namespace Funkos.Dto;

public class FunkoQueryDto
{
    [MaxLength(100, ErrorMessage = "El nombre del Funko no puede superar los 100 caracteres")]
    [MinLength(1, ErrorMessage = "El nombre del Funko debe tener más de 1 caracteres")]
    [NoLink]
    public required string Name { get; set; } = string.Empty;
    
    [NoLink]
    public required decimal Precio  { get; set; } = 0;

    [EnumDataType(typeof(Categoria), ErrorMessage = "La categoría no es válida")]
    [NoLink]
    public required Categoria Categoria { get; set; } 
}