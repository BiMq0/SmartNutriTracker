using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class TipoAccion
{
    [Key]
    public int TipoAccionId { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<Log>? Logs { get; set; }

}
