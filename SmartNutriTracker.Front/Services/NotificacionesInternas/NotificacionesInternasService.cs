using SmartNutriTracker.Shared.DTOs.Notificaciones;
using SmartNutriTracker.Shared.Endpoints;
using System.Net.Http.Json;

namespace SmartNutriTracker.Front.Services.NotificacionesInternas
{
    public class NotificacionesInternasService
    {
        private readonly HttpClient _http;

        public NotificacionesInternasService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        /// <summary>
        /// Obtiene la lista de todas las notificaciones pendientes (Administrador).
        /// </summary>
        public async Task<List<NotificacionDiariaDTO>> ObtenerNotificacionesPendientesAsync()
        {
            // Corregimos la concatenación de la URL eliminando el slash extra
            var url = $"{NotificacionesDiariasEndpoints.BASE}{NotificacionesDiariasEndpoints.OBTENER_PENDIENTES_DIARIOS}";

            try
            {
                var notificaciones = await _http.GetFromJsonAsync<List<NotificacionDiariaDTO>>(url);
                return notificaciones ?? new List<NotificacionDiariaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener notificaciones: {ex.Message}");
                return new List<NotificacionDiariaDTO>();
            }
        }

        /// <summary>
        /// Obtiene la notificación diaria pendiente para un estudiante específico.
        /// </summary>
        /// <param name="estudianteId">ID del estudiante.</param>
        public async Task<NotificacionDiariaDTO?> ObtenerNotificacionPorEstudianteAsync(int estudianteId)
        {
            var ruta = NotificacionesDiariasEndpoints.OBTENER_PENDIENTES_DIARIOS_POR_ESTUDIANTE
                        .Replace("{estudianteId}", estudianteId.ToString());
            
            var url = $"{NotificacionesDiariasEndpoints.BASE}{ruta}";

            try
            {
                // Usamos GetAsync para poder manejar el 404 (NotFound) sin lanzar excepción
                var response = await _http.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<NotificacionDiariaDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener notificación del estudiante {estudianteId}: {ex.Message}");
                return null;
            }
        }
    }
}