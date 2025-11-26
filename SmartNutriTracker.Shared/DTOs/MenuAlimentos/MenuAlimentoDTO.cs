using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Shared.DTOs.MenuAlimentos;

public class MenuAlimentoDTO
{
    public int MenuAlimentoId { get; set; }
    public int MenuId { get; set; }
    public AlimentoDTO Alimento { get; set; } = null!;
}
