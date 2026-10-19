using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Funkos.Dto;
using Funkos.Mappers;
using Funkos.Models;
using Funkos.Repositories;
using Funkos.Service;
using Moq;

namespace Funkos.Test.Services;

[TestFixture]
public class FunkoServiceTest
{
    private FunkoService _service;
    private FunkoUpdateDto _updateDto;
    private FunkoCreateDto _createDto;
    private FunkosDtoResponse _responseDto;
    private Mock<IValidator<FunkoCreateDto>> _validatorCreateMock;
    private Mock<IValidator<FunkoUpdateDto>> _validatorUpdateMock;
    private Mock<IFunkoRepository> _funkoRepositoryMock = null;
    private static List<Funko> _funkos = new();
    
    [SetUp]
    public void SetUp()
    {
        _funkoRepositoryMock = new Mock<IFunkoRepository>();
        
        _validatorCreateMock = new Mock<IValidator<FunkoCreateDto>>();
        _validatorUpdateMock = new Mock<IValidator<FunkoUpdateDto>>();
        _service = new FunkoService(_funkoRepositoryMock.Object, _validatorCreateMock.Object, _validatorUpdateMock.Object);
        
        
        _funkos = new List<Funko>()
        {
            new Funko 
            { 
                Id = 1,
                Name = "Cristian",
                Precio = 24,
                Categoria = 0
            },
            new Funko 
            { 
                Id = 2,
                Name = "Paco",
                Precio = 25,
                Categoria = 0
            }
        };
    }
    [Test]
    public void Add_DeberiaRetornarError_CuandoLaValidacionFalla()
    {
        // ARRANGE
        var dto1 = new FunkoCreateDto { Name = "", Precio = -1, Categoria = 0};
        
        var dto2 = new FunkoCreateDto { Name = "", Precio = 1000000000, Categoria = 0};

        // Simulamos que el validador devuelve un error
        var failures = new List<ValidationFailure> { new ValidationFailure("Precio", "El precio debe ser mayor que 0") };
        var validationResult = new ValidationResult(failures);

        // Le decimos al mock: "cuando alguien llame a Validate, devuelve este resultado con errores"
        _validatorCreateMock.Setup(v => v.Validate(dto1)).Returns(validationResult);
        _validatorCreateMock.Setup(v => v.Validate(dto2)).Returns(validationResult);

        // ACT
        var resultado = _service.Add(dto1);
        var resultado2 = _service.Add(dto2);

        // ASSERT
        resultado.IsFailure.Should().BeTrue();
        resultado2.IsFailure.Should().BeTrue();
    }
    
    [Test]
    public void Create_Retorna_1Funko()
    {
        // ARRANGE: Configuramos el comportamiento del mock usando los datos del SetUp
        var newfunkoRequest = new FunkoCreateDto
        {
            Name = "paz",
            Precio = 300,
            Categoria = 0
        };
        
        var newfunko = new Funko
        {
            Id = 3,
            Name = "paz",
            Precio = 300,
            Categoria = 0
        };
        // como el servicio llama a GetAll() internamente para validar, debemos mockearlo 
        // (le pasamos una lista vacía _funkos)
        _funkoRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Funko>());
        _funkoRepositoryMock.Setup(r => r.Create(It.IsAny<Funko>())).Returns(newfunko);
        
        var validationResult = new FluentValidation.Results.ValidationResult(); // IsValid por defecto es true
        _validatorCreateMock.Setup(v => v.Validate(newfunkoRequest)).Returns(validationResult);

        // ACT: Ejecutamos el método que queremos probar
        var resultado = _service.Add(newfunkoRequest);

        // ASSERT: Comprobamos que el resultado es el esperado
        resultado.IsSuccess.Should().BeTrue();
        resultado.Should().NotBe(null);
        resultado.Value.Should().BeEquivalentTo(newfunko, options => options.ExcludingMissingMembers());
        _funkoRepositoryMock.Verify(repository => repository.Create(It.IsAny<Funko>()), Times.Once);
    }
    
    [Test]
    public void GetAll_Reotna_Todos_Los_Funkos()
    {
        // ARRANGE: Configuramos el comportamiento del mock usando los datos del SetUp
        _funkoRepositoryMock.Setup(r => r.GetAll()).Returns(_funkos);

        // ACT: Ejecutamos el método que queremos probar
        var resultado = _service.GetAll(null, null, null);

        // ASSERT: Comprobamos que el resultado es el esperado
        resultado.IsSuccess.Should().BeTrue();
        resultado.Should().NotBeNull();
        resultado.Value.Should().NotBeNullOrEmpty();
    
        // Debería quedarse solo con Goku (Precio 20, Marvel). Paco pasa de precio y Ana es de otra categoría.
        resultado.Value.Should().HaveCount(2);
        resultado.Value[0].Name.Should().Be("Cristian");
        
        resultado.Value.Should().BeEquivalentTo(_funkos, options => options.ExcludingMissingMembers());
        _funkoRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
    }
    [Test]
