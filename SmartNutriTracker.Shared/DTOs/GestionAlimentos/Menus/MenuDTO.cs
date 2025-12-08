using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Shared.DTOs.Menus
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public DateTime Fecha { get; set; }
        public List<AlimentoDTO> Alimentos { get; set; } = new();

        public MenuDTO() { }

        public MenuDTO(Menu entity)
        {
            MenuId = entity.MenuId;
            Fecha = entity.Fecha;

            Alimentos = entity.MenuAlimentos?
                .Select(ma => new AlimentoDTO(ma.Alimento!))
                .ToList() ?? new();
        }
    }
}
