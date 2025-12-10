namespace SmartNutriTracker.Shared.DTOs.Usuarios;

public class LoginResponseDTO
{
    public string? Token { get; set; }
    public UsuarioRegistroDTO Usuario { get; set; }
    public string? Mensaje { get; set; }


}
