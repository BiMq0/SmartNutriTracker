using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(UsuariosEndpoints.BASE)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // Obtener usuarios
        [Authorize]
        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<ActionResult<List<UsuarioRegistroDTO>>> ObtenerUsuarios()
        {
            var usuarios = await _userService.ObtenerUsuariosAsync();
            return Ok(usuarios); // Devuelve 200 con la lista de usuarios
        }

        // Registrar usuario
        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioNuevoDTO nuevoUsuario)
        {
            try
            {
                var creado = await _userService.RegistrarUsuarioAsync(nuevoUsuario);

                if (creado != null)
                {
                    return Ok(new
                    {
                        mensaje = "Usuario registrado exitosamente.",
                        usuario = creado
                    });
                }

                return BadRequest(new
                {
                    mensaje = "Error al registrar el usuario."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error: {ex.Message}" });
            }
        }

        // Iniciar sesión
        [HttpPost(UsuariosEndpoints.INICIAR_SESION)]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            var usuario = await _userService.ValidarCredencialesAsync(loginDTO.Username, loginDTO.Password);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            var token = _tokenService.GenerarToken(usuario);

            // Guardar token en cookie (opcional)
            Response.Cookies.Append("SmartNutriTrackerAuth", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // CAMBIA A true EN PRODUCCIÓN
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new
            {
                mensaje = "Inicio de sesión exitoso.",
                token,
                usuario = new UsuarioRegistroDTO(usuario)
            });
        }

        // Cerrar sesión
        [Authorize]
        [HttpPost(UsuariosEndpoints.CERRAR_SESION)]
        public async Task<IActionResult> CerrarSesion()
        {
            Response.Cookies.Delete("SmartNutriTrackerAuth");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { mensaje = "Sesión cerrada exitosamente." });
        }
    }
}
