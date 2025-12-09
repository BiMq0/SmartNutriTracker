namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class TipoComidaDTO
    {
        public int TipoComidaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        
        public TipoComidaDTO() { }
    }
}