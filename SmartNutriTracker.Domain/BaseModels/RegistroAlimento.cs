using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class RegistroAlimento
{
    [Key]
    public int RegistroAlimentoId { get; set; }
    public int RegistroHabitoId { get; set; }
    public int AlimentoId { get; set; }
    public int TipoComidaId { get; set; }
    public int? Cantidad { get; set; }

    public RegistroHabito? RegistroHabito { get; set; }
    public Alimento? Alimento { get; set; }
    public TipoComida? TipoComida { get; set; }
}
