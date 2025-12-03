using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Nutrition;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly INutritionRecommendationService _nutritionRecommendationService;

        public NutritionController(INutritionService nutritionService, INutritionRecommendationService nutritionRecommendationService)
        {
            _nutritionService = nutritionService;
            _nutritionRecommendationService = nutritionRecommendationService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] NutritionRequestDTO request)
        {
            try
            {
                var result = await _nutritionService.CalculateAsync(request);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                //abc.funcion(libreria.variable)
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("recommendations")]
        public async Task<IActionResult> Recommendations([FromBody] NutritionRequestDTO solicitud)
        {
            try
            {
                // Resolve recommendation service via DI
                var recService = _nutritionRecommendationService;
                if (recService == null)
                {
                    return StatusCode(500, new { mensaje = "Servicio de recomendaciones no configurado." });
                }
                var result = await recService.GenerateAsync(solicitud);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }
}
