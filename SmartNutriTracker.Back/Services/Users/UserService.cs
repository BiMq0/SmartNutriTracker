using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SmartNutriTracker.Shared.DTOs.Usuarios;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Back.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UserService(ApplicationDbContext db, IPasswordHasher<Usuario> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync()
        {
            var usuarios = await _db.Usuarios.Include(u => u.Rol).ToListAsync();
            return usuarios.Select(u => new UsuarioRegistroDTO(u)).ToList();
        }

        public async Task<bool> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Username) || string.IsNullOrWhiteSpace(nuevoUsuario.Password))
                return false;

            var existe = await _db.Usuarios.AnyAsync(u => u.Username == nuevoUsuario.Username);
            if (existe) return false;

            var usuario = new Usuario
            {
                Username = nuevoUsuario.Username,
                RolId = nuevoUsuario.RolId
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, nuevoUsuario.Password);

            _db.Usuarios.Add(usuario);
            var saved = await _db.SaveChangesAsync();
            return saved > 0;
        }
    }
}
