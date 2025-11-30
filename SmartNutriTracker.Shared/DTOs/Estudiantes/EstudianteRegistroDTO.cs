using System;
using System.ComponentModel.DataAnnotations;
using SmartNutriTracker.Domain.Statics;

namespace SmartNutriTracker.Shared.DTOs.Estudiantes;

public class EstudianteRegistroDTO
{
    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string NombreCompleto { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es requerido")]
    public Sexo Sexo { get; set; }

    [Required(ErrorMessage = "El peso es requerido")]
    [Range(1, 500, ErrorMessage = "El peso debe estar entre 1 y 500 kg")]
    public decimal Peso { get; set; }

    [Required(ErrorMessage = "La altura es requerida")]
    [Range(0.5, 2.5, ErrorMessage = "La altura debe estar entre 0.5 y 2.5 metros")]
    public decimal Altura { get; set; }
}