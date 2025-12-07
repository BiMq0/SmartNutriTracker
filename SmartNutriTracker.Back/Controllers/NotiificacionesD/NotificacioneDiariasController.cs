using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Notificaciones;
using SmartNutriTracker.Shared.DTOs.Notificaciones;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers;

[ApiController]
[Route(NotificacionesDiariasEndpoints.BASE)]
public class NotificacionDiariaController : ControllerBase
{
    private readonly INotificacionDiariaService _notificacionService;

    public NotificacionDiariaController(INotificacionDiariaService notificacionService)
    {
        _notificacionService = notificacionService;
    }
    
    [HttpGet(NotificacionesDiariasEndpoints.OBTENER_PENDIENTES_DIARIOS)]
    public async Task<List<RecordatorioDiarioDTO>> ObtenerPendientesDiarios()
    {
        return await _notificacionService.ObtenerPendientesDiariosAsync();
    }

    [HttpGet(NotificacionesDiariasEndpoints.OBTENER_PENDIENTES_DIARIOS_POR_ESTUDIANTE)]
    public async Task<IActionResult> ObtenerRecordatorioPorEstudiante(int estudianteId)
    {
        var recordatorio = await _notificacionService.ObtenerRecordatorioPorEstudianteAsync(estudianteId);
        if (recordatorio == null)
        {
            return NotFound(new { mensaje = "Estudiante no encontrado o no requiere recordatorio." });
        }
        return Ok(recordatorio);
    }
}