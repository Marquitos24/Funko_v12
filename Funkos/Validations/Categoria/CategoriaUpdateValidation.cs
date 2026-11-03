using FluentValidation;
using Funkos.Dto.Categoria;
using Funkos.Repositories.Categoria;

namespace Funkos.Validations;

public class CategoriaUpdateValidation : AbstractValidator<CategoriaUpdateDto>
{
    public CategoriaUpdateValidation(ICategoriaRepository repo)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(200)
            .WithMessage("El nombre no puede superar los 200 caracteres")
            .MustAsync(async (dto, name, context, cancellationToken) =>
            {
                var id = (long)context.RootContextData["CatId"];
                return !await repo.ExistsByNameAsync(name, id, cancellationToken);
            })
            .WithMessage("Ya existe una categoria con ese nombre");

        RuleFor(x => x.Descripcion)
            .NotEmpty()
            .WithMessage("La descripción es obligatoria")
            .MaximumLength(300)
            .WithMessage("La descripción no puede superar los 300 caracteres");
    }
}