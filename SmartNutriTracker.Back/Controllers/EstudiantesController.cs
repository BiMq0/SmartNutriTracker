using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Estudiantes;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System;
using System.Threading.Tasks;
// ELIMINAR este using: using SmartNutriTracker.Shared.Endpoints;

namespace SmartNutriTracker.Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;
    private readonly ApplicationDbContext _context;

    public EstudiantesController(IEstudianteService estudianteService, ApplicationDbContext context)
    {
        _estudianteService = estudianteService;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarEstudiante([FromBody] EstudianteRegistroDTO dto)
    {
        try
        {
            var estudiante = await _estudianteService.RegistrarEstudianteAsync(dto);
            return Created($"/api/estudiantes/{estudiante.EstudianteId}", estudiante);
        }
        catch (Exception ex)
        {
            // MOSTRAR EL ERROR INTERNO COMPLETO
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return BadRequest($"Error al registrar estudiante: {errorMessage}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerEstudiantes()
    {
        var estudiantes = await _context.Estudiantes.ToListAsync();
        return Ok(estudiantes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerEstudiantePorId(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);
        return estudiante != null ? Ok(estudiante) : NotFound();
    }

    // GET api/estudiantes/perfil/3
    /*[HttpGet("perfil/{id}")]
    public async Task<IActionResult> ObtenerPerfilEstudiante(int id)
    {
        var perfil = await _estudianteService.ObtenerPerfilAsync(id);

        if (perfil == null)
            return NotFound($"El estudiante con ID {id} no existe.");

        return Ok(perfil);
    }*/

    [HttpGet("perfil/{id}")]
    public IActionResult ObtenerPerfilEstudianteFake(int id)
    {
        var perfilFake = new PerfilEstudianteDTO
        {
            EstudianteId = id,
            NombreCompleto = "Prueba Estudiante",
            Edad = 16,
            Sexo = "Masculino",
            Peso = 60,
            Altura = 1.70m,
            IMC = 20.76m,
            TMB = 1500
        };

        return Ok(perfilFake);
    }


}