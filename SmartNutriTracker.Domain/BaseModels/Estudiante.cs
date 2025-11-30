using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartNutriTracker.Domain.Statics;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Estudiante
{
    [Key]
    public int EstudianteId { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Peso { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Altura { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal IMC { get; set; }
    [Column(TypeName = "decimal(5,2)")]

    [NotMapped]
    public decimal TMB =>
        10 * Peso + 6.25m * Altura - 5 * Edad + (Sexo == Sexo.Varon ? 5 : -161);

    [NotMapped]
    public int Edad =>
        DateTime.Now.Year - FechaNacimiento.Year -
        (DateTime.Now.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);

    public Sexo Sexo { get; set; }
    public string SexoString =>
        Sexo switch
        {
            Sexo.Varon => "Varón",
            Sexo.Mujer => "Mujer",
            _ => "Desconocido"
        };

    public ICollection<RegistroHabito>? RegistroHabitos { get; set; }


}