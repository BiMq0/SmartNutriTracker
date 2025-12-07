// Front/Services/RegistroHabitoService.cs
using SmartNutriTracker.Shared.DTOs.Habitos;
using SmartNutriTracker.Shared.Endpoints;
using System.Text.Json;
using System.Net.Http.Json;

namespace SmartNutriTracker.Front.Services
{
    public class RegistroHabitoService
    {
        private readonly HttpClient _http;
        
        public RegistroHabitoService(IHttpClientFactory http)
        {
            _http = http.CreateClient("ApiClient");
        }
        
        public async Task<bool> RegistrarHabitosAsync(RegistroHabitoNuevoDTO registro)
        {
            try
            {
                var url = $"{RegistroHabitoEndpoints.BASE}{RegistroHabitoEndpoints.REGISTRAR_HABITOS}";
                var response = await _http.PostAsJsonAsync(url, registro);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<List<RegistroHabitoDTO>?> ObtenerHabitosPorEstudianteAsync(int estudianteId)
        {
            try
            {
                var url = $"{RegistroHabitoEndpoints.BASE}{RegistroHabitoEndpoints.OBTENER_HABITOS_POR_ESTUDIANTE}"
                    .Replace("{estudianteId}", estudianteId.ToString());
                
                return await _http.GetFromJsonAsync<List<RegistroHabitoDTO>>(url);
            }
            catch
            {
                return new List<RegistroHabitoDTO>();
            }
        }
        
        public async Task<List<RegistroHabitoDTO>?> ObtenerTodoPorEstudianteAsync(int estudianteId)
        {
            try
            {
                var url = $"{RegistroHabitoEndpoints.BASE}{RegistroHabitoEndpoints.OBTENER_TODO_POR_ESTUDIANTE}"
                    .Replace("{estudianteId}", estudianteId.ToString());
                
                return await _http.GetFromJsonAsync<List<RegistroHabitoDTO>>(url);
            }
            catch
            {
                return new List<RegistroHabitoDTO>();
            }
        }
        
        // ✅ CORREGIDO: Usar el endpoint correcto
        public async Task<RegistroHabitoDTO?> ObtenerHabitoPorIdAsync(int registroHabitoId)
        {
            try
            {
                var url = $"{RegistroHabitoEndpoints.BASE}{registroHabitoId}";
                Console.WriteLine($"🔍 Obteniendo hábito desde: {url}");
                
                var response = await _http.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RegistroHabitoDTO>();
                }
                
                Console.WriteLine($"⚠️ Respuesta no exitosa: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ObtenerHabitoPorIdAsync: {ex.Message}");
                return null;
            }
        }
        
        public async Task<bool> ActualizarHabitoAsync(int registroHabitoId, RegistroHabitoActualizarDTO dto)
        {
            try
            {
                var url = $"{RegistroHabitoEndpoints.BASE}{RegistroHabitoEndpoints.ACTUALIZAR}"
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
                Console.WriteLine($"❌ Excepción en ActualizarHabitoAsync (Front): {ex.Message}");
                return false;
            }
        }
    }
}