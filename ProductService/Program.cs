using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductService.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Controladores
builder.Services.AddControllers();

// 2. Conexión a la BD de productos (Asegúrate que en appsettings.json sea 'ProductConnection')
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductConnection")));

// 3. CONFIGURACIÓN DE JWT (Debe ser igual a la del CustomerService)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretaLlaveParaBuildCostQueNadieDebeSaber12345!";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BuildCostAuthServer",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BuildCostUsers",
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. ORDEN CRÍTICO: Autenticación antes que Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();