using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.Loggers;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SmartNutriTracker.Back.Services.Logger;
//implementacion de ejemplo
//await _logService.LogAsync(
//    mensajeGlobal: Loggers_Globales.LOG_USUARIO_LOGOUT_EXITOSO,
//    entidad: "Usuario",
//    ip: ipAddress,
//    usuarioId: usuarioId,
//    rol: rolClaim  // ← Incluso si viene NULL, el LogService lo buscará
//);
public class LogService : ILogService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LogService> _logger;

    public LogService(ApplicationDbContext context, ILogger<LogService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private string ConvertirAIPv4(string? ip)
    {
        if (string.IsNullOrEmpty(ip))
            return "127.0.0.1";

        try
        {
            if (IPAddress.TryParse(ip, out var ipAddress))
            {
                if (ip == "::1")
                {
                    return "127.0.0.1";
                }

                if (ipAddress.IsIPv4MappedToIPv6)
                {
                    return ipAddress.MapToIPv4().ToString();
                }

                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return ip;
        }
        catch
        {
            return ip ?? "127.0.0.1";
        }
    }

    private async Task<string?> ObtenerRolSiEsNecesarioAsync(int? usuarioId, string? rol)
    {
        if (!string.IsNullOrEmpty(rol))
            return rol;

        if (!usuarioId.HasValue)
            return null;

        try
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId.Value);

            return usuario?.Rol?.Nombre;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo obtener el rol del usuario {UsuarioId}", usuarioId);
            return null;
        }
    }

    public async Task RegistrarLogAsync(
        LogLevel level,
        string detalle,
        string? entidad = null,
        string? ip = null,
        int? usuarioId = null,
        string? rol = null)
    {
        try
        {
            var (tipoAccionId, resultadoId) = await ObtenerTiposPorNivelAsync(level);

            var ipv4 = ConvertirAIPv4(ip);
            var rolFinal = await ObtenerRolSiEsNecesarioAsync(usuarioId, rol);

            var log = new Log
            {
                TipoAccionId = tipoAccionId,
                ResultadoId = resultadoId,
                Fecha = DateTime.UtcNow,
                UsuarioId = usuarioId,
                Rol = rolFinal,
                Entidad = entidad,
                Detalle = detalle,
                IP = ipv4
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            _logger.Log(level, 
                "[{Entidad}] {Detalle} | Usuario: {UsuarioId} | Rol: {Rol} | IP: {IP}", 
                entidad ?? "Sistema", 
                detalle,
                usuarioId?.ToString() ?? "N/A",
                rolFinal ?? "N/A",
                ipv4);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar log en base de datos: {Detalle}", detalle);
        }
    }

    public async Task LogAsync(
        string mensajeGlobal, 
        string? entidad = null,
        string? ip = null,
        int? usuarioId = null,
        string? rol = null)
    {
        var (level, mensajeLimpio) = ExtraerNivelYMensaje(mensajeGlobal);
        var entidadFinal = entidad ?? InferirEntidadDesdeLlamador();

        await RegistrarLogAsync(level, mensajeLimpio, entidadFinal, ip, usuarioId, rol);
    }

    private string InferirEntidadDesdeLlamador()
    {
        try
        {
            var stackTrace = new StackTrace();
            
            for (int i = 2; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame?.GetMethod();
                
                if (method?.DeclaringType != null)
                {
                    var typeName = method.DeclaringType.Name;
                    
                    if (typeName.EndsWith("Controller"))
                    {
                        return typeName.Replace("Controller", "");
                    }
                    
                    if (typeName.EndsWith("Service") && !typeName.Contains("LogService"))
                    {
                        return typeName.Replace("Service", "");
                    }
                }
            }
            
            return "Sistema";
        }
        catch
        {
            return "Sistema";
        }
    }

    private (LogLevel level, string mensajeLimpio) ExtraerNivelYMensaje(string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
            return (LogLevel.Information, mensaje);

        var separatorIndex = mensaje.IndexOf(',');
        if (separatorIndex <= 0)
            return (LogLevel.Information, mensaje);

        var prefijo = mensaje.Substring(0, separatorIndex).Trim().ToLowerInvariant();
        var mensajeLimpio = mensaje.Substring(separatorIndex + 1).Trim();

        var level = prefijo switch
        {
            "info" => LogLevel.Information,
            "success" => LogLevel.Information,
            "warning" => LogLevel.Warning,
            "error" => LogLevel.Error,
            "critical" => LogLevel.Critical,
            "debug" => LogLevel.Debug,
            "trace" => LogLevel.Trace,
            _ => LogLevel.Information
        };

        return (level, mensajeLimpio);
    }

    private async Task<(int tipoAccionId, int resultadoId)> ObtenerTiposPorNivelAsync(LogLevel level)
    {
        string nombreAccion = level switch
        {
            LogLevel.Trace => "Trace",
            LogLevel.Debug => "Debug",
            LogLevel.Information => "Información",
            LogLevel.Warning => "Advertencia",
            LogLevel.Error => "Error",
            LogLevel.Critical => "Crítico",
            _ => "Información"
        };

        string nombreResultado = level switch
        {
            LogLevel.Error or LogLevel.Critical => "Fallo",
            LogLevel.Warning => "Advertencia",
            _ => "Éxito"
        };

        var tipoAccion = await _context.TiposAccion
            .Where(t => t.Nombre.Trim().ToLower() == nombreAccion.Trim().ToLower())
            .FirstOrDefaultAsync();

        if (tipoAccion == null)
        {
            tipoAccion = new TipoAccion { Nombre = nombreAccion.Trim() };
            _context.TiposAccion.Add(tipoAccion);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("TipoAccion creado: {Nombre} con ID {Id}", tipoAccion.Nombre, tipoAccion.TipoAccionId);
        }

        var tipoResultado = await _context.TiposResultado
            .Where(t => t.Nombre.Trim().ToLower() == nombreResultado.Trim().ToLower())
            .FirstOrDefaultAsync();

        if (tipoResultado == null)
        {
            tipoResultado = new TipoResultado { Nombre = nombreResultado.Trim() };
            _context.TiposResultado.Add(tipoResultado);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("TipoResultado creado: {Nombre} con ID {Id}", tipoResultado.Nombre, tipoResultado.TipoResultadoId);
        }

        return (tipoAccion.TipoAccionId, tipoResultado.TipoResultadoId);
    }
}