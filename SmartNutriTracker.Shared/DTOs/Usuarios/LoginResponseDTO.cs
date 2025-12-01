namespace SmartNutriTracker.Shared.DTOs.Usuarios;

public class LoginResponseDTO
{
    public UsuarioRegistroDTO Usuario { get; set; }
    public string Token { get; set; }
    public string Mensaje { get; set; }
}
