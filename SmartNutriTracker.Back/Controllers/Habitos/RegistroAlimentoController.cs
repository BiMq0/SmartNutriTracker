using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Alimentos;
using SmartNutriTracker.Shared.DTOs.Alimentos; 
using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route(RegistroAlimentoEndpoints.BASE)]
    public class RegistroAlimentoController : ControllerBase
    {
        private readonly IRegistroAlimentoService _service;

        public RegistroAlimentoController(IRegistroAlimentoService service)
        {
            _service = service;
        }

        [HttpPost(RegistroAlimentoEndpoints.REGISTRAR_CONSUMO)]
        public async Task<IActionResult> RegistrarConsumo([FromBody] RegistroConsumoNuevoDTO nuevo)
        {
            if (nuevo == null || !ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos" });

            var resultado = await _service.RegistrarConsumoAsync(nuevo);

            return resultado 
                ? Ok(new { mensaje = "Consumo registrado exitosamente." })
                : StatusCode(500, new { mensaje = "Error al registrar el consumo." });
        }

        [HttpGet(RegistroAlimentoEndpoints.OBTENER_POR_ESTUDIANTE)]
        public async Task<ActionResult<List<RegistroConsumoDTO>>> ObtenerPorEstudiante(int estudianteId)
        {
            var lista = await _service.ObtenerPorEstudianteAsync(estudianteId);
            return Ok(lista);
        }

        [HttpGet(RegistroAlimentoEndpoints.OBTENER_POR_ID)]
        public async Task<ActionResult<RegistroConsumoDTO>> ObtenerPorId(int registroHabitoId)
        {
            var registro = await _service.ObtenerPorIdAsync(registroHabitoId);
            
            if (registro == null)
                return NotFound(new { mensaje = "Registro no encontrado" });

            return Ok(registro);
        }

        [HttpPut(RegistroAlimentoEndpoints.ACTUALIZAR_CONSUMO)]
        public async Task<IActionResult> ActualizarConsumo(int registroHabitoId, [FromBody] RegistroConsumoActualizarDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos" });

            var resultado = await _service.ActualizarConsumoAsync(registroHabitoId, dto);

            return resultado 
                ? Ok(new { mensaje = "Consumo actualizado exitosamente." })
                : NotFound(new { mensaje = "Registro no encontrado." });
        }
    }
}