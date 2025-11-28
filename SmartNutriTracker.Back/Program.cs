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

// Configure logging explicitly so console logs are visible
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Auditoría
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditService, AuditService>();


// Configuración JWT
var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    // Use cookies for local authentication (sign-in) and keep Jwt for API bearer
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "SmartNutriTrackerAuth"; // frontend expects this
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // poner true en producción
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

// Apply EF migrations at startup and seed default roles if missing (only in Development).
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var db = services.GetRequiredService<ApplicationDbContext>();
            // Apply any pending migrations
            db.Database.Migrate();

            // Seed default roles to avoid FK constraint issues when registering users
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
}

app.Run();

