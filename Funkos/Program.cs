using System.Text.Json.Serialization;
using FluentValidation;
using Funkos.Cache;
using Funkos.DbContext;
using Funkos.Infrastructure;
using Funkos.Validations;
using Microsoft.AspNetCore.Http.Features;
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

#region Negociación de Codigo
// Negociación de codigo
    #region JSON
    // JSON
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


    #endregion

    #region XML

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

    #endregion
#endregion

#region Imagenes
// Configurar límite de formularios multipart
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});

// Configurar límite del request body
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
});
#endregion

// Ejecución automática vía action filter --> Paquete FluentValidation .AspNetCore
// Requiere llamarse DESPUÉS de AddControllers() = Solo funciona en MVC/controllers
//builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<ProveedorUpdateValidation>();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // <- Aquí conectas Serilog con el sistema de ILogger de .NET y lo Sustitulles

// ============================================================================
// 🚀 CONSTRUCCIÓN DE LA APLICACIÓN
// ============================================================================

var app = builder.Build();

app.UseExceptionHandler();

// Habilitar archivos estáticos desde wwwroot
app.UseStaticFiles();

Console.WriteLine($"WebRootPath: {app.Environment.WebRootPath}");
// Salida: C:\...\TuProyecto\wwwroot

Console.WriteLine($"ContentRootPath: {app.Environment.ContentRootPath}");
// Salida: C:\...\TuProyecto

app.MapControllers();
app.Run();


