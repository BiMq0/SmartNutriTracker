using System.ComponentModel.DataAnnotations;
using SmartNutriTracker.Domain.Statics; // AGREGAR este using

namespace SmartNutriTracker.Shared.DTOs.Estudiantes;

public class EstudianteUpdateDTO
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
    public string? NombreCompleto { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; } // En lugar de Edad

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public Sexo? Sexo { get; set; } // Enum en lugar de string

    [Required(ErrorMessage = "El peso es obligatorio.")]
    [Range(1, 500, ErrorMessage = "El peso debe estar entre 1 y 500 kg.")]
    public decimal? Peso { get; set; }

    [Required(ErrorMessage = "La altura es obligatoria.")]
    [Range(0.3, 3.0, ErrorMessage = "La altura debe estar entre 0.3 y 3.0 metros.")]
    public decimal? Altura { get; set; }
}