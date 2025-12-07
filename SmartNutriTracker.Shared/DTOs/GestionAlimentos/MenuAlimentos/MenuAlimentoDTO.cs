using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Shared.DTOs.MenuAlimentos
{
    public class MenuAlimentoDTO
    {
        public int MenuAlimentoId { get; set; }
        public int MenuId { get; set; }
        public int AlimentoId { get; set; }

        public MenuAlimentoDTO() { }

        public MenuAlimentoDTO(MenuAlimento entity)
        {
            MenuAlimentoId = entity.MenuAlimentoId;
            MenuId = entity.MenuId;
            AlimentoId = entity.AlimentoId;
        }
    }
}
