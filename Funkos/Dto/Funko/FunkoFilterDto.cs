using System.ComponentModel.DataAnnotations;
using Funkos.idGenerators;
using Funkos.Models;
using Funkos.Validations;

namespace Funkos.Dto.Funko;

public record FunkoFilterDto
{
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del Funko no puede superar los 100 caracteres y debe ser superior a 1")]
    [NoLink]
    public string? Name { get; init; } 
    
    [Range(1, 10000, ErrorMessage = "El precio maximo debe ser mayor a 0 con limite de 10000 €")]
    public decimal? PrecioMax  { get; init; } 
    
    [Range(1, 10000, ErrorMessage = "El precio minimo debe ser mayor a 0 €")]
    public decimal? PrecioMin  { get; init; } 
    
    [StringLength(200, MinimumLength = 1, ErrorMessage = "La categoria del Funko no puede superar los 200 caracteres y debe ser superior a 1")]
    public string? Categoria { get; init; }
    
    public bool? IsDeleted { get; init; }

    [Range(1, 10000000000000, ErrorMessage = "El precio debe ser mayor a 0")]
    public int? StockMin { get; init; } 

    [Required (ErrorMessage =  "El número de pagina es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "La página no puede ser negativa")]
    public int Page { get; init; } = 0;

    [Required (ErrorMessage =  "El número de elementos en la pagina es obligatorio")]
    [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
    public int Size { get; init; } = 10;
    
    // <summary>
    // Campo para ordenar resultados.
    // </summary>
    // <default>id</default>
    // <example>precio</example>
    public string SortBy = "id";

    // <summary>
    // Dirección de ordenamiento (asc/desc).
    // </summary>
    // <default>asc</default>
    // <example>desc</example>
    public string Direction = "asc";
}