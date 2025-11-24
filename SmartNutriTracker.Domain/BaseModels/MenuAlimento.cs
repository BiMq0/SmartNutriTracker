using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class MenuAlimento
{
    [Key]
    public int MenuAlimentoId { get; set; }
    public int MenuId { get; set; }
    public int AlimentoId { get; set; }

    public Menu? Menu { get; set; }
    public Alimento? Alimento { get; set; }
}
