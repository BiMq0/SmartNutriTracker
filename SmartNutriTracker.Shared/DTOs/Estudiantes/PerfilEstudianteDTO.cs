using SmartNutriTracker.Domain.Statics;

namespace SmartNutriTracker.Shared.DTOs.Estudiantes;

public class PerfilEstudianteDTO
{
    public int EstudianteId { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public int Edad { get; set; }
    public Sexo Sexo { get; set; } = Sexo.Varon;
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public decimal IMC { get; set; }
    public decimal TMB { get; set; }
}
