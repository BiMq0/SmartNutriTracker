using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Log
{
    [Key]
    public int LogId { get; set; }
    public int TipoAccionId { get; set; }
    public int ResultadoId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int? UsuarioId { get; set; }
    public string? Rol { get; set; }
    public string? Entidad { get; set; }
    public string Detalle { get; set; } = null!;
    public string? IP { get; set; }

    public Usuario? Usuario { get; set; }
    public TipoAccion? TipoAccion { get; set; }
    public TipoResultado? Resultado { get; set; }
}
