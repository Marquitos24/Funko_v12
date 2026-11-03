using FluentValidation;
using Funkos.Dto.Categoria;
using Funkos.Dto.Proveedor;
using Funkos.Repositories.Categoria;

namespace Funkos.Validations;

public class CategoriaCreateValidation : AbstractValidator<CategoriaCreateDto>
{
    public CategoriaCreateValidation(ICategoriaRepository repo)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres")
            .MustAsync(async (name, cancellationToken) =>
                !await repo.ExistsByNameAsync(name, null, cancellationToken))
            .WithMessage("Ya existe una categoria con ese nombre");

        RuleFor(x => x.Descripcion)
            .NotEmpty()
            .WithMessage("La descripción es obligatoria")
            .MaximumLength(300)
            .WithMessage("La descripción no puede superar los 300 caracteres");
    }
}