using SmartNutriTracker.Shared.DTOs.Menus;

namespace SmartNutriTracker.Back.Services.Menus;

public interface IMenuService
{
    Task<List<MenuDTO>> GetAllAsync();
    Task<MenuDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateMenuDTO dto);
    Task<bool> UpdateAsync(int id, UpdateMenuDTO dto);
    Task<bool> DeleteAsync(int id);
}
