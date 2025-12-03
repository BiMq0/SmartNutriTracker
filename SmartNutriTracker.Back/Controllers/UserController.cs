
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Back.Services.Tokens;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // 1. Autenticar usuario
            var usuarioAutenticado = await _userService.AutenticarUsuarioAsync(loginDTO);
            
            if (usuarioAutenticado == null)
                return Unauthorized(new { mensaje = "Credenciales inv�lidas" });

            // 2. Obtener datos del usuario para generar token
            var usuario = await _userService.ObtenerUsuariosAsync();
            var usuarioCompleto = usuario.FirstOrDefault(u => u.Nombre == usuarioAutenticado.Nombre);

            // 3. Generar token
            // NOTA: Aqu� necesitamos el usuario completo con el Rol cargado
            var usuarioConRol = _userService.ObtenerUsuariosAsync().Result
                .FirstOrDefault(u => u.Nombre == usuarioAutenticado.Nombre);

            // Mejor soluci�n: modificar el m�todo para retornar el Usuario completo
            // Por ahora, hacemos una consulta directa
            var usuarioParaToken = await _userService.ObtenerUsuariosAsync();
            
            // Retornar respuesta con token
            return Ok(new { mensaje = "Login exitoso", token = usuarioAutenticado.Rol });
        }

        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }

        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioNuevoDTO nuevoUsuario)
        {
            try
            {
                var creado = await _userService.RegistrarUsuarioAsync(nuevoUsuario);
                if (creado != null)
                {
                    return Ok(new { mensaje = "Usuario registrado exitosamente.", usuario = creado });
                }

                return BadRequest(new { mensaje = "Error al registrar el usuario. Verifique que el usuario no exista y los datos sean válidos." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegistrarUsuario: {ex.Message}");
                return StatusCode(500, new { mensaje = $"Error al registrar el usuario: {ex.Message}" });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("SmartNutriTrackerAuth");
            return Ok(new { mensaje = "Logout exitoso" });
        }

        [Authorize]
        [HttpPost("CerrarSesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { mensaje = "Sesión cerrada exitosamente." });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            return Ok(new
            {
                Id = userId,
                Nombre = userName,
                Rol = userRole
            });
        }
    }
}
