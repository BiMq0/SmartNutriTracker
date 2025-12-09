namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class RegistroConsumoDTO
    {
        public int RegistroHabitoId { get; set; }
        public int EstudianteId { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleAlimentoDTO> AlimentosConsumidos { get; set; } = new();
        
        public RegistroConsumoDTO() { }
    }

    public class DetalleAlimentoDTO
    {
        public int AlimentoId { get; set; }
        public string AlimentoNombre { get; set; } = "";
        public decimal Calorias { get; set; }
        public int TipoComidaId { get; set; }
        public string TipoComidaNombre { get; set; } = "";
        public int Cantidad { get; set; }
        
        public DetalleAlimentoDTO() { }
    }
}