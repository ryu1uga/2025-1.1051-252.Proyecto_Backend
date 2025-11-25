using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Loop.Data;

// Cargar variables de entorno locales (solo en desarrollo)
DotNetEnv.Env.Load();

// Configurar ruta y carpeta de logs (Render usa filesystem efímero)
var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
Directory.CreateDirectory(logDirectory);
var logFilePath = Path.Combine(logDirectory, "login-audit-.log");

// Configurar Serilog
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

// Usar Serilog como proveedor de logging
builder.Host.UseSerilog();

// Configurar Kestrel para Render/Docker
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "8080"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Controllers
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// PostgreSQL con EF Core (productos)
// Se prioriza CONNECTION_STRING_PRODUCTS (cadena completa o nombre de cadena del config)
var productConnEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_PRODUCTS");
var productConn = string.IsNullOrWhiteSpace(productConnEnv)
    ? builder.Configuration.GetConnectionString("ProductConnection")
    : productConnEnv.Contains("Host=", StringComparison.OrdinalIgnoreCase) || productConnEnv.Contains("://", StringComparison.OrdinalIgnoreCase)
        ? productConnEnv
        : builder.Configuration.GetConnectionString(productConnEnv);

if (string.IsNullOrWhiteSpace(productConn))
{
    Log.Error("No se encontró la cadena de conexión para 'ProductConnection' ni se pudo resolver 'CONNECTION_STRING_PRODUCTS'.");
}

builder.Services.AddDbContext<ProductDbContext>(options => options
    .UseNpgsql(productConn!)
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine)
);

// JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Swagger con JWT
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
    var productServiceUrl = builder.Configuration["PRODUCT_SERVICE_URL"];
    if (string.IsNullOrWhiteSpace(productServiceUrl))
    {
        Log.Warning("No se configuró PRODUCT_SERVICE_URL; el HttpClient 'ProductService' no tendrá BaseAddress.");
    }
    else
    {
        client.BaseAddress = new Uri(productServiceUrl);
    }
});

var app = builder.Build();

// APP URL (Docker/Render)
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
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
