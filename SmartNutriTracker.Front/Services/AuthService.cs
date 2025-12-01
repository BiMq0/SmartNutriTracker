using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using SmartNutriTracker.Shared.DTOs.Usuarios;

namespace SmartNutriTracker.Front.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

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
            catch
            {
                return null;
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
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        // Obtener token desde localStorage
        public async Task<string?> ObtenerTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }
    }

    public class UsuarioRegistroResponseDTO
    {
        public string? Mensaje { get; set; }
        public UsuarioRegistroDTO? Usuario { get; set; }
    }
}
