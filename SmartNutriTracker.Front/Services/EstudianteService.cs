using System.Net.Http.Json;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using SmartNutriTracker.Shared.Endpoints;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Front.Handlers;
using System.Text.Json;

namespace SmartNutriTracker.Front.Services;

public class EstudianteService
{
    private readonly HttpClient _http;

    public EstudianteService(IHttpClientFactory httpClientFactory)
    {
        // Para desarrollo, usar siempre HTTP (5073) para evitar errores de certificado/puerto.
        // No se tocan configuraciones globales; solo este cliente concreto.
        _http = httpClientFactory.CreateClient("ApiClient");
        _http.BaseAddress = new Uri(ApiConfig.HttpApiUrl);
    }

    public async Task<EstudianteRegistroResponseDTO?> RegistrarEstudianteAsync(EstudianteRegistroDTO dto)
    {
        try
        {
            var url = EstudiantesEndpoints.REGISTRAR_ESTUDIANTE;
            var response = await _http.PostAsJsonAsync(url, dto);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadFromJsonAsync<Estudiante>();
                return new EstudianteRegistroResponseDTO 
                { 
                    Estudiante = resultado,
                    Mensaje = "Estudiante registrado correctamente."
                };
            }

            // En caso de error, intentar leer el mensaje
            var errorContent = await response.Content.ReadAsStringAsync();
            return new EstudianteRegistroResponseDTO 
            { 
                Mensaje = $"Error: {errorContent}" 
            };
        }
        catch (Exception ex)
        {
            return new EstudianteRegistroResponseDTO 
            { 
                Mensaje = $"Error: {ex.Message}" 
            };
        }
    }

    public async Task<PerfilEstudianteDTO?> ObtenerPerfilAsync(int id)
    {
        try
        {
            var url = EstudiantesEndpoints.OBTENER_PERFIL_ESTUDIANTE.Replace("{id}", id.ToString());
            return await _http.GetFromJsonAsync<PerfilEstudianteDTO>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener perfil {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<Estudiante?> ObtenerPorIdAsync(int id)
    {
        try
        {
            var url = EstudiantesEndpoints.OBTENER_ESTUDIANTE_POR_ID.Replace("{id}", id.ToString());
            return await _http.GetFromJsonAsync<Estudiante>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener estudiante {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Estudiante>?> ObtenerTodosAsync()
    {
        try
        {
            var url = EstudiantesEndpoints.OBTENER_ESTUDIANTES;
            return await _http.GetFromJsonAsync<List<Estudiante>>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener estudiantes: {ex.Message}");
            return null;
        }
    }

    public async Task<EstudianteDetalleCompletoDTO?> ObtenerDetalleCompletoAsync(int id)
    {
        try
        {
            var url = $"/api/estudiantes/{id}/detalle-completo";
            return await _http.GetFromJsonAsync<EstudianteDetalleCompletoDTO>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener detalle completo {id}: {ex.Message}");
            return null;
        }
    }
}

public class EstudianteRegistroResponseDTO
{
    public string? Mensaje { get; set; }
    public Estudiante? Estudiante { get; set; }
}

