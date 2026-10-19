using Funkos.Mappers;
using Funkos.Models;
using Funkos.Dto;
using NUnit.Framework;
using FluentAssertions;

namespace Funkos.Test.Mappers;

[TestFixture]
public class FunkoMapperTest
{
    private FunkoCreateDto _createDto;
    private FunkoUpdateDto _updateDto;
    private Funko funko;
    
    [SetUp]
    public void SetUp()
    {
        _updateDto = new FunkoUpdateDto()
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        
        _createDto = new FunkoCreateDto()
        {
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        
        funko = new Funko()
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        
        
    }
    
    [Test]
    public void ToEntity_Ok()
    {
        
        // Act (ejecutar)
        var funkCreate = FunkoMapper.ToEntityCreate(_createDto);
        var funkUpdate = FunkoMapper.ToEntityUpdate(_updateDto);
    
        // Assert (Validar con FluentAssertions --> Mas comprensible)
        funkCreate.Should().NotBeNull();  // Comprobamos que usuario no sea nulo
        // Comprobamos que ningun valor llegue como nulo, vacio o cambiado
        funkCreate.Name.Should().Be(_createDto.Name);
        funkCreate.Precio.Should().Be(_createDto.Precio);
        funkCreate.Categoria.Should().Be(_createDto.Categoria);
        
        funkUpdate.Should().NotBeNull();  // Comprobamos que usuario no sea nulo
        // Comprobamos que ningun valor llegue como nulo, vacio o cambiado
        funkUpdate.Id.Should().Be(_updateDto.Id);
        funkUpdate.Name.Should().Be(_updateDto.Name);
        funkUpdate.Precio.Should().Be(_updateDto.Precio);
        funkUpdate.Categoria.Should().Be(_updateDto.Categoria);
    }
    
    [Test]
    public void ToDTOResponse_Ok()
    {
        // Act (ejecutar)
        var funk = FunkoMapper.ToDtoResponse(funko);
        
        funk.Should().NotBeNull();  
        funk.Id.Should().Be(funko.Id); 
        funk.Name.Should().Be(funko.Name);
        funk.Precio.Should().Be(funko.Precio);
        funk.Categoria.Should().Be(funko.Categoria);
    }
}