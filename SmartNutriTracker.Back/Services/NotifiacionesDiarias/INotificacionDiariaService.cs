using SmartNutriTracker.Shared.DTOs.Notificaciones;

namespace SmartNutriTracker.Back.Services.Notificaciones;

public interface INotificacionDiariaService
{
    Task<List<RecordatorioDiarioDTO>> ObtenerPendientesDiariosAsync();
    Task<RecordatorioDiarioDTO?> ObtenerRecordatorioPorEstudianteAsync(int estudianteId);
}