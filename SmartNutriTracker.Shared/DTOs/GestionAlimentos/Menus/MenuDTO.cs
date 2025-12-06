using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Shared.DTOs.Menus;

public class MenuDTO
{
    public int MenuId { get; set; }
    public DateTime Fecha { get; set; }
    public List<AlimentoDTO> Alimentos { get; set; } = new();

    public int CaloriasTotales => Alimentos.Sum(a => a.Calorias);
    public decimal ProteinasTotales => Alimentos.Sum(a => a.Proteinas);
    public decimal CarbohidratosTotales => Alimentos.Sum(a => a.Carbohidratos);
    public decimal GrasasTotales => Alimentos.Sum(a => a.Grasas);
}
