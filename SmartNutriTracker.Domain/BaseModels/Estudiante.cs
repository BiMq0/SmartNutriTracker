using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Estudiante
{
    [Key]
    public int EstudianteId { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public int Edad { get; set; }

    public string Sexo { get; set; } = null!; 

    [Column(TypeName = "decimal(5,2)")]
    public decimal Peso { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal Altura { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal IMC { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public decimal TMB { get; set; }

    public ICollection<RegistroHabito>? RegistroHabitos { get; set; }
}
