using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartNutriTracker.Domain.Models.BaseModels;

public class Alimento
{
    [Key]
    public int AlimentoId { get; set; }
    public string Nombre { get; set; } = null!;
    public int Calorias { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Proteinas { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Carbohidratos { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    public decimal Grasas { get; set; }

    public ICollection<MenuAlimento>? MenuAlimentos { get; set; }
    public ICollection<RegistroAlimento>? RegistroAlimentos { get; set; }
}
