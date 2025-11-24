namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Menu
{
    public int MenuId { get; set; }
    public DateTime Fecha { get; set; }

    public ICollection<MenuAlimento>? MenuAlimentos { get; set; }
}
