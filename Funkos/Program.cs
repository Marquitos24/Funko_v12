using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Funkos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddProblemDetails();
// Registrar los validators --> NO ejecuta la validación por sí solo
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Negociación de codigo
// JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//  XML
/*
 * builder.Services.AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = true;   // Respetar header Accept del cliente
        options.ReturnHttpNotAcceptable = true;       // Devolver 406 si formato no soportado
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddXmlSerializerFormatters(); // Cambiado a XmlSerializer para DTOs normales (sin [DataContract])
*/
// Ejecución automática vía action filter --> Paquete FluentValidation .AspNetCore
// Requiere llamarse DESPUÉS de AddControllers() = Solo funciona en MVC/controllers
builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();
app.Run();

