using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared.DTOs.NotificacionesLimite;

namespace SmartNutriTracker.Back.Services.NotificacionesLimite
{
    public class NotificacionesLimiteService : INotificacionesLimiteService
    {
        private readonly ApplicationDbContext _context;

        public NotificacionesLimiteService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ejecuta el pipeline completo de análisis de riesgos nutricionales.
        /// 1. Establece el contexto temporal (día actual).
        /// 2. Recupera el grafo de objetos de estudiantes y sus registros mediante una consulta optimizada.
        /// 3. Itera sobre cada estudiante para realizar una evaluación metabólica individualizada.
        /// 4. Compara la ingesta calórica real contra la Tasa Metabólica Basal (TMB).
        /// 5. Genera DTOs de alerta para aquellos casos que transgreden los límites saludables.
        /// </summary>
        /// <returns>Una colección de alertas nutricionales listas para ser consumidas por la capa de presentación.</returns>
        public async Task<List<NotificacionesAlertaNutricionalDTO>> ObtenerAlertasCaloricasAsync()
        {
            var fechaActual = DateTime.Today;
            var estudiantes = await ObtenerEstudiantesConRegistros(fechaActual);
            var alertas = new List<NotificacionesAlertaNutricionalDTO>();

            foreach (var estudiante in estudiantes)
            {
                var limiteCalorico = estudiante.TMB;
                var habitoDelDia = estudiante.RegistroHabitos?.FirstOrDefault(h => h.Fecha.Date == fechaActual);

                if (habitoDelDia != null && habitoDelDia.RegistroAlimentos != null)
                {
                    var caloriasTotales = CalcularCaloriasConsumidas(habitoDelDia);

                    if (caloriasTotales > limiteCalorico)
                    {
                        alertas.Add(GenerarDTOAlerta(estudiante, limiteCalorico, caloriasTotales));
                    }
                }
            }

            return alertas;
        }

        /// <summary>
        /// Realiza una consulta de alto rendimiento a la base de datos utilizando Entity Framework Core.
        /// Emplea la técnica de 'Eager Loading' (Carga Ansiosa) mediante `.Include` y `.ThenInclude` para hidratar
        /// en una sola ida y vuelta (round-trip) toda la jerarquía de objetos necesaria:
        /// Estudiante -> RegistroHabitos -> RegistroAlimentos -> Alimento.
        /// Esto previene el problema de N+1 consultas y asegura la eficiencia del proceso de análisis.
        /// </summary>
        /// <param name="fecha">La fecha de corte para filtrar los registros de hábitos relevantes.</param>
        /// <returns>Una lista de estudiantes con sus propiedades de navegación pobladas.</returns>
        private async Task<List<Estudiante>> ObtenerEstudiantesConRegistros(DateTime fecha)
        {
            return await _context.Estudiantes
                .Include(e => e.RegistroHabitos)
                .ThenInclude(rh => rh.RegistroAlimentos)
                .ThenInclude(ra => ra.Alimento)
                .ToListAsync();
        }

        /// <summary>
        /// Algoritmo de agregación calórica.
        /// Recorre la colección de registros de alimentos de un hábito específico.
        /// Para cada entrada, multiplica las calorías base del alimento por la cantidad consumida (normalizando nulos a 1).
        /// El resultado es una suma escalar precisa de la energía total ingerida en ese registro de hábito.
        /// </summary>
        /// <param name="habito">La entidad compleja que contiene los detalles del consumo diario.</param>
        /// <returns>El total acumulado de kilocalorías (decimal).</returns>
        private decimal CalcularCaloriasConsumidas(RegistroHabito habito)
        {
            decimal total = 0;
            foreach (var registro in habito.RegistroAlimentos!)
            {
                if (registro.Alimento != null)
                {
                    var cantidad = registro.Cantidad ?? 1;
                    total += registro.Alimento.Calorias * cantidad;
                }
            }
            return total;
        }

        /// <summary>
        /// Factoría de objetos de transferencia de datos (DTO).
        /// Transforma los resultados del análisis lógico y los datos crudos de la entidad en un objeto plano y serializable.
        /// Construye un mensaje de alerta semántico y legible por humanos que describe la naturaleza y magnitud del exceso.
        /// </summary>
        /// <param name="estudiante">La entidad fuente del estudiante.</param>
        /// <param name="limite">El valor límite calculado (TMB).</param>
        /// <param name="consumo">El valor real consumido.</param>
        /// <returns>Una instancia de AlertaNutricionalDTO completamente poblada.</returns>
        private NotificacionesAlertaNutricionalDTO GenerarDTOAlerta(Estudiante estudiante, decimal limite, decimal consumo)
        {
            return new NotificacionesAlertaNutricionalDTO
            {
                EstudianteId = estudiante.EstudianteId,
                NombreEstudiante = estudiante.NombreCompleto,
                LimiteCalorico = limite,
                CaloriasConsumidas = consumo,
                Exceso = consumo - limite,
                Mensaje = $"ALERTA CRÍTICA: El estudiante {estudiante.NombreCompleto} ha transgredido su umbral metabólico diario ({limite} kcal). Ingesta actual: {consumo} kcal."
            };
        }
    }
}
