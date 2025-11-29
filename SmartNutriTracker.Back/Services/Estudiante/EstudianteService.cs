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

    public async Task<bool> ActualizarPerfilEstudianteAsync(int id, EstudianteUpdateDTO dto)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);

        if (estudiante == null)
        {
            return false; // El estudiante no fue encontrado
        }

        // Aplicar actualizaciones solo si el DTO proporciona un valor (actualización parcial)
        if (dto.NombreCompleto != null)
        {
            estudiante.NombreCompleto = dto.NombreCompleto;
        }
        if (dto.Edad.HasValue)
        {
            estudiante.Edad = dto.Edad.Value;
        }
        if (dto.Sexo != null)
        {
            estudiante.Sexo = dto.Sexo;
        }
        if (dto.Peso.HasValue)
        {
            estudiante.Peso = dto.Peso.Value;
        }
        if (dto.Altura.HasValue)
        {
            estudiante.Altura = dto.Altura.Value;
        }

        // Recalcular IMC y TMB si los datos físicos relevantes han cambiado
        if (dto.Peso.HasValue || dto.Altura.HasValue || dto.Edad.HasValue || dto.Sexo != null)
        {
            // Usa los valores actualizados o los existentes si no se actualizaron
            var pesoActual = dto.Peso ?? estudiante.Peso;
            var alturaActual = dto.Altura ?? estudiante.Altura;
            var edadActual = dto.Edad ?? estudiante.Edad;
            var sexoActual = dto.Sexo ?? estudiante.Sexo;

            estudiante.IMC = CalcularIMC(pesoActual, alturaActual);
            estudiante.TMB = CalcularTMB(pesoActual, alturaActual, edadActual, sexoActual);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
