using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Services.Logger;

public interface ILogService
{
    // Método principal (interno)
    Task RegistrarLogAsync(
        LogLevel level,
        string detalle,
        string? entidad = null,
        string? ip = null,
        int? usuarioId = null,
        string? rol = null);

    // Método simplificado - IP como TERCER parámetro
    Task LogAsync(
        string mensajeGlobal,
        string? entidad = null,
        string? ip = null,
        int? usuarioId = null,
        string? rol = null);
}