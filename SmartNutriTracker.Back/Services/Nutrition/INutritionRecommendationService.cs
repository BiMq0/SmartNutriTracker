using System.Threading.Tasks;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public interface INutritionRecommendationService
    {
        Task<ResultadoRecomendacionNutricionalDTO> GenerateAsync(NutritionRequestDTO solicitud);
    }
}
