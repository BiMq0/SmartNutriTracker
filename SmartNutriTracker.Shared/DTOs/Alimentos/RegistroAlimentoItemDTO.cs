namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class RegistroAlimentoItemDTO
    {
        public int AlimentoId { get; set; }
        public int TipoComidaId { get; set; }
        public int Cantidad { get; set; }
        
        public RegistroAlimentoItemDTO() { }
    }
}