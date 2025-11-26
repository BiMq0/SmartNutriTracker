using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Shared.Endpoints;
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
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // 2. Obtener datos del usuario para generar token
            var usuario = await _userService.ObtenerUsuariosAsync();
            var usuarioCompleto = usuario.FirstOrDefault(u => u.Nombre == usuarioAutenticado.Nombre);

            // 3. Generar token
            // NOTA: Aquí necesitamos el usuario completo con el Rol cargado
            var usuarioConRol = _userService.ObtenerUsuariosAsync().Result
                .FirstOrDefault(u => u.Nombre == usuarioAutenticado.Nombre);

            // Mejor solución: modificar el método para retornar el Usuario completo
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