namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class CreateAlimentoDTO
    {
        public string Nombre { get; set; } = null!;
        public int Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carbohidratos { get; set; }
        public decimal Grasas { get; set; }
    }
}
