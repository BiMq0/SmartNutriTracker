using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class RegistroHabito
{
    [Key]
    public int RegistroHabitoId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal HorasSueno { get; set; }
    public decimal HorasActividadFisica { get; set; }
    public int EstudianteId { get; set; }

    public Estudiante? Estudiante { get; set; }
    public ICollection<RegistroAlimento>? RegistroAlimentos { get; set; }

}
