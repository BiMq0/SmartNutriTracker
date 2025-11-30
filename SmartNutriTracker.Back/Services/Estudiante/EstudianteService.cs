using SmartNutriTracker.Back.Database;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System;
using System.Threading.Tasks;
using SmartNutriTracker.Domain.Statics;


namespace SmartNutriTracker.Back.Services.Estudiantes;

public class EstudianteService : IEstudianteService
{
    private readonly ApplicationDbContext _context;

    public EstudianteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Estudiante> RegistrarEstudianteAsync(EstudianteRegistroDTO dto)
    {
        if (dto.Peso <= 0 || dto.Altura <= 0)
            throw new ArgumentException("Peso y altura deben ser mayores a 0");

        var estudiante = new Estudiante
        {
            NombreCompleto = dto.NombreCompleto,
            FechaNacimiento = dto.FechaNacimiento, 
            Sexo = dto.Sexo, 
            Peso = dto.Peso,
            Altura = dto.Altura,
            IMC = 0
        };

        _context.Estudiantes.Add(estudiante);
        await _context.SaveChangesAsync();

        return estudiante;
    }

    public async Task<PerfilEstudianteDTO?> ObtenerPerfilAsync(int id)
    {
        return await _context.Estudiantes
            .Where(x => x.EstudianteId == id)
            .Select(x => new PerfilEstudianteDTO
            {
                EstudianteId = x.EstudianteId,
                NombreCompleto = x.NombreCompleto,
                Edad = x.Edad,
                Sexo = x.Sexo,
                Peso = x.Peso,
                Altura = x.Altura,
                IMC = x.IMC,
                TMB = x.TMB
            })
            .FirstOrDefaultAsync();
    }


    public async Task<bool> ActualizarPerfilEstudianteAsync(int id, EstudianteUpdateDTO dto)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);

        if (estudiante == null)
        {
            return false;
        }

        // CORREGIDO: Solo campos que se pueden modificar
        if (dto.NombreCompleto != null)
        {
            estudiante.NombreCompleto = dto.NombreCompleto;
        }
        // CORREGIDO: Usar FechaNacimiento en lugar de Edad
        if (dto.FechaNacimiento.HasValue)
        {
            estudiante.FechaNacimiento = dto.FechaNacimiento.Value;
        }
        // CORREGIDO: Sexo es enum, no string
        if (dto.Sexo.HasValue)
        {
            estudiante.Sexo = dto.Sexo.Value;
        }
        if (dto.Peso.HasValue)
        {
            estudiante.Peso = dto.Peso.Value;
        }
        if (dto.Altura.HasValue)
        {
            estudiante.Altura = dto.Altura.Value;
        }

        // CORREGIDO: ELIMINAR recalculo de IMC y TMB - se calculan automáticamente
        // IMC y TMB son propiedades calculadas [NotMapped] en el modelo

        await _context.SaveChangesAsync();
        return true;
    }
    public decimal CalcularIMC(decimal peso, decimal altura)
    {
        return peso / (altura * altura);
    }

    public decimal CalcularTMB(decimal peso, decimal altura, int edad, string sexo)
    {
        if (sexo.ToLower() == "masculino")
            return 10 * peso + 6.25m * (altura * 100) - 5 * edad + 5;
        else
            return 10 * peso + 6.25m * (altura * 100) - 5 * edad - 161;
    }
}
