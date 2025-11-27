using SmartNutriTracker.Front.Components;
using SmartNutriTracker.Front.Handlers;
using SmartNutriTracker.Front.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp =>
{
    // HttpClient con soporte para cookies
    var handler = new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer()
    };

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(ApiConfig.HttpsApiUrl),
   Timeout = TimeSpan.FromSeconds(30)
    };
});

// Registrar AuthService
builder.Services.AddScoped<AuthService>();

// Registrar AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

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
