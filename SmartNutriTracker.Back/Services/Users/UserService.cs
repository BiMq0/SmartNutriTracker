using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Back.Services.Users;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UserService(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        return usuarios.Select(u => new UsuarioRegistroDTO(u)).ToList();
    }

    public async Task<bool> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario)
    {
        if (string.IsNullOrWhiteSpace(nuevoUsuario.Username) || string.IsNullOrWhiteSpace(nuevoUsuario.Password))
            return false;

        var existe = await _context.Usuarios.AnyAsync(u => u.Username == nuevoUsuario.Username);
        if (existe) return false;

        var usuario = new Usuario
        {
            Username = nuevoUsuario.Username,
            RolId = nuevoUsuario.RolId
        };

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password);

        _context.Usuarios.Add(usuario);
        var resultado = await _context.SaveChangesAsync() > 0;
        return resultado;
    }

    public async Task<LoginResponseDTO?> AutenticarUsuarioAsync(LoginDTO loginDTO)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

        if (usuario == null)
            return null;

        bool esValida = BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.PasswordHash);

        if (esValida)
        {
            var token = _tokenService.GenerarToken(usuario);
            return new LoginResponseDTO
            {
                Usuario = new UsuarioRegistroDTO(usuario),
                Token = token
            };
        }

        return null;
    }
}