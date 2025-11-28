using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Alimentos;
using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlimentoController : ControllerBase
    {
        private readonly IAlimentoService _alimentoService;

        public AlimentoController(IAlimentoService alimentoService)
        {
            _alimentoService = alimentoService;
        }

        // GET: api/Alimento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlimentoDTO>>> GetAll()
        {
            var alimentos = await _alimentoService.GetAllAsync();
            return Ok(alimentos);
        }

        // GET: api/Alimento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AlimentoDTO>> GetById(int id)
        {
            var alimento = await _alimentoService.GetByIdAsync(id);
            if (alimento == null)
                return NotFound();
            return Ok(alimento);
        }

        // POST: api/Alimento
        [HttpPost]
        public async Task<ActionResult<AlimentoDTO>> Create([FromBody] CreateAlimentoDTO dto)
        {
            var alimento = await _alimentoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = alimento.AlimentoId }, alimento);
        }

        // PUT: api/Alimento/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AlimentoDTO>> Update(int id, [FromBody] UpdateAlimentoDTO dto)
        {
            if (id != dto.AlimentoId)
                return BadRequest("El ID del alimento no coincide con el cuerpo de la petición.");

            var alimento = await _alimentoService.UpdateAsync(dto);
            if (alimento == null)
                return NotFound();

            return Ok(alimento);
        }

        // DELETE: api/Alimento/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _alimentoService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
