
using SmartNutriTracker.Shared.DTOs.Habitos;

namespace SmartNutriTracker.Back.Services.Habitos
{
    public interface IRegistroHabitoService
    {

        Task<bool> RegistrarHabitosAsync(RegistroHabitoNuevoDTO registro);
        

        Task<List<RegistroHabitoDTO>> ObtenerPorEstudianteAsync(int estudianteId);
        

        Task<List<RegistroHabitoDTO>> ObtenerHabitosPorEstudianteAsync(int estudianteId);
        

        Task<List<RegistroHabitoDTO>> ObtenerConsumoPorEstudianteAsync(int estudianteId);
        

        Task<RegistroHabitoDTO?> ObtenerHabitoPorIdAsync(int registroHabitoId);
        

        Task<bool> ActualizarRegistroAsync(int registroHabitoId, RegistroHabitoActualizarDTO dto);
    }
}