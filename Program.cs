using HealthBridgeAPI.Data;
using HealthBridgeAPI.Repositories.Implementations;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Implementations;
using HealthBridgeAPI.Services.Interfaces;
using HealthBridgeAPI.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.Configure<ModMedSettings>(
    builder.Configuration.GetSection("ModMedSettings")
);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddHttpClient<ModMedAuthService>();
builder.Services.AddHttpClient<IModMedRepository, ModMedRepository>();
builder.Services.AddScoped<IModMedPatientService, ModMedPatientService>();
builder.Services.AddScoped<IModMedAppointmentService, ModMedAppointmentService>();
builder.Services.AddScoped<IModMedAuthService, ModMedAuthService>();
builder.Services.AddScoped<IModMedPractitionerService, ModMedPractitionerService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
