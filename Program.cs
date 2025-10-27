using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UepexApiDemo.Data;
using UepexApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
// 1. DbContext (misma base para Estudiantes y Usuarios)
// -----------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // Si usas otra base (Sqlite, Postgres), cámbialo aquí
});

// -----------------------------------------------------
// 2. Servicios de aplicación / dominio
// -----------------------------------------------------
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IValidacionService, ValidacionService>(); // el que ya tenías

builder.Services.AddControllers();

// -----------------------------------------------------
// 3. Autenticación JWT
// -----------------------------------------------------
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // sin minutos gratis extra
        };
    });

builder.Services.AddAuthorization();

// -----------------------------------------------------
// 4. Swagger con soporte Bearer
// -----------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Uepex API",
        Version = "v1",
        Description = "API UEPEX con autenticación JWT"
    });

    // Definir el esquema de seguridad (Bearer)
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduce el token JWT con el formato: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Exigir el esquema de seguridad en las operaciones protegidas
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// -----------------------------------------------------
// 5. Middleware
// -----------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS es buena práctica aunque estés en local
app.UseHttpsRedirection();

// IMPORTANTE: primero autenticación, luego autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
