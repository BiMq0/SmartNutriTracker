using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SmartNutriTracker.Shared.Validators;

namespace SmartNutriTracker.Shared.DTOs.Usuarios
{
    public class UsuarioNuevoDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [SecurePassword]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int RolId { get; set; }
    }
}