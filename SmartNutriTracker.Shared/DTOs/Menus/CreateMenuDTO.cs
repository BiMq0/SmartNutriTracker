namespace SmartNutriTracker.Shared.DTOs.Menus;

public class CreateMenuDTO
{
    public DateTime Fecha { get; set; }
    public List<int> AlimentoIds { get; set; } = new();
}
