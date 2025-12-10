using SmartNutriTracker.Shared.DTOs.Notificaciones;

namespace SmartNutriTracker.Back.Services.Notificaciones;

public interface INotificacionDiariaService
{
    Task<List<NotificacionDiariaDTO>> ObtenerPendientesDiariosAsync();
    Task<NotificacionDiariaDTO?> ObtenerRecordatorioPorEstudianteAsync(int estudianteId);
}