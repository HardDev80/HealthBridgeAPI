using System.Threading.RateLimiting;
using HealthBridgeAPI.Data;
using HealthBridgeAPI.Repositories.Implementations;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using HealthBridgeAPI.Utils;
using Microsoft.AspNetCore.RateLimiting;
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

// -----------------------------------------------------------------------------
// RATE LIMITING (límite global por IP)
// -----------------------------------------------------------------------------
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1),  
            QueueLimit = 0,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// -----------------------------------------------------------------------------
// CORS
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
    app.UseDeveloperExceptionPage();

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
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// -----------------------------------------------------------------------------
// MIDDLEWARES
// -----------------------------------------------------------------------------

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
    await next();
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");       
app.UseRateLimiter();         

app.UseAuthorization();

// -----------------------------------------------------------------------------
// CONTROLADORES
// -----------------------------------------------------------------------------
app.MapControllers();

// -----------------------------------------------------------------------------
// EJECUTAR APP
// -----------------------------------------------------------------------------
app.Run();
