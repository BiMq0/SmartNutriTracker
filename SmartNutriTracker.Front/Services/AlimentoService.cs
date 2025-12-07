using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Shared.Endpoints;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
namespace SmartNutriTracker.Front.Services
{
    public class AlimentoService
    {
        private readonly HttpClient _http;
        
        public AlimentoService(IHttpClientFactory http)
        {
            _http = http.CreateClient("ApiClient");
        }
        
        public async Task<List<AlimentoDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var url = $"{AlimentoEndpoints.BASE}{AlimentoEndpoints.OBTENER_TODOS}";
                return await _http.GetFromJsonAsync<List<AlimentoDTO>>(url);
            }
            catch
            {
                return new List<AlimentoDTO>();
            }
        }
    }
}