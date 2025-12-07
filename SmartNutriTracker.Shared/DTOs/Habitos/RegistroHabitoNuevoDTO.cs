// Shared/DTOs/Habitos/RegistroHabitoNuevoDTO.cs
namespace SmartNutriTracker.Shared.DTOs.Habitos
{
    public class RegistroHabitoNuevoDTO
    {
        public int EstudianteId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public decimal HorasSueno { get; set; }
        public decimal HorasActividadFisica { get; set; }
        
        public RegistroHabitoNuevoDTO() { }
    }
}