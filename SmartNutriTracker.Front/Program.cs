using SmartNutriTracker.Front.Components;
using SmartNutriTracker.Front.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp =>
{
    // HttpClient para WASM - navegador maneja cookies automáticamente
    return new HttpClient
    {
        BaseAddress = new Uri(ApiConfig.HttpsApiUrl),
        Timeout = TimeSpan.FromSeconds(30)
    };
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapBlazorHub();

app.Run();