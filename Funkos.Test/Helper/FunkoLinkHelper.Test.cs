using FluentAssertions;
using Funkos.Helper;

namespace Funkos.Test.Helper;

[TestFixture]
public class FunkoLinkHelperTest
{


    [Test]
    public void GenerarEnlace_General_RetornaUrlCorrecta()
    {
        // Arrange
        long funkoId = 5;
        // Si tu helper necesita dependencias como un IUrlHelper o HttpContext simulado, 
        // se inicializan aquí (o se mockean si hace falta).

        // Act
        var resultado = FunkoLinkHelper.GenerateLinks(funkoId, "/api/funkos"); // (o como se llame tu método)

        // Assert
        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Contain("/api/funkos/5");
    }
    
    [Test]
    public void GenerarEnlace_Update_RetornaUrlCorrecta()
    {
        // Arrange
        long funkoId = 5;
        // Si tu helper necesita dependencias como un IUrlHelper o HttpContext simulado, 
        // se inicializan aquí (o se mockean si hace falta).

        // Act
        var resultado = FunkoLinkHelper.GenerateUpdateLinks(funkoId, "/api/funkos"); // (o como se llame tu método)

        // Assert
        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Contain("/api/funkos/5");
    }
    
    [Test]
    public void GenerarEnlace_Delete_RetornaUrlCorrecta()
    {
        // Arrange
        long funkoId = 5;
        // Si tu helper necesita dependencias como un IUrlHelper o HttpContext simulado, 
        // se inicializan aquí (o se mockean si hace falta).

        // Act
        var resultado = FunkoLinkHelper.GenerateDeleteLinks(funkoId, "/api/funkos"); // (o como se llame tu método)

        // Assert
        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Contain("/api/funkos/5");
    }
}