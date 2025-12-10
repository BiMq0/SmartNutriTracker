namespace SmartNutriTracker.Shared.DTOs.NotificacionesLimite
{
    public class NotificacionesAlertaNutricionalDTO
    {
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; } = string.Empty;
        public decimal LimiteCalorico { get; set; }
        public decimal CaloriasConsumidas { get; set; }
        public decimal Exceso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
