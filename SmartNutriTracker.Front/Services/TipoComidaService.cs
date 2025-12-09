using SmartNutriTracker.Shared.DTOs.Alimentos; 
using SmartNutriTracker.Shared.Endpoints;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
namespace SmartNutriTracker.Front.Services
{
    public class TipoComidaService
    {
        private readonly HttpClient _http;
        
        public TipoComidaService(IHttpClientFactory http)
        {
            _http = http.CreateClient("ApiClient");
        }
        
        public async Task<List<TipoComidaDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var url = $"{TipoComidaEndpoints.BASE}{TipoComidaEndpoints.OBTENER_TODOS}";
                return await _http.GetFromJsonAsync<List<TipoComidaDTO>>(url);
            }
            catch
            {
                return new List<TipoComidaDTO>();
            }
        }
    }
}