using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNutriTracker.Shared.Endpoints
{
    public static class NotificacionesDiariasEndpoints
    {
        public const string BASE = "api/notificaciones-diarias/";
        public const string OBTENER_PENDIENTES_DIARIOS = "recordatorios-pendientes";
        public const string OBTENER_PENDIENTES_DIARIOS_POR_ESTUDIANTE = "recordatorios-pendientes/{estudianteId}";
    }
}