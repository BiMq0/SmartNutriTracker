using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;


namespace SmartNutriTracker.Front.Services
{
    public class AuthService : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly ILogger<AuthService> _logger;
        private string? _token;
        private const string BASE = UsuariosEndpoints.BASE;

        public AuthService(IHttpClientFactory http, IJSRuntime js, ILogger<AuthService> logger)
        {
            _http = http.CreateClient("ApiClient");
            _js = js;
            _logger = logger;
        }

        // Registro de usuario
        public async Task<UsuarioRegistroResponseDTO?> RegisterAsync(UsuarioNuevoDTO nuevoUsuario)
        {
            _logger.LogInformation("Iniciando registro de usuario: {Username}", nuevoUsuario?.Username);

            try
            {
                var url = BASE + UsuariosEndpoints.REGISTRAR_USUARIO;
                
                _logger.LogDebug("Enviando solicitud de registro a {Url}", url);
                var response = await _http.PostAsJsonAsync(url, nuevoUsuario);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Respuesta exitosa del servidor para registro de {Username}", nuevoUsuario?.Username);
                    // Retorna la respuesta deserializada
                    var resultado = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                    
                    if (resultado?.Usuario != null)
                    {
                        _logger.LogInformation("Usuario {Username} registrado exitosamente con ID {UserId}", 
                            resultado.Usuario.Username, resultado.Usuario.Id);
                    }
                    
                    return resultado;
                }

                _logger.LogWarning("Respuesta no exitosa del servidor. Código: {StatusCode}", response.StatusCode);
                
                // En caso de error, intentar leer el mensaje
                var error = await response.Content.ReadFromJsonAsync<UsuarioRegistroResponseDTO>();
                _logger.LogWarning("Error en registro: {Mensaje}", error?.Mensaje);
                
                return error;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al intentar registrar usuario {Username}", nuevoUsuario?.Username);
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error de conexión: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar respuesta del servidor para usuario {Username}", nuevoUsuario?.Username);
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error en respuesta del servidor: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario {Username}", nuevoUsuario?.Username);
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error: {ex.Message}" };
            }
        }

        // Login de usuario
        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO login)
        {
            _logger.LogInformation("Intento de login para usuario: {Username}", login?.Username);

            try
            {
                var url = BASE + UsuariosEndpoints.INICIAR_SESION;
                
                _logger.LogDebug("Enviando solicitud de login a {Url}", url);
                var response = await _http.PostAsJsonAsync(url, login);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Intento de login fallido. Código de estado: {StatusCode} para usuario {Username}", 
                        response.StatusCode, login?.Username);
                    return null;
                }

                var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                if (resultado != null && !string.IsNullOrEmpty(resultado.Token))
                {
                    _logger.LogInformation("Login exitoso para usuario {Username}. Token obtenido correctamente.", login?.Username);
                    
                    await GuardarTokenAsync(resultado.Token);
                    _logger.LogDebug("Token guardado en localStorage para usuario {Username}", login?.Username);
                    
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }
                else
                {
                    _logger.LogWarning("Respuesta de login inválida para usuario {Username}", login?.Username);
                }

                return resultado;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al intentar login para usuario {Username}", login?.Username);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar respuesta de login para usuario {Username}", login?.Username);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante login para usuario {Username}", login?.Username);
                return null;
            }
        }


        // Guardar token en localStorage
        public async Task GuardarTokenAsync(string token)
        {
            try
            {
                _token = token;
                
                _logger.LogDebug("Iniciando guardado de token en localStorage");
                await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                
                _logger.LogInformation("Token guardado correctamente en localStorage");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No se pudo guardar token - posiblemente durante prerenderizado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al guardar token en localStorage");
            }
        }

        // Obtener token desde localStorage
        public async Task<string?> ObtenerTokenAsync()
        {
            if (_token == null)
            {
                // Verificar si el prerenderizado está habilitado
                if (_js is null)
                {
                    _logger.LogDebug("JSRuntime es nulo - posiblemente durante prerenderizado");
                    return null; // Evitar llamadas durante el prerenderizado
                }

                try
                {
                    _logger.LogDebug("Obteniendo token desde localStorage");
                    _token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
                    
                    if (!string.IsNullOrEmpty(_token))
                    {
                        _logger.LogDebug("Token obtenido correctamente desde localStorage");
                    }
                    else
                    {
                        _logger.LogDebug("No se encontró token en localStorage");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "No se pudo obtener token - posiblemente durante prerenderizado");
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener token desde localStorage");
                    return null;
                }
            }

            return _token;
        }

        public async Task LogoutAsync()
        {
            _logger.LogInformation("Iniciando proceso de logout");

            try
            {
                _token = null;
                
                _logger.LogDebug("Eliminando token de localStorage");
                await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                
                _logger.LogInformation("Token removido correctamente. Notificando cambio de estado de autenticación.");
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error durante logout - posiblemente durante prerenderizado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante logout");
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await ObtenerTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogDebug("Token no disponible - retornando usuario anónimo");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                _logger.LogDebug("Procesando y validando JWT");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                // IMPORTANTE: Establecer el AuthenticationType para que IsAuthenticated sea true
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                
                // Verificar que el token no haya expirado
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    _logger.LogWarning("Token expirado. ValidTo: {ValidTo}, Ahora: {Now}", 
                        jwtToken.ValidTo, DateTime.UtcNow);
                    
                    // Token expirado, limpiar
                    _token = null;
                    await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                    
                    _logger.LogInformation("Token expirado removido de localStorage");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var user = new ClaimsPrincipal(identity);
                var expiresIn = jwtToken.ValidTo - DateTime.UtcNow;
                
                _logger.LogInformation("Autenticación válida. Usuario: {Username}, Expira en: {ExpiresIn:g}", 
                    jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "desconocido",
                    expiresIn);
                
                return new AuthenticationState(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error al procesar JWT - Token inválido");
                _token = null;
                await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error durante validación de JWT - posiblemente durante prerenderizado");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al validar token JWT");
                _token = null;
                
                try
                {
                    await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Error al limpiar localStorage después de error en JWT");
                }
                
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // Método para verificar si existe token
        public bool ExistsToken()
        {
            bool exists = !string.IsNullOrEmpty(_token);
            _logger.LogDebug("Verificando existencia de token: {Exists}", exists);
            return exists;
        }
    }

    public class UsuarioRegistroResponseDTO
    {
        public string? Mensaje { get; set; }
        public UsuarioRegistroDTO? Usuario { get; set; }
    }
}
