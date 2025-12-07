using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Alimentos; // ✅ DTO correcto
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(AlimentoEndpoints.BASE)]
    public class AlimentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlimentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(AlimentoEndpoints.OBTENER_TODOS)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var alimentos = await _context.Alimentos
                .AsNoTracking()
                .Select(a => new AlimentoDTO // ✅ Usar AlimentoDTO
                {
                    AlimentoId = a.AlimentoId,
                    Nombre = a.Nombre,
                    Calorias = a.Calorias
                })
                .ToListAsync();

            return Ok(alimentos);
        }
    }
}