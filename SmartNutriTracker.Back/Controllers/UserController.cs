
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
    [Route("api/[controller]")]
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
            var respuesta = await _userService.AutenticarUsuarioAsync(loginDTO);
            
            if (respuesta == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // 2. Generar token
            var token = _tokenService.GenerarToken(respuesta.Usuario);

            // 3. Guardar token en cookie HttpOnly + Secure
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,        // No accesible desde JavaScript (más seguro contra XSS)
                Secure = true,          // Solo se envía por HTTPS
                SameSite = SameSiteMode.None,  // Necesario si frontend en distinto origen
                Expires = DateTime.UtcNow.AddHours(24)
            };

            Response.Cookies.Append("SmartNutriTrackerAuth", token, cookieOptions);

            // 4. Retornar solo confirmación (sin datos sensibles en el cuerpo)
            return Ok(new { mensaje = "Login exitoso" });
        }

        [HttpGet("ObtenerUsuarios")]
        [Authorize]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioNuevoDTO nuevoUsuario)
        {
            bool resultado = await _userService.RegistrarUsuarioAsync(nuevoUsuario);
            if (resultado)
            {
                return Ok(new { mensaje = "Usuario registrado exitosamente." });
            }
            else
            {
                return BadRequest(new { mensaje = "Error al registrar el usuario." });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Eliminar la cookie de autenticación
            Response.Cookies.Delete("SmartNutriTrackerAuth");
            return Ok(new { mensaje = "Logout exitoso" });
        }

        [AllowAnonymous]
        [HttpPost("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginDTO loginDTO)
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