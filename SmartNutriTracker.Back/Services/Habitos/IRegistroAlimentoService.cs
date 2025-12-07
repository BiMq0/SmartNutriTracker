// .back/Services/Alimentos/IRegistroAlimentoService.cs
using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Services.Alimentos
{
    public interface IRegistroAlimentoService
    {
        Task<bool> RegistrarConsumoAsync(RegistroConsumoNuevoDTO registro);
        Task<List<RegistroConsumoDTO>> ObtenerPorEstudianteAsync(int estudianteId);
        Task<RegistroConsumoDTO?> ObtenerPorIdAsync(int registroHabitoId);
        Task<bool> ActualizarConsumoAsync(int registroHabitoId, RegistroConsumoActualizarDTO dto);
    }
}