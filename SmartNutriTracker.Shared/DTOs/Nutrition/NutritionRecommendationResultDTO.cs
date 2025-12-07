using System.Collections.Generic;

namespace SmartNutriTracker.Shared.DTOs.Nutrition
{
    public class ResultadoRecomendacionNutricionalDTO
    {
        // Texto libre con recomendaciones generadas por la IA (resumen legible)
        public string TextoRecomendacion { get; set; } = string.Empty;

        // Resultado numérico calculado por el servicio (contexto)
        public NutritionResultDTO? ResultadoNutricional { get; set; }

        // Salida estructurada por comidas (opcional si la IA devuelve JSON)
        public IEnumerable<ComidaDTO>? Comidas { get; set; }

        // Tiempo de respuesta de la IA en milisegundos
        public long TiempoRespuestaMs { get; set; }
    }
}
