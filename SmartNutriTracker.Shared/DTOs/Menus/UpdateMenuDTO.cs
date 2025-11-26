namespace SmartNutriTracker.Shared.DTOs.Menus;

public class UpdateMenuDTO
{
    public DateTime Fecha { get; set; }
    public List<int> AlimentoIds { get; set; } = new();
}
