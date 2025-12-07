using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.NotificacionesLimite;
using SmartNutriTracker.Shared.DTOs.NotificacionesLimite;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers.NotificacionesLimite
{
    [Route(NotificacionesLimiteEndpoints.BASE)]
    [ApiController]
    public class NotificacionesLimiteController : ControllerBase
    {
        private readonly INotificacionesLimiteService _notificacionesLimiteService;

        public NotificacionesLimiteController(INotificacionesLimiteService notificacionesLimiteService)
        {
            _notificacionesLimiteService = notificacionesLimiteService;
        }

        /// <summary>
        /// Inicia el protocolo de detección de riesgos nutricionales a nivel global en la institución.
        /// Este método invoca al servicio de notificaciones para realizar un análisis profundo de los datos,
        /// abstrayendo la complejidad de las consultas a base de datos y cálculos metabólicos.
        /// Retorna un conjunto de alertas de alta prioridad diseñadas para activar intervenciones preventivas inmediatas.
        /// </summary>
        /// <returns>Una colección estructurada de alertas de seguridad nutricional para estudiantes en riesgo.</returns>
        [HttpGet(NotificacionesLimiteEndpoints.OBTENER_ALERTAS)]
        public async Task<List<NotificacionesAlertaNutricionalDTO>> ObtenerAlertasCaloricas()
        {
            return await _notificacionesLimiteService.ObtenerAlertasCaloricasAsync();
        }
    }
}
