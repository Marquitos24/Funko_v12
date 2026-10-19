using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Funkos.Validations;

public class NoLinkAttribute : ValidationAttribute
{
    private const string LinkPattern = @"(?i)(https?:\/\/|www\.|[a-zA-Z0-9-]+\.(com|es|net|org|eu|info|biz|io|co|me|xyz))\b";
    
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        // Si el valor es null o no es string, es válido (otro atributo se encarga del Required)
        if (value is not string texto)
            return ValidationResult.Success;

        if (Regex.IsMatch(texto, LinkPattern))
        {
            return new ValidationResult("No se admiten enlaces");
        }

        // Si todo está bien, éxito
        return ValidationResult.Success;
    }
}