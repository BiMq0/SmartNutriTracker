using SmartNutriTracker.Shared.DTOs.Habitos;
using SmartNutriTracker.Shared.Endpoints;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
namespace SmartNutriTracker.Front.Services
{
    public class HabitoService
    {
        private readonly HttpClient _http;
        
        public HabitoService(IHttpClientFactory http)
        {
            _http = http.CreateClient("ApiClient");
        }
        
        public async Task<List<HabitoRegistroDTO>?> ObtenerHabitosPorEstudianteAsync(int estudianteId)
        {
            try
            {
                var url = $"{HabitoEndpoints.BASE}{HabitoEndpoints.OBTENER_POR_ESTUDIANTE}"
                    .Replace("{estudianteId}", estudianteId.ToString());
                
                return await _http.GetFromJsonAsync<List<HabitoRegistroDTO>>(url);
            }
            catch
            {
                return new List<HabitoRegistroDTO>();
            }
        }
        public async Task<HabitoRegistroDTO?> ObtenerHabitoPorIdAsync(int registroHabitoId, int estudianteId)
{
    try
    {
        var habitos = await ObtenerHabitosPorEstudianteAsync(estudianteId);
        return habitos?.FirstOrDefault(h => h.RegistroHabitoId == registroHabitoId);
    }
    catch
    {
        return null;
    }
}
    }
}