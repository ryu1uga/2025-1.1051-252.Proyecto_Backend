using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Loop.Data;
using Loop.Services;
using FirebaseAdmin; 
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;

// Cargar variables de entorno (.env y .secrets.env para las credenciales de Firebase)
DotNetEnv.Env.Load();
if (File.Exists(".secrets.env"))
{
    DotNetEnv.Env.Load(".secrets.env");
}

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

// 🔑 Configurar y Inicializar Firebase Admin SDK (MODIFICACIÓN CLAVE)
try
{
    // 1. Obtener las credenciales de las variables de entorno
    var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
    var privateKey = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY");
    var clientEmail = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL");

    if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(clientEmail))
    {
        Log.Warning("Faltan variables de entorno de Firebase. La autenticación de Firebase no funcionará.");
    }
    else
    {
        // 2. Crear un objeto de configuración que simule el archivo JSON
        var config = new Dictionary<string, object>
        {
            { "type", "service_account" },
            { "project_id", projectId },
            { "private_key_id", Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID") },
            // Reemplaza los saltos de línea codificados (\n) para que sean interpretados correctamente
            { "private_key", privateKey}, 
            { "client_email", clientEmail },
            { "token_uri", "https://oauth2.googleapis.com/token" }
            // Puedes añadir otros campos si son necesarios, pero estos son los principales
        };

        // 3. Serializar la configuración en JSON
        var jsonConfig = System.Text.Json.JsonSerializer.Serialize(config);
        
        // 4. Crear las credenciales a partir del JSON en memoria
        var credential = GoogleCredential.FromJson(jsonConfig);

        // 5. Inicializar Firebase
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
                ProjectId = projectId // Opcional, pero bueno para la configuración explícita
            });
            Log.Information("Firebase Admin SDK inicializado correctamente usando credenciales de .env.");
        }
    }
}
catch (Exception ex)
{
    Log.Error(ex, "Error al inicializar Firebase Admin SDK desde .env.");
}

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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
    )
    .EnableSensitiveDataLogging() // logs detallados para debugging
    .LogTo(Console.WriteLine)
);

// 🔐 Configurar JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
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

// Configuración de EmailService
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();

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
