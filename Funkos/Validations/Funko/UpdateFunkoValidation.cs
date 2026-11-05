using FluentValidation;
using Funkos.Dto;
using Funkos.idGenerators;
using Funkos.Repositories;
using Funkos.Repositories.Categoria;
using Funkos.Repositories.Proveedor;

namespace Funkos.Validations;

public class UpdateFunkoValidation: AbstractValidator<FunkoUpdateDto>
{
    public UpdateFunkoValidation(ICategoriaRepository categoriaRepository,
        IProveedorRepository proveedorRepository, IFunkoRepository  funkoRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres")
            .MustAsync(async (dto, name, context, cancellationToken) =>
            {
                var id = (long)context.RootContextData["FunkoId"];
                return !await funkoRepository.ExistsByNameAsync(name, id, cancellationToken);
            })
            .WithMessage("El nombre del funko ya existe");

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(1m)
            .WithMessage("El precio debe ser como mínimo 1 €")
            .LessThan(10001)
            .WithMessage("El precio no puede superar 10000 €");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo");

        RuleFor(x => x.CategoriaId)
            .MaximumLength(IdYoutube.Longitud)
            .Matches(IdYoutube.PatronTexto)
            .WithMessage(IdYoutube.MensajeFormato)
            .MustAsync(async (id, cancellation) =>
                await categoriaRepository.ExistsAsync(id, cancellation))
            .WithMessage("La categoría no existe");

        RuleFor(x => x.ProveedorId)
            .MustAsync(async (id, cancellation) =>
                await proveedorRepository.ExistsAsync(id, cancellation))
            .WithMessage("El proveedor no existe");
    }
}