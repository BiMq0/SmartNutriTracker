using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmartNutriTracker.Back.Services.Nutrition;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public class NutritionRecommendationService : INutritionRecommendationService
    {
        private readonly INutritionService _servicioNutricional;
        private readonly IConfiguration _config;

        public NutritionRecommendationService(INutritionService servicioNutricional, IConfiguration config)
        {
            _servicioNutricional = servicioNutricional;
            _config = config;
        }

        public async Task<ResultadoRecomendacionNutricionalDTO> GenerateAsync(NutritionRequestDTO solicitud)
        {
            // Calcular valores numéricos
            var resultadoNumerico = await _servicioNutricional.CalculateAsync(solicitud);

            // Preparar prompt para la IA, solicitando salida JSON estructurada en español
            var prompt = BuildPrompt(solicitud, resultadoNumerico);

            var apiKey = _config["AI:ApiKey"] ?? _config["AI_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("AI API key not configured. Set configuration 'AI:ApiKey' or environment 'AI_API_KEY'.");
            }

            // Crear HttpClient localmente usando la base URL configurada
            var baseUrl = _config["AI:BaseUrl"] ?? "https://api.cohere.ai/";
            using var client = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(200) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "command-a-03-2025",
                messages = new[] { new { role = "user", content = prompt } },
                temperature = 0.25
            };

            var response = await client.PostAsJsonAsync("v2/chat", requestBody);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"AI provider error: {response.StatusCode} - {err}");
            }

            var cuerpoRespuesta = await response.Content.ReadAsStringAsync();
            var textoIA = ExtractTextFromProviderResponse(cuerpoRespuesta);

            // Intentar parsear texto como JSON (si la IA devolvió un bloque JSON)
            IEnumerable<ComidaDTO>? comidas = null;
            string textoRecomendacion = textoIA;
            
            var jsonBlock = TryExtractJsonBlock(textoIA);
            if (!string.IsNullOrEmpty(jsonBlock))
            {
                try
                {
                    using var doc = JsonDocument.Parse(jsonBlock);
                    var root = doc.RootElement;
                    
                    // Extraer textoRecomendacion si existe
                    if (root.TryGetProperty("textoRecomendacion", out var textoEl))
                    {
                        textoRecomendacion = textoEl.GetString() ?? textoIA;
                    }
                    
                    // Buscar propiedad 'comidas' o 'meals'
                    if (root.TryGetProperty("comidas", out var comidasEl) || root.TryGetProperty("meals", out comidasEl))
                    {
                        var lista = new List<ComidaDTO>();
                        if (comidasEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var it in comidasEl.EnumerateArray())
                            {
                                var comida = new ComidaDTO();
                                if (it.TryGetProperty("nombre", out var n)) comida.Nombre = n.GetString() ?? string.Empty;
                                if (it.TryGetProperty("calorias", out var c) && c.TryGetDecimal(out var cv)) comida.Calorias = cv;
                                if (it.TryGetProperty("proteinas_g", out var p) && p.TryGetDecimal(out var pv)) comida.Proteinas_g = pv;
                                if (it.TryGetProperty("grasas_g", out var g) && g.TryGetDecimal(out var gv)) comida.Grasas_g = gv;
                                if (it.TryGetProperty("carbohidratos_g", out var cb) && cb.TryGetDecimal(out var cbv)) comida.Carbohidratos_g = cbv;
                                if (it.TryGetProperty("descripcion", out var d)) comida.Descripcion = d.GetString() ?? string.Empty;
                                lista.Add(comida);
                            }
                        }
                        comidas = lista;
                    }
                }
                catch
                {
                    // ignore parse errors; fallback a texto plano
                }
            }
            
            ResultadoRecomendacionNutricionalDTO resultadoFinal = new()
            {
                TextoRecomendacion = textoRecomendacion,
                ResultadoNutricional = resultadoNumerico,
                Comidas = comidas
            };

            return resultadoFinal;
        }

        private string BuildPrompt(NutritionRequestDTO solicitud, NutritionResultDTO numeric)
        {
            var objetivo = solicitud.Objetivo ?? "mantener";
            var actividad = solicitud.NivelActividad ?? "sedentario";

                        // Pedir explícitamente un bloque JSON con claves en español: 'textoRecomendacion' y 'comidas'
                        return $@"Eres un nutricionista escolar. Genera recomendaciones claras para un estudiante.
Provee:
1) Un campo 'textoRecomendacion' con un resumen breve (2-3 líneas).
2) Un campo 'comidas' (array) donde cada elemento tenga: nombre, calorias, proteinas_g, grasas_g, carbohidratos_g, descripcion.

Datos calculados:
- TMB: {numeric.TMB} kcal
- Calorías mantenimiento: {numeric.CaloriasMantenimiento} kcal
- Calorías objetivo: {numeric.CaloriasObjetivo} kcal
- Proteínas: {numeric.ProteinasGr} g
- Grasas: {numeric.GrasasGr} g
- Carbohidratos: {numeric.CarbohidratosGr} g
Objetivo: {objetivo}
Nivel de actividad: {actividad}

IMPORTANTE: Devuelve SOLO un objeto JSON válido con las propiedades 'textoRecomendacion' (string) y 'comidas' (array de objetos con las claves indicadas). No incluyas texto adicional fuera del JSON."
                        ;
        }

        private string ExtractTextFromProviderResponse(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("message", out var msgEl))
                {
                    if (msgEl.TryGetProperty("content", out var contentEl) && contentEl.ValueKind == JsonValueKind.Array && contentEl.GetArrayLength() > 0)
                    {
                        var first = contentEl[0];
                        if (first.TryGetProperty("text", out var textEl))
                        {
                            return textEl.GetString() ?? string.Empty;
                        }
                    }
                }

                if (doc.RootElement.TryGetProperty("content", out var rootContent) && rootContent.ValueKind == JsonValueKind.String)
                {
                    return rootContent.GetString() ?? string.Empty;
                }

                // Fallback a todo el JSON como string
                return doc.RootElement.ToString();
            }
            catch
            {
                return json;
            }
        }

        private string TryExtractJsonBlock(string texto)
        {
            // Intentar extraer el primer bloque JSON del texto buscando desde la primera '{' hasta el último '}'
            var idxStart = texto.IndexOf('{');
            var idxEnd = texto.LastIndexOf('}');
            if (idxStart >= 0 && idxEnd > idxStart)
            {
                var candidate = texto.Substring(idxStart, idxEnd - idxStart + 1);
                // Validar que sea JSON
                try
                {
                    using var doc = JsonDocument.Parse(candidate);
                    return candidate;
                }
                catch { }
            }
            return string.Empty;
        }
    }
}
