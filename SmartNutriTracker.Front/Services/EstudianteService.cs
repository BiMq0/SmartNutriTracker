using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SmartNutriTracker.Front.Handlers;
using SmartNutriTracker.Shared.Endpoints;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Front.Services
{
    public class EstudianteService
    {
        private readonly HttpClient _http;

        public EstudianteService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ApiClient");
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
                return await _http.GetFromJsonAsync<PerfilEstudianteDTO?>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener perfil {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Estudiante?> ObtenerEstudiantePorIdAsync(int id)
        {
            try
            {
                var url = EstudiantesEndpoints.OBTENER_ESTUDIANTE_POR_ID.Replace("{id}", id.ToString());
                return await _http.GetFromJsonAsync<Estudiante?>(url);
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

        public async Task<HttpResponseMessage> ActualizarPerfilAsync(int id, EstudianteUpdateDTO dto)
        {
            var url = EstudiantesEndpoints.ACTUALIZAR_PERFIL_ESTUDIANTE.Replace("{id}", id.ToString());
            return await _http.PutAsJsonAsync(url, dto);
        }
    }

    public class EstudianteRegistroResponseDTO
    {
        public string? Mensaje { get; set; }
        public Estudiante? Estudiante { get; set; }
    }
}
