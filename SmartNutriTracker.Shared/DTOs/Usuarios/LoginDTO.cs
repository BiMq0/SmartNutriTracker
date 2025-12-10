using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Usuarios
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe ser válido.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "La contraseña debe ser válida.")]
        public string Password { get; set; } = null!;
    }
}