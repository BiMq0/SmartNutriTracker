using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels
;

public class TipoComida
{
    [Key]
    public int TipoComidaId { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<RegistroAlimento>? RegistroAlimentos { get; set; }

}
