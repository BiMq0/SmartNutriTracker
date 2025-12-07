using SmartNutriTracker.Shared.DTOs.Habitos; // ← CAMBIAR namespace
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Services.Habitos
{
    public interface IHabitosService
    {
        Task<List<HabitoRegistroDTO>> ObtenerHabitosAsync(int estudianteId); // ← Cambiar tipo
    }
}