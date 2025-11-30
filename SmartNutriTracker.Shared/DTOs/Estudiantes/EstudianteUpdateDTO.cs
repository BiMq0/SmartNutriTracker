using SmartNutriTracker.Domain.Statics; // AGREGAR este using

namespace SmartNutriTracker.Shared.DTOs.Estudiantes;

public class EstudianteUpdateDTO
{
    public string? NombreCompleto { get; set; }
    public DateTime? FechaNacimiento { get; set; } // En lugar de Edad
    public Sexo? Sexo { get; set; } // Enum en lugar de string
    public decimal? Peso { get; set; }
    public decimal? Altura { get; set; }
}