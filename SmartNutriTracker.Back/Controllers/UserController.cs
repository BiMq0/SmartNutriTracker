using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Shared.Endpoints;

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

        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }

        [HttpPost(UsuariosEndpoints.REGISTRAR_USUARIO)]
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
    }
}