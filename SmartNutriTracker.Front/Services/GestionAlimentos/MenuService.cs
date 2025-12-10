using SmartNutriTracker.Shared.DTOs.Menus;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Front.Services.Menus
{
    public class MenuService
    {
        private readonly HttpClient _http;
        private const string BASE = MenusEndpoints.BASE;

        public MenuService(IHttpClientFactory httpClientFactory)
        {
            // Usar siempre esta configuración puesto que ya se definió en Program.cs
            _http = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<MenuDTO>?> ObtenerTodosAsync()
        {
            try
            {
                var url = BASE + MenusEndpoints.OBTENER_TODOS;
                var response = await _http.GetFromJsonAsync<List<MenuDTO>>(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener menús: {ex.Message}");
                return null;
            }
        }

        public async Task<MenuDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var url = $"{BASE}{MenusEndpoints.OBTENER_POR_ID}/{id}";
                var response = await _http.GetFromJsonAsync<MenuDTO>(url);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener menú {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> CrearAsync(CreateMenuDTO dto)
        {
            try
            {
                var url = BASE + MenusEndpoints.CREAR_MENU;
                var response = await _http.PostAsJsonAsync(url, dto);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<int>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear menú: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ActualizarAsync(int id, UpdateMenuDTO dto)
        {
            try
            {
                var url = $"{BASE}{MenusEndpoints.ACTUALIZAR_MENU}/{id}";
                var response = await _http.PutAsJsonAsync(url, dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar menú {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var url = $"{BASE}{MenusEndpoints.ELIMINAR_MENU}/{id}";
                var response = await _http.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar menú {id}: {ex.Message}");
                return false;
            }
        }
    }
}