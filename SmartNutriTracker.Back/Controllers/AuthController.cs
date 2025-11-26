using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Back.Services.Security;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IJwtTokenService _jwtService;

        public AuthController(ApplicationDbContext db, IPasswordHasher<Usuario> passwordHasher, IJwtTokenService jwtService)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var usuario = await _db.Usuarios.Include(u => u.Rol)
                                            .FirstOrDefaultAsync(u => u.Username == model.Username);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

            var token = _jwtService.GenerateToken(usuario);
            return Ok(new { token });
        }
    }
}