using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database; // ← CORREGIR este using
using SmartNutriTracker.Back.Services.Estudiantes; // ← AGREGAR este using
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Domain.Statics;
using SmartNutriTracker.Shared.DTOs.Estudiantes;
using System;
using System.Threading.Tasks;
using System.Linq;

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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

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

    [HttpGet("{id}/detalle-completo")]
    public async Task<IActionResult> ObtenerDetalleCompleto(int id)
    {
        try
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.RegistroHabitos!)
                    .ThenInclude(rh => rh.RegistroAlimentos!)
                        .ThenInclude(ra => ra.Alimento)
                .Include(e => e.RegistroHabitos!)
                    .ThenInclude(rh => rh.RegistroAlimentos!)
                        .ThenInclude(ra => ra.TipoComida)
                .FirstOrDefaultAsync(e => e.EstudianteId == id);

            if (estudiante == null)
            {
                return NotFound($"Estudiante con ID {id} no encontrado.");
            }

            var resultado = new
            {
                EstudianteId = estudiante.EstudianteId,
                NombreCompleto = estudiante.NombreCompleto,
                Edad = estudiante.Edad,
                IMC = estudiante.IMC,
                RegistroHabitos = estudiante.RegistroHabitos?.Select(rh => new
                {
                    RegistroHabitoId = rh.RegistroHabitoId,
                    Fecha = rh.Fecha,
                    HorasSueno = rh.HorasSueno,
                    HorasActividadFisica = rh.HorasActividadFisica,
                    RegistroAlimentos = rh.RegistroAlimentos?.Select(ra => new
                    {
                        RegistroAlimentoId = ra.RegistroAlimentoId,
                        Alimento = ra.Alimento != null ? new
                        {
                            AlimentoId = ra.Alimento.AlimentoId,
                            Nombre = ra.Alimento.Nombre,
                            Calorias = ra.Alimento.Calorias,
                            Proteinas = ra.Alimento.Proteinas,
                            Carbohidratos = ra.Alimento.Carbohidratos,
                            Grasas = ra.Alimento.Grasas
                        } : null,
                        Cantidad = ra.Cantidad,
                        TipoComida = ra.TipoComida != null ? new
                        {
                            TipoComidaId = ra.TipoComida.TipoComidaId,
                            Nombre = ra.TipoComida.Nombre
                        } : null
                    }).ToList()
                }).ToList()
            };

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener detalle completo: {ex.Message}");
        }
    }

    // Tu método para actualizar el perfil
    [HttpPut("{id}/perfil")] 
    public async Task<IActionResult> ActualizarPerfilEstudiante(int id, [FromBody] EstudianteUpdateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Si el DTO no es válido
        }

        try
        {
            var actualizado = await _estudianteService.ActualizarPerfilEstudianteAsync(id, dto);

            if (actualizado)
            {
                return NoContent(); 
            }
            else
            {
                return NotFound($"Estudiante con ID {id} no encontrado.");
            }
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return StatusCode(500, $"Error interno del servidor al actualizar el perfil del estudiante: {errorMessage}");
        }
    }

    // El método del remoto para obtener perfil (fake/simulado)
    [HttpGet("perfil/{id}")]
    public IActionResult ObtenerPerfilEstudianteFake(int id)
    {
        // Nota: PerfilEstudianteDTO necesita ser definido en SmartNutriTracker.Shared/DTOs/Estudiantes/
        var perfilFake = new PerfilEstudianteDTO
        {
            EstudianteId = id,
            NombreCompleto = "Prueba Estudiante",
            Edad = 16,
            Sexo = Sexo.Varon,
            Peso = 60,
            Altura = 1.70m,
            IMC = 20.76m,
            TMB = 1500
        };

        return Ok(perfilFake);
    }
    [HttpGet("dashboard")]
    public async Task<IActionResult> ObtenerDashboardEstudiantes()
    {
        try
        {
            var estudiantes = await _context.Estudiantes
                .Select(e => new
                {
                    e.EstudianteId,
                    e.NombreCompleto,
                    e.IMC,
                    CategoriaIMC = e.IMC < 18.5m ? "Bajo peso" :        // ← Agregar 'm' para decimal
                                  e.IMC < 25m ? "Normal" :              // ← Agregar 'm' para decimal
                                  e.IMC < 30m ? "Sobrepeso" : "Obesidad", // ← Agregar 'm' para decimal
                    TieneAlerta = e.IMC < 18.5m || e.IMC >= 25m         // ← Agregar 'm' para decimal
                })
                .ToListAsync();

            return Ok(estudiantes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener dashboard: {ex.Message}");
        }
    }
}