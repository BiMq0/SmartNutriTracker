namespace SmartNutriTracker.Shared.DTOs.Usuarios
{
    public class TokenResponseDTO
    {
        public string Token { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}