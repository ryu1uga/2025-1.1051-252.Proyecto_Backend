using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Loop.Data;
using Loop.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // For Docker
});

// Agregar CORS para permitir solicitudes desde 'http://localhost:*/'
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("CONNECTION_STRING")))
               .EnableSensitiveDataLogging() // Enable sensitive data logging for debugging
               .LogTo(Console.WriteLine)
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
app.Urls.Add(Environment.GetEnvironmentVariable("APP_URL"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Aplicar la política de CORS antes de los controladores
app.UseCors("AllowSpecificOrigin");

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();
