using AcopioAPIs.Models;
using AcopioAPIs.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProveedor, ProveedorRepository>();
builder.Services.AddScoped<ITierra, TierraRepository>();
builder.Services.AddScoped<IAsignarTierra, AsignarTierraRepository>();
builder.Services.AddScoped<ICosecha, CosechaRepository>();

builder.Services.AddDbContext<DbacopioContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NewPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
