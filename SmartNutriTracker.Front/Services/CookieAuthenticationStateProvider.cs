using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace SmartNutriTracker.Front.Services
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
{
        private readonly HttpClient _httpClient;

        public CookieAuthenticationStateProvider(HttpClient httpClient)
     {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
     {
         try
        {
         // Hacer una llamada al backend para verificar si el usuario está autenticado
             var response = await _httpClient.GetAsync("api/user/me");

      if (response.IsSuccessStatusCode)
       {
     var content = await response.Content.ReadFromJsonAsync<UserInfo>();
 
         var claims = new[]
             {
       new Claim(ClaimTypes.NameIdentifier, content?.Id ?? ""),
       new Claim(ClaimTypes.Name, content?.Nombre ?? ""),
    new Claim(ClaimTypes.Role, content?.Rol ?? "")
                };

     var identity = new ClaimsIdentity(claims, "ServerAuth");
     var user = new ClaimsPrincipal(identity);

         return new AuthenticationState(user);
            }
        }
          catch
    {
                // Si hay error, el usuario no está autenticado
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public class UserInfo
        {
    public string? Id { get; set; }
         public string? Nombre { get; set; }
    public string? Rol { get; set; }
   }
    }
}
