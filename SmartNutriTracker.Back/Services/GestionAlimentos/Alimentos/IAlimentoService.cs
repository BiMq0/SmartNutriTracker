using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Services.Alimentos
{
    public interface IAlimentoService
    {
        Task<IEnumerable<AlimentoDTO>> GetAllAsync();
        Task<AlimentoDTO?> GetByIdAsync(int id);
        Task<AlimentoDTO> CreateAsync(CreateAlimentoDTO dto);
        Task<AlimentoDTO?> UpdateAsync(UpdateAlimentoDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
