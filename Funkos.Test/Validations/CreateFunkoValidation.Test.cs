using FluentValidation.TestHelper;
using Funkos.Dto;
using Funkos.Validations;

namespace Funkos.Test.Validations;

[TestFixture]
public class CreateFunkoValidation_Test
{
    private CreateFunkoValidation _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateFunkoValidation();
    }

    [Test]
    public void Deberia_Pasar_Cuando_ElPrecioEsValido()
    {
        // ARRANGE
        var dto = new FunkoCreateDto {Name="", Precio = 29.99m, Categoria = 0};

        // ACT
        var resultado = _validator.TestValidate(dto);

        // ASSERT
        resultado.ShouldNotHaveValidationErrorFor(x => x.Precio);
    }

    [TestCase(0)]
    [TestCase(-10.50)]
    public void Deberia_DarError_Cuando_ElPrecioEsMenorOIgualACero(decimal precioInvalido)
    {
        // ARRANGE
        var dto = new FunkoCreateDto { Name="", Precio = precioInvalido, Categoria = 0 };

        // ACT
        var resultado = _validator.TestValidate(dto);

        // ASSERT
        resultado.ShouldHaveValidationErrorFor(x => x.Precio)
            .WithErrorMessage("El precio debe ser mayor que 0");
    }

    [TestCase(10000.00)]
    [TestCase(99999.99)]
    public void Deberia_DarError_Cuando_ElPrecioSuperaElMaximo(decimal precioInvalido)
    {
        // ARRANGE
        var dto = new FunkoCreateDto { Name="", Precio = precioInvalido, Categoria = 0 };

        // ACT
        var resultado = _validator.TestValidate(dto);

        // ASSERT
        resultado.ShouldHaveValidationErrorFor(x => x.Precio)
            .WithErrorMessage("El precio no puede superar 9999.99 €");
    }
}