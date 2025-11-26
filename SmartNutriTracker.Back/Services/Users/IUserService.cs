using SmartNutriTracker.Shared.DTOs.Usuarios;

namespace SmartNutriTracker.Back.Services.Users;

public interface IUserService
{
    Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync();
    Task<bool> RegistrarUsuarioAsync(UsuarioNuevoDTO nuevoUsuario);
}
