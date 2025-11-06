using HealthBridgeAPI.Models;
using HealthBridgeAPI.Services;
{
    
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ModMedSettings>(
    builder.Configuration.GetSection("ModMedSettings")
);
builder.Services.AddHttpClient<ModMedService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
