using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Alimentos;
using SmartNutriTracker.Shared.DTOs.Alimentos;
using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlimentoController : ControllerBase
    {
        private readonly IAlimentoService _alimentoService;
        private readonly ILogger<AlimentoController> _logger; // Logger agregado

        public AlimentoController(IAlimentoService alimentoService, ILogger<AlimentoController> logger)
        {
            _alimentoService = alimentoService;
            _logger = logger; // Se asigna el logger
        }

        // GET: api/Alimento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlimentoDTO>>> GetAll()
        {
            _logger.LogInformation("Solicitud GET: Obtener todos los alimentos."); // LOG

            var alimentos = await _alimentoService.GetAllAsync();

            _logger.LogInformation("Total de alimentos obtenidos: {Cantidad}", alimentos.Count()); // LOG

            return Ok(alimentos);
        }

        // GET: api/Alimento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AlimentoDTO>> GetById(int id)
        {
            _logger.LogInformation("Solicitud GET: Obtener alimento por ID: {Id}", id); // LOG

            var alimento = await _alimentoService.GetByIdAsync(id);

            if (alimento == null)
            {
                _logger.LogWarning("Alimento no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Alimento encontrado: {Nombre}", alimento.Nombre); // LOG
            return Ok(alimento);
        }

        // POST: api/Alimento
        [HttpPost]
        public async Task<ActionResult<AlimentoDTO>> Create([FromBody] CreateAlimentoDTO dto)
        {
            _logger.LogInformation("Solicitud POST: Crear alimento. Nombre: {Nombre}", dto.Nombre); // LOG

            var alimento = await _alimentoService.CreateAsync(dto);

            _logger.LogInformation("Alimento creado exitosamente con ID: {Id}", alimento.AlimentoId); // LOG

            return CreatedAtAction(nameof(GetById), new { id = alimento.AlimentoId }, alimento);
        }

        // PUT: api/Alimento/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AlimentoDTO>> Update(int id, [FromBody] UpdateAlimentoDTO dto)
        {
            _logger.LogInformation("Solicitud PUT: Actualizar alimento con ID: {Id}", id); // LOG

            if (id != dto.AlimentoId)
            {
                _logger.LogWarning("ID del alimento no coincide. Ruta: {IdRuta}, Cuerpo: {IdBody}", id, dto.AlimentoId); // LOG
                return BadRequest("El ID del alimento no coincide con el cuerpo de la petición.");
            }

            var alimento = await _alimentoService.UpdateAsync(dto);

            if (alimento == null)
            {
                _logger.LogWarning("No se pudo actualizar. Alimento no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Alimento actualizado correctamente: {Id}", id); // LOG

            return Ok(alimento);
        }

        // DELETE: api/Alimento/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Solicitud DELETE: Eliminar alimento con ID: {Id}", id); // LOG

            var success = await _alimentoService.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("No se pudo eliminar. Alimento no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Alimento eliminado exitosamente con ID: {Id}", id); // LOG

            return NoContent();
        }
    }
}