public void GetAll_DeberiaFiltrarCorrectamente_SegunParametros()
{
    // ARRANGE: Preparamos los datos de prueba y configuramos el mock
    var funkosDePrueba = new List<Funko>
    {
        new Funko { Id = 1, Name = "Goku", Precio = 20.00m, Categoria = Categoria.Marvel },
        new Funko { Id = 2, Name = "Paco", Precio = 150.00m, Categoria = Categoria.Marvel },
        new Funko { Id = 3, Name = "Ana", Precio = 50.00m, Categoria = Categoria.Anime },
        new Funko { Id = 4, Name = "Cristian", Precio = 29.88m, Categoria = Categoria.Marvel }
    };

    _funkoRepositoryMock.Setup(r => r.GetAll()).Returns(funkosDePrueba);

    // ACT & ASSERT: Probamos diferentes combinaciones de filtros paso a paso dentro del mismo test

    // 1. Sin filtros (deben salir todos)
    var res1 = _service.GetAll(null, null, null);
    res1.IsSuccess.Should().BeTrue();
    res1.Value.Should().HaveCount(4);

    // 2. Filtrando solo por categoría
    var res2 = _service.GetAll("Marvel", null, null);
    res2.IsSuccess.Should().BeTrue();
    res2.Value.Should().HaveCount(3);

    // 3. Filtrando por precio mínimo
    var res3 = _service.GetAll(null, 40.00m, null);
    res3.IsSuccess.Should().BeTrue();
    res3.Value.Should().HaveCount(2);

    // 4. Filtrando por precio máximo
    var res4 = _service.GetAll(null, null, 60.00m);
    res4.IsSuccess.Should().BeTrue();
    res4.Value.Should().HaveCount(3);

    // 5. Filtro combinado (categoría + rangos de precio)
    var res5 = _service.GetAll("Marvel", 10.00m, 30.00m);
    res5.IsSuccess.Should().BeTrue();
    res5.Value.Should().HaveCount(2);
    res5.Value.Select(f => f.Name).Should().Contain(new[] { "Goku", "Cristian" });

    // 6. Filtro sin resultados (lista vacía)
    var res6 = _service.GetAll("Anime", 100.00m, null);
    res6.IsSuccess.Should().BeTrue();
    res6.Value.Should().BeEmpty();

    // Verificamos cuantas veces se invocó al repositorio en total
    _funkoRepositoryMock.Verify(r => r.GetAll(), Times.Exactly(6));
}
    
    [Test]
    public void GetById_Reotna_1Funko()
    {
        // ARRANGE: Configuramos el comportamiento del mock usando los datos del SetUp
        long id = 1;
        var funkoEsperado = new Funko
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        
        _funkoRepositoryMock.Setup(r => r.GetById(id)).Returns(funkoEsperado);

        // ACT: Ejecutamos el método que queremos probar
        var resultado = _service.GetById(id);

        // ASSERT: Comprobamos que el resultado es el esperado
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeEquivalentTo(funkoEsperado, options => options.ExcludingMissingMembers());
        _funkoRepositoryMock.Verify(repository => repository.GetById(id), Times.Once);
    }
    
    [Test]
    public void Update_DeberiaRetornarError_CuandoLaValidacionFalla()
    {
        // ARRANGE
        var dto1 = new FunkoUpdateDto { Id = 1, Name = "", Precio = -1, Categoria = 0};
        var dto2 = new FunkoUpdateDto {  Id = 1, Name = "", Precio = 1000000000, Categoria = 0};

        // Simulamos que el validador devuelve un error
        var failures = new List<ValidationFailure> { new ValidationFailure("Precio", "El precio debe ser mayor que 0") };
        var validationResult = new ValidationResult(failures);

        // Le decimos al mock: "cuando alguien llame a Validate, devuelve este resultado con errores"
        _validatorUpdateMock.Setup(v => v.Validate(dto1)).Returns(validationResult);
        _validatorUpdateMock.Setup(v => v.Validate(dto2)).Returns(validationResult);

        // ACT
        var resultado = _service.Update(dto1.Id, dto1);
        var resultado2 = _service.Update(dto2.Id, dto2);

        // ASSERT
        resultado.IsFailure.Should().BeTrue();
        resultado2.IsFailure.Should().BeTrue();
    }
    
    [Test]
    public void Update_Retorna_1Funko()
    {
        // ARRANGE: Configuramos el comportamiento del mock usando los datos del SetUp
        long id = 1;
        var funko = new Funko
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        var funkoActualizado = new Funko
        {
            Id = 1,
            Name = "Superman",
            Precio = 200,
            Categoria = 0
        };
        
        var updatefunkoRequest = new FunkoUpdateDto
        {
            Id = 1,
            Name = "Superman",
            Precio = 200,
            Categoria = 0
        };
        
        var validationResult = new FluentValidation.Results.ValidationResult();
        _validatorUpdateMock.Setup(v => v.Validate(updatefunkoRequest)).Returns(validationResult);
        
        _funkoRepositoryMock.Setup(r => r.GetById(id)).Returns(funko);
        _funkoRepositoryMock.Setup(r => r.GetAll()).Returns(_funkos);

        _funkoRepositoryMock.Setup(r => r.Update(id, It.IsAny<Funko>())).Returns(funkoActualizado);
        // ACT: Ejecutamos el método que queremos probar
        var resultado = _service.Update(id, updatefunkoRequest);

        // ASSERT: Comprobamos que el resultado es el esperado
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeEquivalentTo(funkoActualizado, options => options.ExcludingMissingMembers());
        _funkoRepositoryMock.Verify(repository => repository.Update(id, It.IsAny<Funko>()), Times.Once);
    }
    
    [Test]
    public void Delete_Retorna_Bool()
    {
        // ARRANGE: Configuramos el comportamiento del mock usando los datos del SetUp
        long id = 1;
        var funko = new Funko
        {
            Id = 1,
            Name = "Cristian",
            Precio = 24,
            Categoria = 0
        };
        var deleted = true;
        _funkoRepositoryMock.Setup(r => r.GetById(id)).Returns(funko);

        _funkoRepositoryMock.Setup(r => r.Delete(id)).Returns(deleted);

        // ACT: Ejecutamos el método que queremos probar
        var resultado = _service.Delete(id);

        // ASSERT: Comprobamos que el resultado es el esperado
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeTrue();
        _funkoRepositoryMock.Verify(repository => repository.Delete(id), Times.Once);
    }
}


