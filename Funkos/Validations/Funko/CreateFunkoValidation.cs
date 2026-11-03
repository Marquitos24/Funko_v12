using FluentValidation;
using Funkos.Dto.Funko;
using Funkos.Repositories;
using Funkos.Repositories.Categoria;
using Funkos.Repositories.Proveedor;

namespace Funkos.Validations;

public class CreateFunkoValidationAsync : AbstractValidator<FunkoCreateDto>
{
    public CreateFunkoValidationAsync(ICategoriaRepository categoriaRepository,
        IProveedorRepository proveedorRepository, IFunkoRepository funkoRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres")
            .MustAsync(async (name, cancellation) =>
                !await funkoRepository.ExistsByNameAsync(name, null, cancellation))
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
            .MustAsync(async (id, cancellation) =>
                await categoriaRepository.ExistsAsync(id, cancellation))
            .WithMessage("La categoría no existe")
            .GreaterThan(0)
            .WithMessage("Debes seleccionar una categoría válida");

        RuleFor(x => x.ProveedorId)
            .MustAsync(async (id, cancellation) =>
                await proveedorRepository.ExistsAsync(id, cancellation))
            .WithMessage("El proveedor no existe")
            .GreaterThan(0)
            .WithMessage("Debes seleccionar un proveedor válido");
    }
}