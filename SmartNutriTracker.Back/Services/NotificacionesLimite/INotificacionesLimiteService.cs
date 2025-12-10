using SmartNutriTracker.Shared.DTOs.NotificacionesLimite;

namespace SmartNutriTracker.Back.Services.NotificacionesLimite
{
    public interface INotificacionesLimiteService
    {
        Task<List<NotificacionesAlertaNutricionalDTO>> ObtenerAlertasCaloricasAsync();
    }
}
