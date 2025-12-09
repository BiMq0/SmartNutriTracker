namespace SmartNutriTracker.Shared.DTOs.Habitos
{
    public class RegistroHabitoDTO
    {
        public int RegistroHabitoId { get; set; }
        public int EstudianteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasSueno { get; set; }
        public decimal HorasActividadFisica { get; set; }
        public List<RegistroAlimentoDetalleDTO>? RegistroAlimentos { get; set; }
        
        public RegistroHabitoDTO() { }
    }
}