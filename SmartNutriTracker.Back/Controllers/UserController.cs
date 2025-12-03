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

        [Authorize]
        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }


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

        [HttpPost(UsuariosEndpoints.INICIAR_SESION)]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO loginDTO)
        {
            var usuario = await _userService.ValidarCredencialesAsync(loginDTO.Username, loginDTO.Password);
            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            var token = _tokenService.GenerarToken(usuario);

            Response.Cookies.Append("SmartNutriTrackerAuth", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new { mensaje = "Inicio de sesión exitoso.", token });
        }

        [Authorize]
        [HttpPost(UsuariosEndpoints.CERRAR_SESION)]
        public async Task<IActionResult> CerrarSesion()
        {
            Response.Cookies.Delete("SmartNutriTrackerAuth");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { mensaje = "Sesi�n cerrada exitosamente." });
        }
    }
}
