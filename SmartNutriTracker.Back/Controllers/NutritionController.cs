using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Nutrition;
using SmartNutriTracker.Back.Database;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Controllers
{
    /// <summary>
    /// Controlador para cálculos nutricionales y recomendaciones con IA
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _servicioNutricion;
        private readonly INutritionRecommendationService _servicioRecomendaciones;
        private readonly ApplicationDbContext _contexto;
        private readonly IPdfExportService _servicioPdf;
        private readonly INutritionValidator _validador;

        public NutritionController(
            INutritionService servicioNutricion, 
            INutritionRecommendationService servicioRecomendaciones, 
            ApplicationDbContext contexto, 
            IPdfExportService servicioPdf, 
            INutritionValidator validador)
        {
            _servicioNutricion = servicioNutricion;
            _servicioRecomendaciones = servicioRecomendaciones;
            _contexto = contexto;
            _servicioPdf = servicioPdf;
            _validador = validador;
        }

        /// <summary>
        /// Calcula valores nutricionales (TMB, calorías, macros)
        /// </summary>
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] NutritionRequestDTO solicitud)
        {
            try
            {
                var resultado = await _servicioNutricion.CalculateAsync(solicitud);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Genera recomendaciones nutricionales con IA
        /// </summary>
        [HttpPost("recommendations")]
        public async Task<IActionResult> Recommendations([FromBody] NutritionRequestDTO solicitud)
        {
            try
            {
               
                var validacion = _validador.ValidateNutritionRequest(solicitud);
                if (!validacion.IsValid)
                {
                    return BadRequest(new { 
                        mensaje = "Datos inválidos", 
                        errores = validacion.Errores 
                    });
                }

                
                var resultado = await _servicioRecomendaciones.GenerateAsync(solicitud);
                return Ok(resultado);
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

        /// <summary>
        /// Exporta recomendación a PDF
        /// </summary>
        [HttpPost("export-pdf")]
        public async Task<IActionResult> ExportToPdf([FromBody] ResultadoRecomendacionNutricionalDTO recomendacion)
        {
            try
            {
                if (recomendacion == null)
                    return BadRequest(new { mensaje = "Recomendación no proporcionada" });

                var bytesPdf = await _servicioPdf.ExportRecomendacionesToPdfAsync(
                    recomendacion, 
                    recomendacion.ResultadoNutricional);

                var nombreArchivo = $"Recomendacion_Nutricional_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(bytesPdf, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al exportar PDF: {ex.Message}" });
            }
        }

        /// <summary>
        /// Verifica conexión con la base de datos
        /// </summary>
        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var puedeConectar = await _contexto.Database.CanConnectAsync();
                
                if (!puedeConectar)
                {
                    return StatusCode(503, new
                    {
                        mensaje = "No se pudo conectar a la base de datos",
                        estado = "ERROR",
                        timestamp = DateTime.Now
                    });
                }

                var totalEstudiantes = await _contexto.Estudiantes.CountAsync();
                
                return Ok(new
                {
                    mensaje = "Conexión exitosa",
                    estado = "OK",
                    timestamp = DateTime.Now,
                    estudiantesEnBaseDatos = totalEstudiantes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    mensaje = $"Error: {ex.Message}",
                    estado = "ERROR",
                    timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// Obtiene un estudiante por ID
        /// </summary>
        [HttpGet("estudiante/{id}")]
        public async Task<IActionResult> GetEstudiante(int id)
        {
            try
            {
                var estudiante = await _contexto.Estudiantes.FindAsync(id);
                
                if (estudiante == null)
                {
                    return NotFound(new { mensaje = $"Estudiante con ID {id} no encontrado" });
                }

                var estudianteDto = new
                {
                    EstudianteId = estudiante.EstudianteId,
                    NombreCompleto = estudiante.NombreCompleto,
                    FechaNacimiento = estudiante.FechaNacimiento,
                    Peso = estudiante.Peso,
                    Altura = estudiante.Altura,
                    Sexo = estudiante.SexoString
                };

                return Ok(estudianteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al obtener estudiante: {ex.Message}" });
            }
        }
    }
}
