using SmartNutriTracker.Shared.DTOs.NotificacionesLimite;
using SmartNutriTracker.Shared.Endpoints;
using System.Net.Http.Json;

namespace SmartNutriTracker.Front.Services
{
    public class NotificacionesLimiteService
    {
        private readonly HttpClient _http;

        public NotificacionesLimiteService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        /// <summary>
        /// Obtiene la lista de alertas calóricas desde el backend.
        /// </summary>
        public async Task<List<NotificacionesAlertaNutricionalDTO>> ObtenerAlertasAsync()
        {
            var url = $"{NotificacionesLimiteEndpoints.BASE}/{NotificacionesLimiteEndpoints.OBTENER_ALERTAS}"
                        .TrimStart('/');

            try
            {
                var alertas = await _http.GetFromJsonAsync<List<NotificacionesAlertaNutricionalDTO>>(url);
                return alertas ?? new List<NotificacionesAlertaNutricionalDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener alertas: {ex.Message}");
                return new List<NotificacionesAlertaNutricionalDTO>();
            }
        }
    }
}
