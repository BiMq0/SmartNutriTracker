using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartNutriTracker.Back.Services.Audit;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(UsuariosEndpoints.BASE)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuditService _auditService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService, 
            IAuditService auditService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _auditService = auditService;
            _logger = logger;
        }

        // Obtener usuarios
        [Authorize]
        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<ActionResult<List<UsuarioRegistroDTO>>> ObtenerUsuarios()
        {
            var usuarioActual = User.Identity?.Name ?? "desconocido";
            
            _logger.LogInformation("Solicitud de obtención de usuarios iniciada por: {UsuarioActual}", usuarioActual);

            try
            {
                var usuarios = await _userService.ObtenerUsuariosAsync();

                if (usuarios.Count == 0)
                {
                    _logger.LogWarning("Búsqueda de usuarios retornó lista vacía. Solicitante: {UsuarioActual}", usuarioActual);
                    await _auditService.LogAsync("OBTENER_USUARIOS", "INFO", "Búsqueda de usuarios - lista vacía");
                }
                else
                {
                    _logger.LogInformation("Se obtuvieron {Cantidad} usuarios correctamente. Solicitante: {UsuarioActual}", 
                        usuarios.Count, usuarioActual);
                    await _auditService.LogAsync("OBTENER_USUARIOS", "INFO", 
                        $"Se obtuvieron {usuarios.Count} usuarios exitosamente");
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios. Solicitante: {UsuarioActual}. Error: {Mensaje}", 
                    usuarioActual, ex.Message);
                await _auditService.LogAsync("OBTENER_USUARIOS", "ERROR", 
                    $"Error al obtener usuarios: {ex.Message}");
                
                return StatusCode(500, new { mensaje = $"Error al obtener usuarios: {ex.Message}" });
            }
        }

        // Registrar usuario
        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioNuevoDTO nuevoUsuario)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
            
            _logger.LogInformation("Solicitud de registro de usuario iniciada. Usuario: {Username}, IP: {IPAddress}", 
                nuevoUsuario?.Username, ipAddress);

            try
            {
                // Validar el modelo (Data Annotations)
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    var errorMessages = string.Join(" | ", errors.Select(e => e.ErrorMessage));
                    
                    _logger.LogWarning("Validación fallida en registro. Usuario: {Username}, IP: {IPAddress}, Errores: {Errors}", 
                        nuevoUsuario?.Username, ipAddress, errorMessages);
                    
                    await _auditService.LogAsync("REGISTRAR_USUARIO", "WARNING", 
                        $"Validación fallida - Usuario: {nuevoUsuario?.Username} - Errores: {errorMessages}");
                    
                    return BadRequest(new 
                    { 
                        mensaje = "Datos inválidos.",
                        errores = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                    });
                }

                _logger.LogDebug("Procesando registro para usuario: {Username}. Rol solicitado: {RolId}", 
                    nuevoUsuario.Username, nuevoUsuario.RolId);

                var creado = await _userService.RegistrarUsuarioAsync(nuevoUsuario!);

                if (creado != null)
                {
                    _logger.LogInformation("Usuario {Username} registrado exitosamente. ID: {UserId}, Rol: {RolId}, IP: {IPAddress}", 
                        creado.Username, creado.Id, creado.RolId, ipAddress);
                    
                    await _auditService.LogAsync("REGISTRAR_USUARIO", "INFO", 
                        $"Usuario {creado.Username} (ID: {creado.Id}) registrado exitosamente. Rol: {creado.RolId}");
                    
                    return Ok(new
                    {
                        mensaje = "Usuario registrado exitosamente.",
                        usuario = creado
                    });
                }

                _logger.LogWarning("Falló el registro del usuario {Username}. Posible duplicado. IP: {IPAddress}", 
                    nuevoUsuario?.Username, ipAddress);
                
                await _auditService.LogAsync("REGISTRAR_USUARIO", "WARNING", 
                    $"Falló el registro de {nuevoUsuario?.Username} - usuario ya existe");
                
                return BadRequest(new { mensaje = "El usuario ya existe. Elige otro nombre de usuario." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido al registrar usuario {Username}. IP: {IPAddress}", 
                    nuevoUsuario?.Username, ipAddress);
                
                await _auditService.LogAsync("REGISTRAR_USUARIO", "WARNING", 
                    $"Argumento inválido para {nuevoUsuario?.Username}: {ex.Message}");
                
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al registrar usuario {Username}. IP: {IPAddress}. Error: {Mensaje}", 
                    nuevoUsuario?.Username, ipAddress, ex.Message);
                
                await _auditService.LogAsync("REGISTRAR_USUARIO", "ERROR", 
                    $"Excepción al registrar {nuevoUsuario?.Username}: {ex.Message}");
                
                return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
            }
        }

        // Iniciar sesión
        [HttpPost(UsuariosEndpoints.INICIAR_SESION)]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
            
            _logger.LogInformation("Intento de inicio de sesión. Usuario: {Username}, IP: {IPAddress}", 
                loginDTO?.Username, ipAddress);

            try
            {
                // Validar el modelo (Data Annotations)
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    var errorMessages = string.Join(" | ", errors.Select(e => e.ErrorMessage));
                    
                    _logger.LogWarning("Validación fallida en login. Usuario: {Username}, IP: {IPAddress}, Errores: {Errors}", 
                        loginDTO?.Username, ipAddress, errorMessages);
                    
                    await _auditService.LogAsync("INICIAR_SESION", "WARNING", 
                        $"Intento de login con datos inválidos - Usuario: {loginDTO?.Username} - IP: {ipAddress}");
                    
                    return BadRequest(new { mensaje = "Usuario o contraseña inválidos." });
                }

                _logger.LogDebug("Autenticando usuario: {Username}", loginDTO.Username);

                var resultado = await _userService.AutenticarUsuarioAsync(loginDTO!);

                if (resultado == null)
                {
                    _logger.LogWarning("Autenticación fallida - credenciales inválidas. Usuario: {Username}, IP: {IPAddress}", 
                        loginDTO?.Username, ipAddress);
                    
                    await _auditService.LogAsync("INICIAR_SESION", "WARNING", 
                        $"Intento fallido de login - Usuario: {loginDTO?.Username} - IP: {ipAddress}");
                    
                    return Unauthorized(new { mensaje = "Credenciales inválidas." });
                }

                Response.Cookies.Append("SmartNutriTrackerAuth", resultado.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Cambiar a true en producción
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                var tokenExpiresAt = DateTimeOffset.UtcNow.AddHours(1);

                _logger.LogInformation("Autenticación exitosa. Usuario: {Username}, ID: {UserId}, Rol: {RolId}, IP: {IPAddress}, Token expira: {ExpiresAt}", 
                    resultado.Usuario.Username, resultado.Usuario.Id, resultado.Usuario.RolId, ipAddress, tokenExpiresAt);
                
                await _auditService.LogAsync("INICIAR_SESION", "INFO", 
                    $"Login exitoso - Usuario: {resultado.Usuario.Username} (ID: {resultado.Usuario.Id}) - Rol: {resultado.Usuario.RolId} - IP: {ipAddress}");

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    token = resultado.Token,
                    usuario = resultado.Usuario
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al iniciar sesión para usuario {Username}. IP: {IPAddress}. Error: {Mensaje}", 
                    loginDTO?.Username, ipAddress, ex.Message);
                
                await _auditService.LogAsync("INICIAR_SESION", "ERROR", 
                    $"Excepción en login: {ex.Message}");
                
                return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
            }
        }

        // Cerrar sesión
        [Authorize]
        [HttpPost(UsuariosEndpoints.CERRAR_SESION)]
        public async Task<IActionResult> CerrarSesion()
        {
            var usuarioActual = User.Identity?.Name ?? "desconocido";
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
            
            _logger.LogInformation("Solicitud de cierre de sesión. Usuario: {Usuario}, IP: {IPAddress}", 
                usuarioActual, ipAddress);

            try
            {
                Response.Cookies.Delete("SmartNutriTrackerAuth");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("Sesión cerrada correctamente. Usuario: {Usuario}, IP: {IPAddress}", 
                    usuarioActual, ipAddress);
                
                await _auditService.LogAsync("CERRAR_SESION", "INFO", 
                    $"Sesión cerrada exitosamente - Usuario: {usuarioActual} - IP: {ipAddress}");
                
                return Ok(new { mensaje = "Sesión cerrada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión. Usuario: {Usuario}, IP: {IPAddress}. Error: {Mensaje}", 
                    usuarioActual, ipAddress, ex.Message);
                
                await _auditService.LogAsync("CERRAR_SESION", "ERROR", 
                    $"Error al cerrar sesión: {ex.Message}");
                
                return StatusCode(500, new { mensaje = $"Error al cerrar sesión: {ex.Message}" });
            }
        }

        [HttpGet("test-logger")]
        public IActionResult TestLogger()
        {
            _logger.LogInformation("TestLogger ejecutado - Verificación de nivel Information");
            _logger.LogWarning("TestLogger ejecutado - Verificación de nivel Warning");
            _logger.LogError("TestLogger ejecutado - Verificación de nivel Error");
            _logger.LogDebug("TestLogger ejecutado - Verificación de nivel Debug");

            return Ok("Logger funcionando correctamente. Verifica los logs en la consola o archivos de configuración.");
        }
    }
}
