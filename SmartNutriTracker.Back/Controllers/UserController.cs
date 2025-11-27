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
using SmartNutriTracker.Back.Services.Audit;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuditService _auditService;

        public UserController(IUserService userService, IAuditService auditService)
        {
            _userService = userService;
            _auditService = auditService;
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
            try
            {
                bool resultado = await _userService.RegistrarUsuarioAsync(nuevoUsuario);

                if (resultado)
                {
                    await _auditService.LogAsync(
                        accion: "RegistrarUsuario",
                        nivel: "INFO",
                        detalle: $"Usuario registrado: {nuevoUsuario.Correo}"
                    );

                    return Ok(new { mensaje = "Usuario registrado exitosamente." });
                }
                else
                {
                    await _auditService.LogAsync(
                        accion: "RegistrarUsuario",
                        nivel: "WARNING",
                        detalle: $"Error al registrar usuario: {nuevoUsuario.Correo}"
                    );

                    return BadRequest(new { mensaje = "Error al registrar el usuario." });
                }
            }
            catch (Exception ex)
            {
                await _auditService.LogAsync(
                    accion: "RegistrarUsuario",
                    nivel: "ERROR",
                    detalle: $"Excepci�n: {ex.Message}"
                );
                return StatusCode(500, new { mensaje = "Error interno al registrar usuario." });
            }
        }

        [AllowAnonymous]
        [HttpPost("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginDTO loginDTO)
        {
            var respuesta = await _userService.AutenticarUsuarioAsync(loginDTO);
            if (respuesta == null)
                return Unauthorized(new { mensaje = "Usuario o contrase�a incorrectos." });

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

            return Ok(new { mensaje = "Autenticaci�n exitosa.", usuario = respuesta.Usuario });
        }

        [Authorize]
        [HttpPost("CerrarSesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { mensaje = "Sesi�n cerrada exitosamente." });
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