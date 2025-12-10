using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Front.Services.Alimentos
{
    public class AlimentoService
    {
        private readonly HttpClient _http;
        private const string BASE = AlimentosEndpoints.BASE;

        public AlimentoService(IHttpClientFactory httpClientFactory)
        {
            // Usar siempre esta configuración puesto que ya se definió en Program.cs
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<AlimentoDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var url = BASE + AlimentosEndpoints.OBTENER_TODOS;
                var response = await _http.GetFromJsonAsync<List<AlimentoDTO>>(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener alimentos: {ex.Message}");
                return null;
            }
        }

        public async Task<AlimentoDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var url = $"{BASE}{AlimentosEndpoints.OBTENER_POR_ID}/{id}";
                var response = await _http.GetFromJsonAsync<AlimentoDTO>(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener alimento {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<AlimentoDTO?> CrearAsync(CreateAlimentoDTO dto)
        {
            try
            {
                var url = BASE + AlimentosEndpoints.CREAR;
                var response = await _http.PostAsJsonAsync(url, dto);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AlimentoDTO>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear alimento: {ex.Message}");
                return null;
            }
        }

        public async Task<AlimentoDTO?> ActualizarAsync(int id, UpdateAlimentoDTO dto)
        {
            try
            {
                var url = $"{BASE}{AlimentosEndpoints.ACTUALIZAR}/{id}";
                var response = await _http.PutAsJsonAsync(url, dto);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AlimentoDTO>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar alimento {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var url = $"{BASE}{AlimentosEndpoints.ELIMINAR}/{id}";
                var response = await _http.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar alimento {id}: {ex.Message}");
                return false;
            }
        }
    }
}