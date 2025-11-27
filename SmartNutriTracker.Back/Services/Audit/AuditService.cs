using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SmartNutriTracker.Back.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AuditService(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task LogAsync(string accion, string nivel, string detalle = "")
        {
            var usuario = _httpContext?
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonimo";

            var log = new AuditLog
            {
                Usuario = usuario,
                Accion = accion,
                Nivel = nivel.ToUpper(), // INFO, WARNING, ERROR
                Detalle = detalle,
                Fecha = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
