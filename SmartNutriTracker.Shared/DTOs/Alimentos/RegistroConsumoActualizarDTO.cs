namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class RegistroConsumoActualizarDTO
    {
        public DateOnly? Fecha { get; set; }
        public TimeOnly? Hora { get; set; }
        public List<RegistroAlimentoItemDTO> AlimentosConsumidos { get; set; } = new();
        
        public RegistroConsumoActualizarDTO() { }
    }
}