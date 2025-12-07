// Shared/DTOs/Habitos/RegistroAlimentoDetalleDTO.cs
namespace SmartNutriTracker.Shared.DTOs.Habitos
{
    public class RegistroAlimentoDetalleDTO
    {
        public int AlimentoId { get; set; }
        public string AlimentoNombre { get; set; } = "";
        public decimal Calorias { get; set; }
        public int TipoComidaId { get; set; }
        public string TipoComidaNombre { get; set; } = "";
        public int? Cantidad { get; set; }
        
        public RegistroAlimentoDetalleDTO() { }
    }
}