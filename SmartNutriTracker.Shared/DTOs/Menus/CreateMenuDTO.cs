using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Menus;

public class CreateMenuDTO
{
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Debe incluir al menos un alimento.")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un alimento.")]
    public List<int> AlimentoIds { get; set; } = new();
}
