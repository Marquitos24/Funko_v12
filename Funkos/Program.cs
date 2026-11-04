using System.Text.Json.Serialization;
using FluentValidation;
using Funkos.Cache;
using Funkos.DbContext;
using Funkos.Infrastructure;
using Funkos.Validations;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// appsettings.json + appsettings.{Environment}.json + variables de entorno
//var miConfig = builder.Configuration.GetSection("Application").Get<AppConfig>();
    
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddProblemDetails();
// Registrar los validators --> NO ejecuta la validación por sí solo
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// PostgreSQL (proveedor del curso)
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

//Cache-Redis
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(CacheOptions.SectionName));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

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
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<ProveedorUpdateValidation>();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // <- Aquí conectas Serilog con el sistema de ILogger de .NET y lo Sustitulles

var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();
app.Run();


