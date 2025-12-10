using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Handlers;
using SmartNutriTracker.Back.Services.Audit;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Domain.Models.BaseModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger y logging
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString"))
);

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAllScopes();
builder.Services.AddControllers();

// CORS: permitir solo tu frontend para mayor seguridad
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
        //.WithOrigins("urlDeFrontendEnProduccion");
    });
});

// Auditor�a
builder.Services.AddHttpContextAccessor();

// JWT y cookies
var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty);

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
        // Ponerle true en prod
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["SmartNutriTrackerAuth"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

// Usar CORS antes de Authentication/Authorization
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Aplicar migraciones y seed roles
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        if (!db.Roles.Any())
        {
            db.Roles.Add(new Rol { Nombre = "Administrador" });
            db.Roles.Add(new Rol { Nombre = "Usuario" });
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error applying migrations or seeding the database.");
    }
}

app.Run();
