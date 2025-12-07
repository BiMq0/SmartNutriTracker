// .back/Services/Alimentos/RegistroAlimentoService.cs
using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Services.Alimentos
{
    public class RegistroAlimentoService : IRegistroAlimentoService
    {
        private readonly ApplicationDbContext _context;

        public RegistroAlimentoService(ApplicationDbContext context)
        {
            _context = context;
        }

      public async Task<bool> RegistrarConsumoAsync(RegistroConsumoNuevoDTO registro)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        // VALIDAR DATOS DE ENTRADA
        if (registro.EstudianteId <= 0)
            throw new ArgumentException("ID de estudiante inválido");
        
        if (registro.AlimentosConsumidos == null || !registro.AlimentosConsumidos.Any())
            throw new ArgumentException("Debe proporcionar al menos un alimento");
        
        // Verificar que el estudiante existe
        var estudianteExiste = await _context.Estudiantes
            .AnyAsync(e => e.EstudianteId == registro.EstudianteId);
        
        if (!estudianteExiste)
            throw new ArgumentException($"El estudiante con ID {registro.EstudianteId} no existe");

        var fechaHora = registro.Fecha.ToDateTime(registro.Hora);
        var fechaUtc = DateTime.SpecifyKind(fechaHora, DateTimeKind.Utc);

        // Crear el registro principal
        var nuevoRegistro = new RegistroHabito
        {
            EstudianteId = registro.EstudianteId,
            Fecha = fechaUtc,
            HorasSueno = 0,
            HorasActividadFisica = 0
        };

        _context.RegistroHabitos.Add(nuevoRegistro);
        await _context.SaveChangesAsync(); // Guardar para obtener el ID

        // Ahora crear los alimentos
        foreach (var alimentoConsumido in registro.AlimentosConsumidos)
        {
            // Validar que el alimento existe
            var alimentoExiste = await _context.Alimentos
                .AnyAsync(a => a.AlimentoId == alimentoConsumido.AlimentoId);
            
            if (!alimentoExiste)
                throw new ArgumentException($"El alimento con ID {alimentoConsumido.AlimentoId} no existe");

            // Validar que el tipo de comida existe
            var tipoComidaExiste = await _context.TiposComida
                .AnyAsync(t => t.TipoComidaId == alimentoConsumido.TipoComidaId);
            
            if (!tipoComidaExiste)
                throw new ArgumentException($"El tipo de comida con ID {alimentoConsumido.TipoComidaId} no existe");

            var registroAlimento = new RegistroAlimento
            {
                RegistroHabitoId = nuevoRegistro.RegistroHabitoId,
                AlimentoId = alimentoConsumido.AlimentoId,
                TipoComidaId = alimentoConsumido.TipoComidaId,
                Cantidad = alimentoConsumido.Cantidad
            };

            _context.RegistroAlimentos.Add(registroAlimento);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        
        Console.WriteLine($"✅ Consumo registrado exitosamente. RegistroHabitoId: {nuevoRegistro.RegistroHabitoId}");
        return true;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        Console.WriteLine($"❌ ERROR en RegistrarConsumoAsync: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        return false;
    }
}
        public async Task<List<RegistroConsumoDTO>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            var registros = await _context.RegistroHabitos
                .Include(r => r.RegistroAlimentos)
                    .ThenInclude(ra => ra.Alimento)
                .Include(r => r.RegistroAlimentos)
                    .ThenInclude(ra => ra.TipoComida)
                .Where(r => r.EstudianteId == estudianteId && 
                           r.RegistroAlimentos != null && 
                           r.RegistroAlimentos.Any())
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            return registros.Select(r => new RegistroConsumoDTO
            {
                RegistroHabitoId = r.RegistroHabitoId,
                EstudianteId = r.EstudianteId,
                Fecha = r.Fecha,
                AlimentosConsumidos = r.RegistroAlimentos.Select(ra => new DetalleAlimentoDTO
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

        public async Task<RegistroConsumoDTO?> ObtenerPorIdAsync(int registroHabitoId)
        {
            var registro = await _context.RegistroHabitos
                .Include(r => r.RegistroAlimentos)
                    .ThenInclude(ra => ra.Alimento)
                .Include(r => r.RegistroAlimentos)
                    .ThenInclude(ra => ra.TipoComida)
                .Where(r => r.RegistroHabitoId == registroHabitoId)
                .FirstOrDefaultAsync();

            if (registro == null)
                return null;

            return new RegistroConsumoDTO
            {
                RegistroHabitoId = registro.RegistroHabitoId,
                EstudianteId = registro.EstudianteId,
                Fecha = registro.Fecha,
                AlimentosConsumidos = registro.RegistroAlimentos?.Select(ra => new DetalleAlimentoDTO
                {
                    AlimentoId = ra.AlimentoId,
                    AlimentoNombre = ra.Alimento?.Nombre ?? "Desconocido",
                    Calorias = ra.Alimento?.Calorias ?? 0,
                    TipoComidaId = ra.TipoComidaId,
                    TipoComidaNombre = ra.TipoComida?.Nombre ?? "Desconocido",
                    Cantidad = ra.Cantidad ?? 0
                }).ToList() ?? new List<DetalleAlimentoDTO>()
            };
        }

        public async Task<bool> ActualizarConsumoAsync(int registroHabitoId, RegistroConsumoActualizarDTO dto)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        var registro = await _context.RegistroHabitos
            .Include(r => r.RegistroAlimentos)
            .FirstOrDefaultAsync(r => r.RegistroHabitoId == registroHabitoId);

        if (registro == null)
        {
            Console.WriteLine($"❌ Registro con ID {registroHabitoId} no encontrado");
            return false;
        }

        // Actualizar fecha si se proporciona
        if (dto.Fecha.HasValue && dto.Hora.HasValue)
        {
            var fechaHora = dto.Fecha.Value.ToDateTime(dto.Hora.Value);
            registro.Fecha = DateTime.SpecifyKind(fechaHora, DateTimeKind.Utc);
            Console.WriteLine($"📅 Fecha actualizada a: {registro.Fecha}");
        }

        // Eliminar alimentos existentes
        if (registro.RegistroAlimentos != null && registro.RegistroAlimentos.Any())
        {
            _context.RegistroAlimentos.RemoveRange(registro.RegistroAlimentos);
            Console.WriteLine($"🗑️ Eliminados {registro.RegistroAlimentos.Count} alimentos existentes");
        }

        // Agregar nuevos alimentos
        if (dto.AlimentosConsumidos != null && dto.AlimentosConsumidos.Any())
        {
            foreach (var alimentoDto in dto.AlimentosConsumidos)
            {
                var registroAlimento = new RegistroAlimento
                {
                    RegistroHabitoId = registroHabitoId,
                    AlimentoId = alimentoDto.AlimentoId,
                    TipoComidaId = alimentoDto.TipoComidaId,
                    Cantidad = alimentoDto.Cantidad
                };
                _context.RegistroAlimentos.Add(registroAlimento);
            }
            Console.WriteLine($"➕ Agregados {dto.AlimentosConsumidos.Count} nuevos alimentos");
        }

        var resultado = await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        
        Console.WriteLine($"✅ Consumo actualizado. Filas afectadas: {resultado}");
        return true;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        Console.WriteLine($"❌ ERROR en ActualizarConsumoAsync: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        return false;
    }
}
    }
}