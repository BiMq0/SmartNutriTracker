using System.Threading.Tasks;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public interface IPdfExportService
    {
        /// <summary>
        /// Exporta las recomendaciones nutricionales a PDF
        /// </summary>
        Task<byte[]> ExportRecomendacionesToPdfAsync(ResultadoRecomendacionNutricionalDTO recomendacion, NutritionResultDTO? calculo = null);
    }
}
