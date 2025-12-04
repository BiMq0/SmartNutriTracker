using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Usuarios;

namespace SmartNutriTracker.Back.Services.Users;

public interface IUserService
{
    Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync();
    Task<UsuarioRegistroDTO?> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario);
    Task<LoginResponseDTO?> AutenticarUsuarioAsync(LoginDTO loginDTO);
    Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string contrasena);
}
