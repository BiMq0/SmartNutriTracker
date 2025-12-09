using SmartNutriTracker.Shared.DTOs.Habitos;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace SmartNutriTracker.Back.Services.Habitos
{
    public class HabitosService : IHabitosService
    {
        private readonly ApplicationDbContext _context;

        public HabitosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HabitoRegistroDTO>> ObtenerHabitosAsync(int estudianteId)
{
    try
    {
        var habitosBase = await _context.RegistroHabitos
            .Where(rh => rh.EstudianteId == estudianteId)
            .OrderByDescending(rh => rh.Fecha)
            .Select(rh => new 
            {
                rh.RegistroHabitoId,
                rh.Fecha,
                rh.HorasSueno,
                rh.HorasActividadFisica,
                rh.EstudianteId,
                Estudiante = new 
                {
                    rh.Estudiante!.EstudianteId,
                    rh.Estudiante.NombreCompleto,
                    rh.Estudiante.FechaNacimiento,
                    rh.Estudiante.Peso,
                    rh.Estudiante.Altura,
                    rh.Estudiante.IMC,
                    rh.Estudiante.Sexo
                },
                RegistroAlimentos = rh.RegistroAlimentos!.Select(ra => new
                {
                    ra.RegistroAlimentoId,
                    ra.AlimentoId,
                    ra.TipoComidaId,
                    ra.Cantidad,
                    Alimento = new 
                    {
                        ra.Alimento!.AlimentoId,
                        ra.Alimento.Nombre
                    },
                    TipoComida = new 
                    {
                        ra.TipoComida!.TipoComidaId,
                        ra.TipoComida.Nombre
                    }
                }).ToList()
            })
            .ToListAsync();

        var listahabitos = habitosBase.Select(u => new HabitoRegistroDTO
        {
            RegistroHabitoId = u.RegistroHabitoId,
            Fecha = u.Fecha,
            HorasSueno = u.HorasSueno,
            HorasActividadFisica = u.HorasActividadFisica,
            EstudianteId = u.EstudianteId,
            NombreEstudiante = u.Estudiante.NombreCompleto,
            FechaNacimiento = u.Estudiante.FechaNacimiento,
            Peso = u.Estudiante.Peso,
            Altura = u.Estudiante.Altura,
            IMC = u.Estudiante.IMC,
            Sexo = u.Estudiante.Sexo.ToString(),
            RegistroAlimentos = u.RegistroAlimentos.Select(ra => new RegistroAlimentoDetalleDTO
            {
                AlimentoId = ra.Alimento.AlimentoId,
                AlimentoNombre = ra.Alimento.Nombre,
                TipoComidaId = ra.TipoComida.TipoComidaId,
                TipoComidaNombre = ra.TipoComida.Nombre,
                Cantidad = ra.Cantidad
            }).ToList()
        }).ToList();

        return listahabitos;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en ObtenerHabitosAsync: {ex.Message}");
        throw;
    }
}
    }
}