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
using SmartNutriTracker.Shared.Endpoints;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Back.Services.Tokens;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("autenticar-usuario")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var respuesta = await _userService.AutenticarUsuarioAsync(loginDTO);
                if (respuesta == null)
                    return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

                // Crear claims para la cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, respuesta.Usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, respuesta.Usuario.Nombre),
                    new Claim(ClaimTypes.Role, respuesta.Usuario.Rol),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new { mensaje = "Autenticación exitosa.", usuario = respuesta.Usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error en la autenticación: {ex.Message}" });
            }
        }

        [HttpGet("obtener-usuarios")]
        [Authorize]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }

        [AllowAnonymous]
        [HttpPost("registrar-usuario")]
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
