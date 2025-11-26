using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Services.Alimentos;

public interface IAlimentoService
{
    Task<List<AlimentoDTO>> GetAllAsync();
    Task<AlimentoDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAlimentoDTO dto);
    Task<bool> UpdateAsync(int id, UpdateAlimentoDTO dto);
    Task<bool> DeleteAsync(int id);
}
