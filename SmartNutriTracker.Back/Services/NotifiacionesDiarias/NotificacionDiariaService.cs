using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Notificaciones;

using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Services.Notificaciones;

public class NotificacionDiariaService : INotificacionDiariaService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificacionDiariaService> _logger;

    public NotificacionDiariaService(ApplicationDbContext context, ILogger<NotificacionDiariaService> logger)
    {
        _context = context;
        _logger = logger;
    }


    /// <summary>
    /// Facilito, obtienes todos los recordatorios diarios pendientes.
    /// </summary>
    /// <returns>Lista de recordatorios diarios.</returns>
    public async Task<List<NotificacionDiariaDTO>> ObtenerPendientesDiariosAsync()
    {
        _logger.LogInformation("Inicio del proceso: ObtenerPendientesDiariosAsync realizado hoy {Fecha}", DateTime.Today);
        var hoy = DateTime.Today;
        
        var estudiantes = await _context.Estudiantes
            .Include(e => e.RegistroHabitos)
                .ThenInclude(rh => rh.RegistroAlimentos)
                .ThenInclude(ra => ra.Alimento)
            .ToListAsync();

        var recordatorios = new List<NotificacionDiariaDTO>();

        foreach (var est in estudiantes)
        {
            var dto = ProcesarEstudiante(est, hoy);
            if (dto != null)
            {
                recordatorios.Add(dto);
            }
        }

        _logger.LogInformation("Se encontraron {Cantidad} recordatorios pendientes.", recordatorios.Count);
        return recordatorios;
    }

    /// <summary>
    /// Lo mismo que el anterior pero este solo retorna el recordatorio diario pendiente para un estudiante por su ID.
    /// </summary>
    /// <param name="estudianteId">ID del estudiante.</param>
    /// <returns>Recordatorio diario pendiente para el estudiante.</returns>
    public async Task<NotificacionDiariaDTO?> ObtenerRecordatorioPorEstudianteAsync(int estudianteId)
    {
        _logger.LogInformation("Buscando recordatorio para estudiante con ID {EstudianteId}", estudianteId);
        var hoy = DateTime.Today;

        var estudiante = await _context.Estudiantes
            .Include(e => e.RegistroHabitos)
                .ThenInclude(rh => rh.RegistroAlimentos)
                .ThenInclude(ra => ra.Alimento)
            .FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);

        if (estudiante == null)
        {
            _logger.LogWarning("Estudiante con ID {EstudianteId} no encontrado.", estudianteId);
            return null;
        }

        var recordatorio = ProcesarEstudiante(estudiante, hoy);
        
        if (recordatorio != null)
        {
            _logger.LogInformation("Recordatorio generado para estudiante {Nombre}.", estudiante.NombreCompleto);
        }

        return recordatorio;
    }

    /// <summary>
    /// Esto sólo valida y procesa a un estudiante para generar un recordatorio antes de "mandarlo" para evitar errores.
    /// </summary>  
    /// <param name="estudiante">Estudiante a procesar.</param>
    /// <param name="hoy">Fecha actual.</param>
    /// <returns>Recordatorio diario para el estudiante.</returns>
    private NotificacionDiariaDTO? ProcesarEstudiante(Estudiante est, DateTime hoy)
    {
        var registroHoy = est.RegistroHabitos?.FirstOrDefault(rh => rh.Fecha.Date == hoy);

        if (registroHoy == null)
        {
            return new NotificacionDiariaDTO
            {
                EstudianteId = est.EstudianteId,
                NombreEstudiante = est.NombreCompleto,
                Mensaje = $"Hola {est.NombreCompleto}, recuerda registrar tus hábitos de hoy.",
                FechaIntento = DateTime.Now
            };
        }
        else
        {
            var mensajes = GenerarMensajesPersonalizados(registroHoy);
            
            if (mensajes.Count > 0)
            {
                var mensajeCompleto = $"Hola {est.NombreCompleto}, " + string.Join(" ", mensajes);
                
                return new NotificacionDiariaDTO
                {
                    EstudianteId = est.EstudianteId,
                    NombreEstudiante = est.NombreCompleto,
                    Mensaje = mensajeCompleto,
                    FechaIntento = DateTime.Now
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Dependiendo de los registros de hábitos de un estudiante, crea mensajes personalizados así bien sabrosos.
    /// </summary>
    /// <param name="registro">Registro de hábitos.</param>
    /// <returns>Lista de mensajes personalizados.</returns>
    private List<string> GenerarMensajesPersonalizados(RegistroHabito registro)
    {
        var mensajes = new List<string>();

        if (registro.HorasSueno > 0)
        {
            mensajes.Add($"no olvides que tu meta es dormir {registro.HorasSueno:0.##} horas.");
        }

        if (registro.HorasActividadFisica > 0)
        {
            mensajes.Add($"recuerda hacer tus {registro.HorasActividadFisica:0.##} horas de ejercicio.");
        }

        if (registro.RegistroAlimentos != null && registro.RegistroAlimentos.Any())
        {
            foreach (var regAlimento in registro.RegistroAlimentos)
            {
                if (regAlimento.Alimento != null)
                {
                    var cantidadTexto = regAlimento.Cantidad.HasValue ? $"{regAlimento.Cantidad} " : "";
                    mensajes.Add($"no olvides comer {cantidadTexto}{regAlimento.Alimento.Nombre}.");
                }
            }
        }
        return mensajes;
    }
}