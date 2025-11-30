using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Services.Estudiantes;

public interface IEstudianteService
{
    Task<Estudiante> RegistrarEstudianteAsync(EstudianteRegistroDTO dto);
    Task<bool> ActualizarPerfilEstudianteAsync(int id, EstudianteUpdateDTO dto);
    Task<PerfilEstudianteDTO?> ObtenerPerfilAsync(int id);
}