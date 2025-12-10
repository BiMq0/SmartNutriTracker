using SmartNutriTracker.Back.Database;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuditService> _logger;

        public AuditService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AuditService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public async Task LogAsync(string accion, string nivel, string detalle = "")
        {
            try
            {
                string usuario = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? "anonimo";

                nivel = nivel?.ToUpper() switch
                {
                    "INFO" => "INFO",
                    "WARNING" => "WARNING",
                    "ERROR" => "ERROR",
                    _ => "INFO"
                };

                var log = new AuditLog
                {
                    NombreUsuario = usuario,
                    Accion = accion ?? "Acción no especificada",
                    Nivel = nivel,
                    Detalle = detalle ?? string.Empty,
                    Fecha = DateTime.UtcNow
                };

                _context.AuditLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando auditoría: {Message}", ex.Message);
            }
        }
    }
}
