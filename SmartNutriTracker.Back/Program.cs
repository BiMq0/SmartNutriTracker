using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using SmartNutriTracker.Back.Services.Alimentos;
using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Shared.DTOs.MenuAlimentos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartNutriTracker", Version = "v1" }));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString"))
);
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAllScopes();
builder.Services.AddScoped<IAlimentoService, AlimentoService>();
builder.Services.AddScoped<IAlimentoService, AlimentoService>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

