using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Habitos;
using SmartNutriTracker.Shared.DTOs.Habitos; // ✅ DTO correcto
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(HabitoEndpoints.BASE)]
    public class HabitosController : ControllerBase
    {
        private readonly IHabitosService _habitsService;
        
        public HabitosController(IHabitosService habitsService)
        {
            _habitsService = habitsService;
        }

        [HttpGet(HabitoEndpoints.OBTENER_POR_ESTUDIANTE)]
        public async Task<ActionResult<List<HabitoRegistroDTO>>> ObtenerHabitos(int estudianteId)
        {
            var resultado = await _habitsService.ObtenerHabitosAsync(estudianteId);
            return Ok(resultado);
        }
    }
}