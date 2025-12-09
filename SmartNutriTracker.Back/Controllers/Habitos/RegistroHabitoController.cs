using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Habitos;
using SmartNutriTracker.Shared.DTOs.Habitos;
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(RegistroHabitoEndpoints.BASE)]
    public class RegistroHabitoController : ControllerBase
    {
        private readonly IRegistroHabitoService _service;

        public RegistroHabitoController(IRegistroHabitoService service)
        {
            _service = service;
        }

        [HttpPost(RegistroHabitoEndpoints.REGISTRAR_HABITOS)]
        public async Task<IActionResult> RegistrarHabitos([FromBody] RegistroHabitoNuevoDTO nuevo)
        {
            if (nuevo == null || !ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos" });

            var resultado = await _service.RegistrarHabitosAsync(nuevo);

            return resultado 
                ? Ok(new { mensaje = "Hábitos registrados exitosamente." })
                : StatusCode(500, new { mensaje = "Error al registrar los hábitos." });
        }

        [HttpGet(RegistroHabitoEndpoints.OBTENER_HABITOS_POR_ESTUDIANTE)]
        public async Task<ActionResult<List<RegistroHabitoDTO>>> ObtenerHabitosPorEstudiante(int estudianteId)
        {
            var lista = await _service.ObtenerHabitosPorEstudianteAsync(estudianteId);
            return Ok(lista);
        }

        [HttpGet(RegistroHabitoEndpoints.OBTENER_TODO_POR_ESTUDIANTE)]
        public async Task<ActionResult<List<RegistroHabitoDTO>>> ObtenerTodoPorEstudiante(int estudianteId)
        {
            var lista = await _service.ObtenerPorEstudianteAsync(estudianteId);
            return Ok(lista);
        }


        [HttpGet("{registroHabitoId}")]
        public async Task<ActionResult<RegistroHabitoDTO>> ObtenerPorId(int registroHabitoId)
        {
            var registro = await _service.ObtenerHabitoPorIdAsync(registroHabitoId);
            
            if (registro == null)
                return NotFound(new { mensaje = "Registro no encontrado" });

            return Ok(registro);
        }

        [HttpPut(RegistroHabitoEndpoints.ACTUALIZAR)]
        public async Task<IActionResult> Actualizar(int registroHabitoId,
            [FromBody] RegistroHabitoActualizarDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos" });

            var ok = await _service.ActualizarRegistroAsync(registroHabitoId, dto);

            return ok 
                ? Ok(new { mensaje = "Registro actualizado." })
                : NotFound(new { mensaje = "Registro no encontrado." });
        }
    }
}