namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class AlimentoDTO
    {
        public int AlimentoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Calorias { get; set; }

        // Constructor sin parámetros (OBLIGATORIO para deserialización)
        public AlimentoDTO() { }

       
    }
}