using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(UsuariosEndpoints.BASE)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        // Obtener usuarios
        [Authorize]
        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<ActionResult<List<UsuarioRegistroDTO>>> ObtenerUsuarios()
        {
            _logger.LogInformation("Inicio de la solicitud: ObtenerUsuarios");

            try
            {
                var usuarios = await _userService.ObtenerUsuariosAsync();

                if (usuarios.Count == 0)
                {
                    _logger.LogWarning("No se encontraron usuarios en la base de datos.");
                }
                else
                {
                    _logger.LogInformation("Se obtuvieron {Cantidad} usuarios correctamente.", usuarios.Count);
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios.");
                return StatusCode(500, new { mensaje = $"Error al obtener usuarios: {ex.Message}" });
            }
        }

        // Registrar usuario
        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioNuevoDTO nuevoUsuario)
        {
            _logger.LogInformation("Inicio de la solicitud: RegistrarUsuario para {Username}", nuevoUsuario.Username);

            try
            {
                var creado = await _userService.RegistrarUsuarioAsync(nuevoUsuario);

                if (creado != null)
                {
                    _logger.LogInformation("Usuario {Username} registrado correctamente.", creado.Username);
                    return Ok(new
                    {
                        mensaje = "Usuario registrado exitosamente.",
                        usuario = creado
                    });
                }

                _logger.LogWarning("Error al registrar el usuario {Username}.", nuevoUsuario.Username);
                return BadRequest(new { mensaje = "Error al registrar el usuario." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al registrar el usuario {Username}.", nuevoUsuario.Username);
                return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
            }
        }

        // Iniciar sesión
        [HttpPost(UsuariosEndpoints.INICIAR_SESION)]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Intento de inicio de sesión para {Username}", loginDTO.Username);

            try
            {
                var usuario = await _userService.ValidarCredencialesAsync(loginDTO.Username, loginDTO.Password);

                if (usuario == null)
                {
                    _logger.LogWarning("Inicio de sesión fallido para {Username}", loginDTO.Username);
                    return Unauthorized(new { mensaje = "Credenciales inválidas." });
                }

                var token = _tokenService.GenerarToken(usuario);

                Response.Cookies.Append("SmartNutriTrackerAuth", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Cambiar a true en producción
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                _logger.LogInformation("Inicio de sesión exitoso para {Username}", loginDTO.Username);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    token,
                    usuario = new UsuarioRegistroDTO(usuario)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión para {Username}", loginDTO.Username);
                return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
            }
        }

        // Cerrar sesión
        [Authorize]
        [HttpPost(UsuariosEndpoints.CERRAR_SESION)]
        public async Task<IActionResult> CerrarSesion()
        {
            _logger.LogInformation("Solicitud de cierre de sesión iniciada.");

            try
            {
                Response.Cookies.Delete("SmartNutriTrackerAuth");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("Sesión cerrada correctamente.");
                return Ok(new { mensaje = "Sesión cerrada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión.");
                return StatusCode(500, new { mensaje = $"Error al cerrar sesión: {ex.Message}" });
            }
        }

        [HttpGet("test-logger")]
        public IActionResult TestLogger()
        {
            _logger.LogInformation("Logger de información: TestLogger ejecutado correctamente.");
            _logger.LogWarning("Logger de advertencia: Esto es una advertencia de prueba.");
            _logger.LogError("Logger de error: Esto es un error de prueba.");

            return Ok("Logger funcionando. Revisa la consola o los archivos de logs.");
        }
    }
}
