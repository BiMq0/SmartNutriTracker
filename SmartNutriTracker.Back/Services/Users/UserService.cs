using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Back.Database;
using Microsoft.EntityFrameworkCore;
namespace SmartNutriTracker.Back.Services.Users;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
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
}
