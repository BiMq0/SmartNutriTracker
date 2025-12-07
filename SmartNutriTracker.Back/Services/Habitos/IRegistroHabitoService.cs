// .back/Services/Habitos/IRegistroHabitoService.cs
using SmartNutriTracker.Shared.DTOs.Habitos;

namespace SmartNutriTracker.Back.Services.Habitos
{
    public interface IRegistroHabitoService
    {
        // Para registrar solo hábitos
        Task<bool> RegistrarHabitosAsync(RegistroHabitoNuevoDTO registro);
        
        // Para obtener todos los registros
        Task<List<RegistroHabitoDTO>> ObtenerPorEstudianteAsync(int estudianteId);
        
        // Para obtener solo hábitos
        Task<List<RegistroHabitoDTO>> ObtenerHabitosPorEstudianteAsync(int estudianteId);
        
        // Para obtener solo consumo
        Task<List<RegistroHabitoDTO>> ObtenerConsumoPorEstudianteAsync(int estudianteId);
        
        // ✅ AGREGAR ESTE MÉTODO
        Task<RegistroHabitoDTO?> ObtenerHabitoPorIdAsync(int registroHabitoId);
        
        // Para actualizar
        Task<bool> ActualizarRegistroAsync(int registroHabitoId, RegistroHabitoActualizarDTO dto);
    }
}