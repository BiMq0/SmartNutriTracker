namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class RegistroConsumoNuevoDTO
    {
        public int EstudianteId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public List<RegistroAlimentoItemDTO> AlimentosConsumidos { get; set; } = new();
        
        public RegistroConsumoNuevoDTO() { }
    }
}