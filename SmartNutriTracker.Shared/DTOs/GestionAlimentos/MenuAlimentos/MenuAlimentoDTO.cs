namespace SmartNutriTracker.Shared.DTOs.MenuAlimentos
{
    public class MenuAlimentoDTO
    {
        public int MenuAlimentoId { get; set; }
        public int MenuId { get; set; }
        public int AlimentoId { get; set; }

        // 🔥 Estas dos propiedades son NECESARIAS según tu MenuService
        public string NombreAlimento { get; set; } = string.Empty;
        public int Calorias { get; set; }
    }
}
