using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SmartNutriTracker.Shared.DTOs.Usuarios;


namespace SmartNutriTracker.Front.Services
{
    public class AuthService : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private string? _token;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // Registro de usuario
        public async Task<UsuarioRegistroResponseDTO?> RegisterAsync(UsuarioNuevoDTO nuevoUsuario)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/user/RegistrarUsuario", nuevoUsuario);

                if (response.IsSuccessStatusCode)
                {
                    // Retorna la respuesta deserializada
                    var resultado = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                    return resultado;
                }

                // En caso de error, intentar leer el mensaje
                var error = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                return error;
            }
            catch (Exception ex)
            {
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error: {ex.Message}" };
            }
        }

        // Login de usuario
        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO login)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/user/Login", login);

                if (!response.IsSuccessStatusCode) return null;

                var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                if (resultado != null && !string.IsNullOrEmpty(resultado.Token))
                {
                    // Guardar token en localStorage
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

        // Obtener token desde localStorage
        public async Task<string?> ObtenerTokenAsync()
        {
            if (_token == null)
            {
                // Verificar si el prerenderizado está habilitado
                if (_js is null)
                {
                    return null; // Evitar llamadas durante el prerenderizado
                }

                try
                {
                    _token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
                }
                catch (InvalidOperationException)
                {
                    // Manejar el caso en que la llamada se realiza durante el prerenderizado
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
    }

    public class UsuarioRegistroResponseDTO
    {
        public string? Mensaje { get; set; }
        public UsuarioRegistroDTO? Usuario { get; set; }
    }
}
