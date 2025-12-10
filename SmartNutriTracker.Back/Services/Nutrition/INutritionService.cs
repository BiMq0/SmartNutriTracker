using System.Threading.Tasks;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public interface INutritionService
    {
        Task<NutritionResultDTO> CalculateAsync(NutritionRequestDTO request);
    }
}
