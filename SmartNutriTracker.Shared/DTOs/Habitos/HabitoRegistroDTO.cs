namespace SmartNutriTracker.Shared.DTOs.Habitos
{
    public class HabitoRegistroDTO
    {
        public int RegistroHabitoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasSueno { get; set; }
        public decimal HorasActividadFisica { get; set; }
        public int EstudianteId { get; set; }
        public string? NombreEstudiante { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Altura { get; set; }
        public decimal? IMC { get; set; }
        public string? Sexo { get; set; }
        public List<RegistroAlimentoDetalleDTO>? RegistroAlimentos { get; set; }

        public HabitoRegistroDTO() { }
    }
}