using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using System.Diagnostics;

namespace SmartNutriTracker.Back.Services.Logger;

public class LogService : ILogService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LogService> _logger;

    public LogService(ApplicationDbContext context, ILogger<LogService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ✅ Método principal de logging - ORDEN ACTUALIZADO
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

            var log = new Log
            {
                TipoAccionId = tipoAccionId,
                ResultadoId = resultadoId,
                Fecha = DateTime.UtcNow,
                UsuarioId = usuarioId,
                Rol = rol,
                Entidad = entidad,
                Detalle = detalle,
                IP = ip
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            // ✅ Log mejorado en consola con TODOS los datos
            _logger.Log(level, 
                "[{Entidad}] {Detalle} | Usuario: {UsuarioId} | Rol: {Rol} | IP: {IP}", 
                entidad ?? "Sistema", 
                detalle,
                usuarioId?.ToString() ?? "N/A",
                rol ?? "N/A",
                ip ?? "N/A");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar log en base de datos: {Detalle}", detalle);
        }
    }

    // ✅ Método simplificado - ORDEN ACTUALIZADO
    public async Task LogAsync(
        string mensajeGlobal, 
        string? entidad = null,
        string? ip = null,          
        int? usuarioId = null,      
        string? rol = null)         
    {
        // 1. Extraer el nivel del mensaje
        var (level, mensajeLimpio) = ExtraerNivelYMensaje(mensajeGlobal);

        // 2. Si no se proporciona entidad, inferirla del llamador
        var entidadFinal = entidad ?? InferirEntidadDesdeLlamador();

        // 3. ✅ Llamar al método principal con el NUEVO ORDEN de parámetros
        await RegistrarLogAsync(level, mensajeLimpio, entidadFinal, ip, usuarioId, rol);
    }

    // Método privado para inferir la entidad desde el stack trace
    private string InferirEntidadDesdeLlamador()
    {
        try
        {
            var stackTrace = new StackTrace();
            
            // Recorrer el stack para encontrar el controlador o servicio que llamó
            for (int i = 2; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame?.GetMethod();
                
                if (method?.DeclaringType != null)
                {
                    var typeName = method.DeclaringType.Name;
                    
                    // Detectar si es un Controller
                    if (typeName.EndsWith("Controller"))
                    {
                        return typeName.Replace("Controller", "");
                    }
                    
                    // Detectar si es un Service
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

    // Método privado que extrae el nivel del prefijo del mensaje
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

    // Método privado para obtener tipos por nivel
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
            .FirstOrDefaultAsync(t => t.Nombre == nombreAccion);

        if (tipoAccion == null)
        {
            tipoAccion = new TipoAccion { Nombre = nombreAccion };
            _context.TiposAccion.Add(tipoAccion);
            await _context.SaveChangesAsync();
        }

        var tipoResultado = await _context.TiposResultado
            .FirstOrDefaultAsync(t => t.Nombre == nombreResultado);

        if (tipoResultado == null)
        {
            tipoResultado = new TipoResultado { Nombre = nombreResultado };
            _context.TiposResultado.Add(tipoResultado);
            await _context.SaveChangesAsync();
        }

        return (tipoAccion.TipoAccionId, tipoResultado.TipoResultadoId);
    }
}