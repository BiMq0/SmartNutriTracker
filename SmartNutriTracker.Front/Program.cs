using SmartNutriTracker.Front.Components;
using SmartNutriTracker.Front.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// ✅ TU CONFIGURACIÓN ORIGINAL DE HttpClient - CONSERVADA
builder.Services.AddScoped(sp =>
{
    // HttpClient para WASM - navegador maneja cookies automáticamente
    return new HttpClient
    {
        BaseAddress = new Uri(ApiConfig.HttpsApiUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };
});

// ✅ NUEVO: Agregar servicios adicionales que necesitas
builder.Services.AddControllers(); // Para API controllers si los tienes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// ✅ NUEVO: Agregar CORS - IMPORTANTE para comunicación Backend-Frontend
app.UseCors("AllowAll");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapBlazorHub();

// ✅ NUEVO: Mapear controllers si los usas
app.MapControllers();

app.Run();