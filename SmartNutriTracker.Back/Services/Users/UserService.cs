using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Tokens;
using SmartNutriTracker.Domain.Models.BaseModels;
using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Services.Users;

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

    public async Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        return usuarios.Select(u => new UsuarioRegistroDTO(u)).ToList();
    }

    public async Task<UsuarioRegistroDTO?> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Username) || string.IsNullOrWhiteSpace(nuevoUsuario.Password))
                return null;

            var existe = await _context.Usuarios.AnyAsync(u => u.Username == nuevoUsuario.Username);
            if (existe) return null;

            var usuario = new Usuario
            {
                Username = nuevoUsuario.Username,
                RolId = nuevoUsuario.RolId
            };

            // Hash the password before storing
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password);

            // Log connection info (masked)
            try
            {
                var conn = _context.Database.GetDbConnection()?.DataSource ?? "(no connection)";
                _logger.LogInformation("[UserService] DB DataSource: {DataSource}", conn);
            }
            catch (Exception exConn)
            {
                _logger.LogWarning(exConn, "[UserService] Error reading DB connection info");
            }

            _context.Usuarios.Add(usuario);

            _logger.LogInformation("[UserService] Guardando usuario '{Username}'...", usuario.Username);
            var cambios = await _context.SaveChangesAsync();
            _logger.LogInformation("[UserService] SaveChanges returned: {Count}", cambios);

            if (cambios <= 0) return null;

            // Ensure Rol navigation is loaded for DTO
            await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();

            _logger.LogInformation("[UserService] Usuario creado con Id: {Id}", usuario.UsuarioId);

            return new UsuarioRegistroDTO(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UserService] Exception en RegistrarUsuarioAsync");
            return null;
        }
    }

    public async Task<LoginResponseDTO?> AutenticarUsuarioAsync(LoginDTO loginDTO)
    {
        var usuario = await _context.Usuarios 
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

        if (usuario == null)
            return null;

        // Verify the password using BCrypt
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