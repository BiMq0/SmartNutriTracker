using SmartNutriTracker.Back.Database;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System;
using System.Threading.Tasks;



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

        var imc = CalcularIMC(dto.Peso, dto.Altura);
        var tmb = CalcularTMB(dto.Peso, dto.Altura, dto.Edad, dto.Sexo);

        var estudiante = new Estudiante
        {
            NombreCompleto = dto.NombreCompleto, // ← CORREGIDO
            Edad = dto.Edad,
            Sexo = dto.Sexo,
            Peso = dto.Peso,
            Altura = dto.Altura,
            IMC = imc,
            TMB = tmb
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