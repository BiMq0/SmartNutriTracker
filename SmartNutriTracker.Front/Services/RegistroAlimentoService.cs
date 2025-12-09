using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Shared.Endpoints;
using System.Text.Json; 
using System.Net.Http.Json; 

using System.Text;

namespace SmartNutriTracker.Front.Services
{
    public class RegistroAlimentoService
    {
        private readonly HttpClient _http;
        
        public RegistroAlimentoService(IHttpClientFactory http)
        {
            _http = http.CreateClient("ApiClient");
        }
        
        public async Task<bool> RegistrarConsumoAsync(RegistroConsumoNuevoDTO registro)
        {
            try
            {
                var url = $"{RegistroAlimentoEndpoints.BASE}{RegistroAlimentoEndpoints.REGISTRAR_CONSUMO}";
                var response = await _http.PostAsJsonAsync(url, registro);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<List<RegistroConsumoDTO>?> ObtenerPorEstudianteAsync(int estudianteId)
        {
            try
            {
                var url = $"{RegistroAlimentoEndpoints.BASE}{RegistroAlimentoEndpoints.OBTENER_POR_ESTUDIANTE}"
                    .Replace("{estudianteId}", estudianteId.ToString());
                
                return await _http.GetFromJsonAsync<List<RegistroConsumoDTO>>(url);
            }
            catch
            {
                return new List<RegistroConsumoDTO>();
            }
        }
        
        public async Task<RegistroConsumoDTO?> ObtenerPorIdAsync(int registroHabitoId)
        {
            try
            {
                var url = $"{RegistroAlimentoEndpoints.BASE}{RegistroAlimentoEndpoints.OBTENER_POR_ID}"
                    .Replace("{registroHabitoId}", registroHabitoId.ToString());
                
                return await _http.GetFromJsonAsync<RegistroConsumoDTO>(url);
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<bool> ActualizarConsumoAsync(int registroHabitoId, RegistroConsumoActualizarDTO dto)
{
    try
    {
        var url = $"{RegistroAlimentoEndpoints.BASE}{RegistroAlimentoEndpoints.ACTUALIZAR_CONSUMO}"
            .Replace("{registroHabitoId}", registroHabitoId.ToString());
        
        Console.WriteLine($"🔄 Enviando PUT a: {url}");
        Console.WriteLine($"📦 Payload: {JsonSerializer.Serialize(dto)}");
        
        var response = await _http.PutAsJsonAsync(url, dto);
        
        Console.WriteLine($"📥 Respuesta Status: {response.StatusCode}");
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Error del servidor: {errorContent}");
            return false;
        }
        
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Excepción en ActualizarConsumoAsync (Front): {ex.Message}");
        return false;
    }
}
    }
}