using SmartNutriTracker.Domain.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Menus
{
    public class UpdateMenuDTO
    {
        public UpdateMenuDTO() { }

        public UpdateMenuDTO(Menu entity)
        {
            Fecha = entity.Fecha;
            AlimentoIds = entity.MenuAlimentos?
                .Select(ma => ma.AlimentoId)
                .ToList() ?? new();
        }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> AlimentoIds { get; set; } = new();
    }
}
