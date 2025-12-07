namespace SmartNutriTracker.Shared.DTOs.Notificaciones;

public record RecordatorioDiarioDTO
{
    public int EstudianteId { get; set; }
    public string NombreEstudiante { get; set; }
    public string Mensaje { get; set; }
    public DateTime FechaIntento {get;set;}
}