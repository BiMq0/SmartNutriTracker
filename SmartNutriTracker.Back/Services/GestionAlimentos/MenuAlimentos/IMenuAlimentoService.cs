using SmartNutriTracker.Shared.DTOs.MenuAlimentos;

namespace SmartNutriTracker.Back.Services.MenuAlimentos;

public interface IMenuAlimentoService
{
    Task<bool> AssignFoodToMenuAsync(CreateMenuAlimentoDTO dto);
    Task<bool> RemoveFoodFromMenuAsync(int menuAlimentoId);
}
