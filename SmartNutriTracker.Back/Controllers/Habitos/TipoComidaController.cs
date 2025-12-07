using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Alimentos; // ✅ TipoComidaDTO está en Alimentos/
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(TipoComidaEndpoints.BASE)]
    public class TipoComidaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoComidaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(TipoComidaEndpoints.OBTENER_TODOS)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var tipos = await _context.TiposComida
                .AsNoTracking()
                .Select(t => new TipoComidaDTO // ✅ Usar TipoComidaDTO
                {
                    TipoComidaId = t.TipoComidaId,
                    Nombre = t.Nombre
                })
                .ToListAsync();

            return Ok(tipos);
        }
    }
}