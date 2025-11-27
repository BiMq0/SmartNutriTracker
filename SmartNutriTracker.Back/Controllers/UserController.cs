using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
                    detalle: $"Excepción: {ex.Message}"
                );
                return StatusCode(500, new { mensaje = "Error interno al registrar usuario." });
            }
        }

        [AllowAnonymous]
        [HttpPost("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var respuesta = await _userService.AutenticarUsuarioAsync(loginDTO);

                if (respuesta == null)
                {
                    await _auditService.LogAsync(
                        accion: "AutenticarUsuario",
                        nivel: "WARNING",
                        detalle: $"Login fallido para: {loginDTO.Correo}"
                    );

                    return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
                }

                await _auditService.LogAsync(
                    accion: "AutenticarUsuario",
                    nivel: "INFO",
                    detalle: $"Login exitoso para: {loginDTO.Correo}"
                );

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                await _auditService.LogAsync(
                    accion: "AutenticarUsuario",
                    nivel: "ERROR",
                    detalle: $"Excepción: {ex.Message}"
                );
                return StatusCode(500, new { mensaje = "Error interno al autenticar usuario." });
            }
        }
    }  
}