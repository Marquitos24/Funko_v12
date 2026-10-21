using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Funkos.Validations;

namespace Funkos.Test.Validations;

[TestFixture]
public class NoLinkAttributeTest
{
    
    [Test]
    public void Validar_validadorPersonalizado_RetornaError()
    {
        //A1
        var attribute = new NoLinkAttribute();
        var context = new ValidationContext(new object());

        //A2
        var result = attribute.GetValidationResult("https://gemini.google.com/app/f9147a9e071161c7", context);

        //A3
        result.Should().NotBeNull();
        result!.ErrorMessage.Should().Be("No se admiten enlaces");
    }
}