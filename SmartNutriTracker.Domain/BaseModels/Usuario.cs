using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RolId { get; set; }

    public Rol Rol { get; set; } = null!;
    public ICollection<Log>? Logs { get; set; }
}
