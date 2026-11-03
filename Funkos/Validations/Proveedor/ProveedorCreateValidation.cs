using FluentValidation;
using Funkos.Dto.Proveedor;
using Funkos.Repositories.Proveedor;

namespace Funkos.Validations;

public class ProveedorCreateValidation : AbstractValidator<ProveedorCreateDto>
{
    public ProveedorCreateValidation(IProveedorRepository repo)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres")
            .MustAsync(async (name, cancellationToken) =>
                !await repo.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("Ya existe un proveedor con ese nombre");

        RuleFor(x => x.Descripcion)
            .NotEmpty()
            .WithMessage("La descripción es obligatoria")
            .MaximumLength(300)
            .WithMessage("La descripción no puede superar los 300 caracteres");
    }
}