using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Habitos;

namespace SmartNutriTracker.Back.Services.Habitos
{
    public class RegistroHabitoService : IRegistroHabitoService
    {
        private readonly ApplicationDbContext _context;

        public RegistroHabitoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarHabitosAsync(RegistroHabitoNuevoDTO registro)
        {
            try
            {
                if (registro.EstudianteId <= 0)
                    return false;
                
                if (registro.HorasSueno < 0 || registro.HorasSueno > 24)
                    return false;
                    
                if (registro.HorasActividadFisica < 0 || registro.HorasActividadFisica > 24)
                    return false;

                var fechaHora = registro.Fecha.ToDateTime(registro.Hora);
                var fechaUtc = DateTime.SpecifyKind(fechaHora, DateTimeKind.Utc);

                var nuevoRegistro = new RegistroHabito
                {
                    EstudianteId = registro.EstudianteId,
                    Fecha = fechaUtc,
                    HorasSueno = registro.HorasSueno,
                    HorasActividadFisica = registro.HorasActividadFisica
                };

                _context.RegistroHabitos.Add(nuevoRegistro);
                var resultado = await _context.SaveChangesAsync();
                
                Console.WriteLine($"Registro creado con ID: {nuevoRegistro.RegistroHabitoId}, Filas afectadas: {resultado}");
                
                return resultado > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en RegistrarHabitosAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<List<RegistroHabitoDTO>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            var registros = await _context.RegistroHabitos
                .Include(r => r.RegistroAlimentos!)
                    .ThenInclude(ra => ra.Alimento!)
                .Include(r => r.RegistroAlimentos!)
                    .ThenInclude(ra => ra.TipoComida!)
                .Where(r => r.EstudianteId == estudianteId)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            return registros.Select(r => new RegistroHabitoDTO
            {
                RegistroHabitoId = r.RegistroHabitoId,
                EstudianteId = r.EstudianteId,
                Fecha = r.Fecha,
                HorasSueno = r.HorasSueno,
                HorasActividadFisica = r.HorasActividadFisica,
                RegistroAlimentos = r.RegistroAlimentos?.Select(ra => new RegistroAlimentoDetalleDTO
                {
                    AlimentoId = ra.AlimentoId,
                    AlimentoNombre = ra.Alimento?.Nombre ?? "Desconocido",
                    Calorias = ra.Alimento?.Calorias ?? 0,
                    TipoComidaId = ra.TipoComidaId,
                    TipoComidaNombre = ra.TipoComida?.Nombre ?? "Desconocido",
                    Cantidad = ra.Cantidad ?? 0
                }).ToList()
            }).ToList();
        }

        public async Task<List<RegistroHabitoDTO>> ObtenerHabitosPorEstudianteAsync(int estudianteId)
        {
            var registros = await _context.RegistroHabitos
                .Where(r => r.EstudianteId == estudianteId)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            return registros.Select(r => new RegistroHabitoDTO
            {
                RegistroHabitoId = r.RegistroHabitoId,
                EstudianteId = r.EstudianteId,
                Fecha = r.Fecha,
                HorasSueno = r.HorasSueno,
                HorasActividadFisica = r.HorasActividadFisica
            }).ToList();
        }

        public async Task<List<RegistroHabitoDTO>> ObtenerConsumoPorEstudianteAsync(int estudianteId)
        {
            var registros = await _context.RegistroHabitos
                .Include(r => r.RegistroAlimentos!)
                    .ThenInclude(ra => ra.Alimento!)
                .Include(r => r.RegistroAlimentos!)
                    .ThenInclude(ra => ra.TipoComida!)
                .Where(r => r.EstudianteId == estudianteId && 
                           r.RegistroAlimentos != null && 
                           r.RegistroAlimentos.Any())
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            return registros.Select(r => new RegistroHabitoDTO
            {
                RegistroHabitoId = r.RegistroHabitoId,
                EstudianteId = r.EstudianteId,
                Fecha = r.Fecha,
                HorasSueno = r.HorasSueno,
                HorasActividadFisica = r.HorasActividadFisica,
                RegistroAlimentos = r.RegistroAlimentos?.Select(ra => new RegistroAlimentoDetalleDTO
                {
                    AlimentoId = ra.AlimentoId,
                    AlimentoNombre = ra.Alimento?.Nombre ?? "Desconocido",
                    Calorias = ra.Alimento?.Calorias ?? 0,
                    TipoComidaId = ra.TipoComidaId,
                    TipoComidaNombre = ra.TipoComida?.Nombre ?? "Desconocido",
                    Cantidad = ra.Cantidad ?? 0
                }).ToList() ?? new List<RegistroAlimentoDetalleDTO>()
            }).ToList();
        }

        public async Task<RegistroHabitoDTO?> ObtenerHabitoPorIdAsync(int registroHabitoId)
        {
            try
            {
                var registro = await _context.RegistroHabitos
                    .Include(r => r.RegistroAlimentos!)
                        .ThenInclude(ra => ra.Alimento!)
                    .Include(r => r.RegistroAlimentos!)
                        .ThenInclude(ra => ra.TipoComida!)
                    .FirstOrDefaultAsync(r => r.RegistroHabitoId == registroHabitoId);

                if (registro == null)
                {
                    Console.WriteLine($"⚠️ No se encontró el registro con ID {registroHabitoId}");
                    return null;
                }

                return new RegistroHabitoDTO
                {
                    RegistroHabitoId = registro.RegistroHabitoId,
                    EstudianteId = registro.EstudianteId,
                    Fecha = registro.Fecha,
                    HorasSueno = registro.HorasSueno,
                    HorasActividadFisica = registro.HorasActividadFisica,
                    RegistroAlimentos = registro.RegistroAlimentos?.Select(ra => new RegistroAlimentoDetalleDTO
                    {
                        AlimentoId = ra.AlimentoId,
                        AlimentoNombre = ra.Alimento?.Nombre ?? "Desconocido",
                        Calorias = ra.Alimento?.Calorias ?? 0,
                        TipoComidaId = ra.TipoComidaId,
                        TipoComidaNombre = ra.TipoComida?.Nombre ?? "Desconocido",
                        Cantidad = ra.Cantidad ?? 0
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ObtenerHabitoPorIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ActualizarRegistroAsync(int registroHabitoId, RegistroHabitoActualizarDTO dto)
        {
            try
            {
                var registro = await _context.RegistroHabitos
                    .FirstOrDefaultAsync(r => r.RegistroHabitoId == registroHabitoId);

                if (registro == null)
                {
                    Console.WriteLine($"❌ Registro con ID {registroHabitoId} no encontrado");
                    return false;
                }

                bool cambios = false;
                
                if (dto.HorasSueno.HasValue)
                {
                    if (dto.HorasSueno.Value < 0 || dto.HorasSueno.Value > 24)
                    {
                        Console.WriteLine($"❌ Horas de sueño inválidas: {dto.HorasSueno.Value}");
                        return false;
                    }
                    registro.HorasSueno = dto.HorasSueno.Value;
                    cambios = true;
                    Console.WriteLine($"😴 Horas de sueño actualizadas a: {registro.HorasSueno}");
                }

                if (dto.HorasActividadFisica.HasValue)
                {
                    if (dto.HorasActividadFisica.Value < 0 || dto.HorasActividadFisica.Value > 24)
                    {
                        Console.WriteLine($"❌ Horas de actividad inválidas: {dto.HorasActividadFisica.Value}");
                        return false;
                    }
                    registro.HorasActividadFisica = dto.HorasActividadFisica.Value;
                    cambios = true;
                    Console.WriteLine($"💪 Horas de actividad actualizadas a: {registro.HorasActividadFisica}");
                }

                if (!cambios)
                {
                    Console.WriteLine("ℹ️ No hay cambios para actualizar");
                    return true;
                }

                var resultado = await _context.SaveChangesAsync();
                Console.WriteLine($"✅ Hábito actualizado. Filas afectadas: {resultado}");
                
                return resultado > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR en ActualizarRegistroAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}