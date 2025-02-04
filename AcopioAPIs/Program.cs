using AcopioAPIs.Models;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
builder.Services.AddScoped<ITicket, TicketRepository>();
builder.Services.AddScoped<ICorte, CorteRepository>();
builder.Services.AddScoped<ICarguillo, CarguilloRepository>();
builder.Services.AddScoped<IRecojo, RecojoRepository>();
builder.Services.AddScoped<IServicioTransporte, ServicioTransporteRepository>();
builder.Services.AddScoped<IServicioPalero, ServicioPaleroRepository>();
builder.Services.AddScoped<ILiquidacion, LiquidacionRepository>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<IAuthorization, AuthorizationRepository>();
builder.Services.AddScoped<ITesoreria, TesoreriaRepository>();
builder.Services.AddScoped<IProducto, ProductoRepository>();
builder.Services.AddScoped<IDistribuidor, DistribuidorRepository>();
builder.Services.AddScoped<ICompra, CompraRepository>();
builder.Services.AddScoped<ITipos, TiposRepository>();
builder.Services.AddScoped<IVenta, VentaRepository>();


builder.Services.AddDbContext<DbacopioContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.WithOrigins("http://localhost:5173")
        .AllowAnyMethod().AllowAnyHeader();
    });
});

// Configurar JWT
var key = builder.Configuration.GetValue<string>("JwtSetting:secretKey");
var keyBytes = Encoding.ASCII.GetBytes(key!);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    // Deshabilitar el HTTPS
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // validar el usuario
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // credenciales del token
        ValidateIssuer = false, // quién solicita el token
        ValidateAudience = false, // desde dónde solicita el token
        ValidateLifetime = true, // tiempo de vida del token 
        ClockSkew = TimeSpan.Zero, // evitar desviación del tiempo de vida del token
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NewPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
