using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Tokens;
using Microsoft.EntityFrameworkCore;

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
        var usuariosBase = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        var listaUsuarios = usuariosBase.Select(u => new UsuarioRegistroDTO(u)).ToList();
        return listaUsuarios;
    }

    public async Task<bool> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario)
    {
        _context.Usuarios.Add(new Usuario
        {
            Username = nuevoUsuario.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password),
            RolId = nuevoUsuario.RolId
        });

        var resultado = await _context.SaveChangesAsync() > 0;
        return resultado;
    }

    public async Task<LoginResponseDTO?> AutenticarUsuarioAsync(LoginDTO loginDTO)
    {
        // 1. Buscar el usuario por username
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

        // 2. Si no existe, retornar null
        if (usuario == null)
            return null;

        // 3. Validar contraseña con BCrypt
        bool esValida = BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.PasswordHash);
        
        // 4. Si contraseña es válida, generar token y retornar respuesta
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
