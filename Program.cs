using HealthBridgeAPI.Data;
using HealthBridgeAPI.HostedServices;
using HealthBridgeAPI.Repositories;
using HealthBridgeAPI.Repositories.Implementations;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services;
using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using HealthBridgeAPI.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// CONFIGURACIÓN DE SERVICIOS
// -----------------------------------------------------------------------------

// Leer configuración ModMed desde appsettings.json
builder.Services.Configure<ModMedSettings>(
    builder.Configuration.GetSection("ModMedSettings")
);

// Obtener cadena de conexión desde appsettings.json
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection)
);

// Registrar clientes HTTP y servicios de negocio
builder.Services.AddHttpClient<IModMedRepository, ModMedRepository>();
builder.Services.AddHttpClient<IModMedAuthService, ModMedAuthService>();
builder.Services.AddScoped<IModMedPatientService, ModMedPatientService>();
builder.Services.AddScoped<IModMedAppointmentService, ModMedAppointmentService>();
builder.Services.AddScoped<IModMedPractitionerService, ModMedPractitionerService>();
builder.Services.AddHostedService<ModMedPractitionerSyncHostedService>();
builder.Services.AddScoped<IAvailableDoctorsRepository, AvailableDoctorsRepository>();
builder.Services.AddScoped<IAvailableDoctorsService, AvailableDoctorsService>();
// -----------------------------------------------------------------------------
// CORS (permite peticiones desde tu frontend en GitHub Pages o React)
// -----------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

// -----------------------------------------------------------------------------
// CONTROLADORES Y OPENAPI
// -----------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// -----------------------------------------------------------------------------
// CONSTRUIR APP
// -----------------------------------------------------------------------------
var app = builder.Build();

// -----------------------------------------------------------------------------
// APLICAR MIGRACIONES AUTOMÁTICAMENTE (solo en desarrollo)
// -----------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas correctamente.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
    }
}

// -----------------------------------------------------------------------------
// MIDDLEWARES
// -----------------------------------------------------------------------------
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// -----------------------------------------------------------------------------
// CONTROLADORES
// -----------------------------------------------------------------------------
app.MapControllers();

// -----------------------------------------------------------------------------
// EJECUTAR APP
// -----------------------------------------------------------------------------
app.Run();
