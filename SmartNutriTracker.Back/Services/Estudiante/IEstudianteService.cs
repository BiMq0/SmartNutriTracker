using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Services.Estudiantes;

public interface IEstudianteService
{
    Task<Estudiante> RegistrarEstudianteAsync(EstudianteRegistroDTO dto);
    decimal CalcularIMC(decimal peso, decimal altura);
    decimal CalcularTMB(decimal peso, decimal altura, int edad, string sexo);
}