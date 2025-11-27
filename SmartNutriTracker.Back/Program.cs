using Microsoft.AspNetCore.Authentication.Cookies;
using SmartNutriTracker.Back.Handlers;
using SmartNutriTracker.Back.Services.Audit;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Back.Services.Users;
using System.Text;
using SmartNutriTracker.Domain.Models.BaseModels;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

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

// Auditor�a
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditService, AuditService>();


// Configuraci�n JWT
var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // poner true en producci�n
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
    };
});
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
app.Run();

