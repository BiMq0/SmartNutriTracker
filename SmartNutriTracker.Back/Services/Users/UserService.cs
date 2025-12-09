using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Back.Services.Users;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Usuarios;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ITokenService tokenService, ILogger<UserService> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<UsuarioRegistroDTO?> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario)
    {
        if (string.IsNullOrWhiteSpace(nuevoUsuario.Username) || string.IsNullOrWhiteSpace(nuevoUsuario.Password))
            return null;

        var existe = await _context.Usuarios.AnyAsync(u => u.Username == nuevoUsuario.Username);
        if (existe) return null;

        var usuario = new Usuario
        {
            Username = nuevoUsuario.Username.Trim(),
            RolId = nuevoUsuario.RolId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();

        _logger.LogInformation("Usuario registrado: {Username}", usuario.Username);

        return new UsuarioRegistroDTO(usuario);
    }

    public async Task<LoginResponseDTO?> AutenticarUsuarioAsync(LoginDTO loginDTO)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

        if (usuario == null) return null;

        bool esValida = BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.PasswordHash);
        if (!esValida) return null;

        var token = _tokenService.GenerarToken(usuario);

        return new LoginResponseDTO
        {
            Usuario = new UsuarioRegistroDTO(usuario),
            Token = token
        };
    }

    public async Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        return usuarios.Select(u => new UsuarioRegistroDTO(u)).ToList();
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string contrasena)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == nombreUsuario);
        if (usuario == null) return null;

        bool esValida = BCrypt.Net.BCrypt.Verify(contrasena, usuario.PasswordHash);
        return esValida ? usuario : null;
    }
}
