using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Front.Services
{
    public class AuthService : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private string? _token;
        private const string BASE = UsuariosEndpoints.BASE;

        public AuthService(IHttpClientFactory http, IJSRuntime js)
        {
            _http = http.CreateClient("ApiClient");
            _js = js;
        }

        public async Task<UsuarioRegistroResponseDTO?> RegisterAsync(UsuarioNuevoDTO nuevoUsuario)
        {
            try
            {
                var url = BASE + UsuariosEndpoints.REGISTRAR_USUARIO;
                var response = await _http.PostAsJsonAsync(url, nuevoUsuario);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                    return resultado;
                }

                var error = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                return error;
            }
            catch (Exception ex)
            {
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO login)
        {
            try
            {
                var url = BASE + UsuariosEndpoints.INICIAR_SESION;
                var response = await _http.PostAsJsonAsync(url, login);

                if (!response.IsSuccessStatusCode) return null;

                var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                if (resultado != null && !string.IsNullOrEmpty(resultado.Token))
                {
                    await GuardarTokenAsync(resultado.Token);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }

                return resultado;
            }
            catch
            {
                return null;
            }
        }

        // Guardar token en localStorage
        public async Task GuardarTokenAsync(string token)
        {
            _token = token;
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task<string?> ObtenerTokenAsync()
        {
            if (_token == null)
            {
                if (_js is null)
                {
                    return null; 
                }

                try
                {
                    _token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }

            return _token;
        }

        public async Task LogoutAsync()
        {
            _token = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await ObtenerTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public bool ExistsToken()
        {
            // Versión pre-produccion (funcional)
            return !string.IsNullOrEmpty(_token); // Hardcodeado temporalmente para simular que no hay token
            
            // Mi Version (esta en comentario)
            // return false;
        }
    }

    public class UsuarioRegistroResponseDTO
    {
        public string? Mensaje { get; set; }
        public UsuarioRegistroDTO? Usuario { get; set; }
    }
}