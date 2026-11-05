using System.ComponentModel.DataAnnotations;

namespace Funkos.idGenerators;

/// <summary>
/// Atributo de validación para propiedades que contienen un identificador estilo YouTube.
/// Se aplica a los DTO de petición.
///
/// Que el identificador exista ya en base de datos es una comprobación distinta y pertenece
/// al servicio (devuelve conflicto, no error de validación).
///
/// La regla de formato vive una sola vez en <see cref="IdYoutube"/>; este atributo y la
/// regla FluentValidation equivalente delegan en ella para que no puedan divergir.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class IdYoutubeAttribute : ValidationAttribute
{
    /// <summary>
    /// Comprueba que el valor sea un identificador válido o que esté vacío.
    /// Un valor nulo o vacío es aceptado porque el identificador puede generarse en el servidor.
    /// </summary>
    /// <param name="value">Valor de la propiedad.</param>
    /// <param name="context">Contexto de validación del modelo.</param>
    /// <returns>Éxito si el valor es nulo, vacío o tiene el formato correcto.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        //Permitimos que el id llegue vacio pq speramo generarlo 
        if (value is null || string.IsNullOrWhiteSpace((string)value))
            return ValidationResult.Success;

        return IdYoutube.EsValido((string)value)
            ? ValidationResult.Success
            : new ValidationResult(IdYoutube.MensajeFormato);
    }
}