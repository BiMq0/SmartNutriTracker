using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Nutrition;
using SmartNutriTracker.Back.Database;
using System.Text;
using System.Text.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly INutritionRecommendationService _nutritionRecommendationService;
        private readonly ApplicationDbContext _db;
        private readonly IPdfExportService _pdfExportService;
        private readonly INutritionValidator _nutritionValidator;

        public NutritionController(INutritionService nutritionService, INutritionRecommendationService nutritionRecommendationService, ApplicationDbContext db, IPdfExportService pdfExportService, INutritionValidator nutritionValidator)
        {
            _nutritionService = nutritionService;
            _nutritionRecommendationService = nutritionRecommendationService;
            _db = db;
            _pdfExportService = pdfExportService;
            _nutritionValidator = nutritionValidator;
        }

        private async Task<int?> TryResolveEstudianteIdFromRequestAsync(NutritionRequestDTO request)
        {
            try
            {
                // 1) Revisar claims del usuario (si existe autenticación configurada)
                var user = HttpContext?.User;
                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    // Buscar claim con nombre directo
                    var claimNames = new[] { "estudianteId", "estudiante_id", "EstudianteId", "sub", "id", "usuarioId", "usuario_id" };
                    foreach (var c in claimNames)
                    {
                        var claim = user.FindFirst(c);
                        if (claim != null && int.TryParse(claim.Value, out var idVal))
                        {
                            var exists = await _db.Estudiantes.AnyAsync(e => e.EstudianteId == idVal);
                            if (exists) return idVal;
                        }
                    }

                    // Buscar nombre u email en claims
                    var nameClaim = user.FindFirst("name") ?? user.FindFirst("preferred_username") ?? user.FindFirst("username") ?? user.FindFirst("email");
                    if (nameClaim != null)
                    {
                        var name = nameClaim.Value.Trim();
                        if (!string.IsNullOrEmpty(name))
                        {
                            var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => EF.Functions.ILike(e.NombreCompleto, $"%{name}%"));
                            if (estudiante != null) return estudiante.EstudianteId;
                        }
                    }
                }

                // 2) Si no hay claims útiles, intentar decodificar el payload del Bearer token (sin validar firma)
                if (Request.Headers.TryGetValue("Authorization", out var authValues))
                {
                    var auth = authValues.FirstOrDefault();
                    if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Bearer "))
                    {
                        var token = auth.Substring("Bearer ".Length).Trim();
                        var parts = token.Split('.');
                        if (parts.Length >= 2)
                        {
                            var payload = parts[1];
                            // base64url -> base64
                            payload = payload.Replace('-', '+').Replace('_', '/');
                            switch (payload.Length % 4)
                            {
                                case 2: payload += "=="; break;
                                case 3: payload += "="; break;
                            }
                            try
                            {
                                var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                                using var doc = JsonDocument.Parse(json);
                                if (doc.RootElement.TryGetProperty("estudianteId", out var estIdEl) && estIdEl.ValueKind == JsonValueKind.Number)
                                {
                                    var id = estIdEl.GetInt32();
                                    var exists = await _db.Estudiantes.AnyAsync(e => e.EstudianteId == id);
                                    if (exists) return id;
                                }

                                // buscar name/email en payload
                                string? maybeName = null;
                                if (doc.RootElement.TryGetProperty("name", out var n1) && n1.ValueKind == JsonValueKind.String) maybeName = n1.GetString();
                                if (maybeName == null && doc.RootElement.TryGetProperty("email", out var n2) && n2.ValueKind == JsonValueKind.String) maybeName = n2.GetString();
                                if (maybeName != null)
                                {
                                    var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => EF.Functions.ILike(e.NombreCompleto, $"%{maybeName}%"));
                                    if (estudiante != null) return estudiante.EstudianteId;
                                }
                            }
                            catch
                            {
                                // Ignorar errores de parsing
                            }
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] NutritionRequestDTO request)
        {
            try
            {
                // Si no viene EstudianteId, intentar resolverlo desde el token/claims
                if (!request.EstudianteId.HasValue)
                {
                    var resolved = await TryResolveEstudianteIdFromRequestAsync(request);
                    if (resolved.HasValue)
                    {
                        request.EstudianteId = resolved.Value;
                    }
                }

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
                // Validar datos de entrada
                var validationResult = _nutritionValidator.ValidateNutritionRequest(solicitud);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { 
                        mensaje = "Datos inválidos", 
                        errores = validationResult.Errores 
                    });
                }

                // Si no viene EstudianteId, intentar resolverlo desde el token/claims
                if (!solicitud.EstudianteId.HasValue)
                {
                    var resolved = await TryResolveEstudianteIdFromRequestAsync(solicitud);
                    if (resolved.HasValue)
                    {
                        solicitud.EstudianteId = resolved.Value;
                    }
                }

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

        [HttpPost("export-pdf")]
        public async Task<IActionResult> ExportToPdf([FromBody] ResultadoRecomendacionNutricionalDTO recomendacion)
        {
            try
            {
                if (recomendacion == null)
                {
                    return BadRequest(new { mensaje = "Recomendación no proporcionada." });
                }

                var pdfBytes = await _pdfExportService.ExportRecomendacionesToPdfAsync(recomendacion, recomendacion.ResultadoNutricional);
                return File(pdfBytes, "application/pdf", $"Recomendacion_Nutricional_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al exportar PDF: {ex.Message}" });
            }
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                // Intentar ejecutar una query simple para validar conexión a BD
                var canConnect = await _db.Database.CanConnectAsync();
                if (canConnect)
                {
                    // Contar registros en Estudiantes para validar acceso
                    var estudianteCount = await _db.Estudiantes.CountAsync();
                    return Ok(new
                    {
                        mensaje = "Conexión a base de datos exitosa.",
                        estado = "OK",
                        timestamp = DateTime.Now,
                        estudiantesEnBaseDatos = estudianteCount
                    });
                }
                else
                {
                    return StatusCode(503, new
                    {
                        mensaje = "No se pudo conectar a la base de datos.",
                        estado = "ERROR",
                        timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    mensaje = $"Error al validar conexión a BD: {ex.Message}",
                    estado = "ERROR",
                    timestamp = DateTime.Now
                });
            }
        }
    }
}
