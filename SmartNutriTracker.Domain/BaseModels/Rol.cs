using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Rol
{
    [Key]
    public int RolId { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<Usuario>? Usuarios { get; set; }
}
