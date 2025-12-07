// Shared/DTOs/Habitos/RegistroHabitoActualizarDTO.cs
namespace SmartNutriTracker.Shared.DTOs.Habitos
{
    public class RegistroHabitoActualizarDTO
    {
        public decimal? HorasSueno { get; set; }
        public decimal? HorasActividadFisica { get; set; }
        
        public RegistroHabitoActualizarDTO() { }
    }
}