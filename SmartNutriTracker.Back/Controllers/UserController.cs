csharp SmartNutriTracker.Back\Controllers\UserController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;
using SmartNutriTracker.Back.Services.Users;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ObtenerUsuarios")]
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

        [AllowAnonymous]
        [HttpPost("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginDTO loginDTO)
        {
            var respuesta = await _userService.AutenticarUsuarioAsync(loginDTO);
            if (respuesta == null)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
            return Ok(respuesta);
        }
    }
}