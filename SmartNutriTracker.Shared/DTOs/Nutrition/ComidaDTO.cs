namespace SmartNutriTracker.Shared.DTOs.Nutrition
{
    public class ComidaDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Calorias { get; set; }
        public decimal Proteinas_g { get; set; }
        public decimal Grasas_g { get; set; }
        public decimal Carbohidratos_g { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
