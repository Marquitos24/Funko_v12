using FluentValidation;
using Funkos.Dto;

namespace Funkos.Validations;

public class UpdateFunkoValidation: AbstractValidator<FunkoUpdateDto>
{
    public UpdateFunkoValidation()
    {
        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor que 0")
            .LessThan(9999.99m).WithMessage("El precio no puede superar 9999.99 €");
    }
}