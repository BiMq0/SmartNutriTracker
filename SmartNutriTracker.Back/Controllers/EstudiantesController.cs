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
}