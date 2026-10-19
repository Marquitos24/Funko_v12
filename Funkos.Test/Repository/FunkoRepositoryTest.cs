using Funkos.Models;
using Funkos.Repositories;
using NUnit.Framework;
using FluentAssertions;

namespace Funkos.Test.Repository;

[TestFixture]
public class FunkoRepositoryTest
{
    private Funko funko;
    private FunkoRepository repo;

    [SetUp]
    public void SetUp()
    {
        repo = new FunkoRepository();

        funko = new Funko()
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };

        repo.Create(funko);
    }
    
    

    [Test]
    public void GetAll_RetornaListaFunkos()
    {
        // Act
        var devolverTodos = repo.GetAll();

        // Assert
        devolverTodos.Should().NotBeNull();
        devolverTodos.Should().HaveCount(1);
        devolverTodos[0].Id.Should().Be(1);
        devolverTodos[0].Name.Should().Be("Cristian");
    }
    [Test]
    public void GetById_RetornaFunko()
    {
        // Act
        var funkoId = repo.GetById(funko.Id);

        // Assert
        funkoId.Should().NotBeNull();
        funkoId.Id.Should().Be(1);
        funkoId.Name.Should().Be("Cristian");
        funkoId.Precio.Should().Be(24);
        funkoId.Categoria.Should().Be(0);
    }
    
    [Test]
    public void Create_RetornaDatosFunko()
    {
        // Act
        var funkoCreado = repo.Create(funko);

        // Assert
        funkoCreado.Should().NotBeNull();
        funkoCreado.Id.Should().Be(1);
        funkoCreado.Name.Should().Be("Cristian");
        funkoCreado.Precio.Should().Be(24);
        funkoCreado.Categoria.Should().Be(0);
    }
    
    [Test]
    public void Update_RetornaDatosFunko()
    {
        // Arrange
        var funkoUpdate = new Funko()
        {
            Name = "Cristian actualizado",
            Precio = 30,
            Categoria = 0
        };
        // Act
        var funkActualizado = repo.Update(1, funkoUpdate);

        // Assert
        funkActualizado.Should().NotBeNull();
        funkActualizado.Id.Should().Be(1);
        funkActualizado.Name.Should().Be("Cristian actualizado");
        funkActualizado.Precio.Should().Be(30);
        funkActualizado.Categoria.Should().Be(0);
    }
    
    [Test]
    public void Delete_RetornaDatosFunko()
    {
        // Arrange
        
        // Act
        var funkEliminado = repo.Delete(1);

        // Assert
        funkEliminado.Should().BeTrue();
    }
}
  