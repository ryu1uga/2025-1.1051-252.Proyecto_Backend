using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Loop.Data;

// Cargar variables de entorno
DotNetEnv.Env.Load();

// 📁 Configurar ruta del log
var logFilePath = Path.Combine(AppContext.BaseDirectory, "Logs", "login-audit-.log");

// 🪵 Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// 🪵 Usar Serilog como proveedor de logging
builder.Host.UseSerilog();

// Configurar Kestrel para Docker
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // escucha en todas las IPs
});

// CORS: permitir cualquier origen, header y método (para desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Configurar Controllers y evitar ciclos de referencia JSON
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configurar PostgreSQL con EF Core
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("CONNECTION_STRING_PRODUCTS"))
    )
    .EnableSensitiveDataLogging() // logs detallados para debugging
    .LogTo(Console.WriteLine)
);

// 🔐 Configurar JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]); // Lee la clave compartida
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // ...
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key), // Usa la clave compartida
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// 🔹 Configurar Swagger con JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Loop API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token en este formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHttpClient("ProductService", client =>
{
    // client.BaseAddress usará la variable PRODUCT_SERVICE_URL definida en docker-compose
    client.BaseAddress = new Uri(builder.Configuration["PRODUCT_SERVICE_URL"]!);
});

var app = builder.Build();

// Configurar URL (para Docker)
var appUrl = Environment.GetEnvironmentVariable("APP_URL");
if (!string.IsNullOrEmpty(appUrl))
{
    app.Urls.Add(appUrl);
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Loop API v1");
        options.RoutePrefix = string.Empty; // Swagger en /
    });
}

// CORS
app.UseCors("AllowSpecificOrigin");

// Autenticación / Autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear Controllers
app.MapControllers();

// Ejecutar aplicación
app.Run();