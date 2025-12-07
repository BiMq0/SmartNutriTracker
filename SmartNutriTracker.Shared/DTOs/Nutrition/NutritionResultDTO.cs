namespace SmartNutriTracker.Shared.DTOs.Nutrition
{
    public class NutritionResultDTO
    {
        public decimal TMB { get; set; }
        public decimal CaloriasMantenimiento { get; set; }
        public decimal CaloriasObjetivo { get; set; }

        // Macros en gramos
        public decimal ProteinasGr { get; set; }
        public decimal GrasasGr { get; set; }
        public decimal CarbohidratosGr { get; set; }

        // Macros en kcal (opcional)
        public decimal ProteinasKcal => ProteinasGr * 4m;
        public decimal GrasasKcal => GrasasGr * 9m;
        public decimal CarbohidratosKcal => CarbohidratosGr * 4m;
    }
}
