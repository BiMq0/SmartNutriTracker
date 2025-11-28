using System.Net.Http.Json;
using SmartNutriTracker.Shared.DTOs.Usuarios;

namespace SmartNutriTracker.Front.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("ApiClient");
        }

        public async Task<AuthResponse?> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user/autenticar-usuario", loginDTO);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en login: {response.StatusCode} - {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en LoginAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<AuthResponse?> RegisterAsync(UsuarioNuevoDTO nuevoUsuario)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user/registrar-usuario", nuevoUsuario);

                var contentString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Try to read the expected JSON (mensaje + usuario)
                    try
                    {
                        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                        return result ?? new AuthResponse { Mensaje = "Usuario registrado (sin contenido devuelto)." };
                    }
                    catch
                    {
                        return new AuthResponse { Mensaje = "Usuario registrado." };
                    }
                }
                else
                {
                    // Return error message from server (if any)
                    return new AuthResponse { Mensaje = contentString };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegisterAsync: {ex.Message}");
                return new AuthResponse { Mensaje = ex.Message };
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("api/user/cerrar-sesion", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar sesión: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UsuarioRegistroDTO>> GetUsuariosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user/obtener-usuarios");
                
                if (response.IsSuccessStatusCode)
                {
                    var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioRegistroDTO>>();
                    return usuarios ?? new List<UsuarioRegistroDTO>();
                }

                return new List<UsuarioRegistroDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<UsuarioRegistroDTO>();
            }
        }
    }

    public class AuthResponse
    {
        public string Mensaje { get; set; } = string.Empty;
        public UsuarioRegistroDTO? Usuario { get; set; }
    }
}
