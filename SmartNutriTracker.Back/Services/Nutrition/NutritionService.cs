using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Domain.Statics;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public class NutritionService : INutritionService
    {
        private readonly ApplicationDbContext _db;

        public NutritionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<NutritionResultDTO> CalculateAsync(NutritionRequestDTO request)
        {
            // Preparar datos: si viene EstudianteId, obtener registro
            Estudiante? estudiante = null;
            if (request.EstudianteId.HasValue)
            {
                estudiante = _db.Estudiantes.FirstOrDefault(e => e.EstudianteId == request.EstudianteId.Value);
            }

            decimal peso = 0m;
            decimal alturaM = 0m;
            int edad = 0;
            Sexo sexo = Sexo.Varon;

            if (estudiante != null)
            {
                peso = estudiante.Peso;
                alturaM = estudiante.Altura;
                edad = estudiante.Edad;
                sexo = estudiante.Sexo;
            }
            else
            {
                if (!request.PesoKg.HasValue || !request.AlturaM.HasValue || !request.FechaNacimiento.HasValue || string.IsNullOrWhiteSpace(request.Sexo))
                {
                    throw new ArgumentException("Si no se envía EstudianteId, se requieren PesoKg, AlturaM, FechaNacimiento y Sexo en el payload.");
                }
                peso = request.PesoKg.Value;
                alturaM = request.AlturaM.Value;
                var dob = request.FechaNacimiento.Value;
                edad = DateTime.Now.Year - dob.Year - (DateTime.Now.DayOfYear < dob.DayOfYear ? 1 : 0);
                sexo = ParseSexo(request.Sexo);
            }

            // TMB: usar la propiedad calculada si estudiante existe, sino calcular
            decimal tmb = 0m;
            if (estudiante != null)
            {
                tmb = estudiante.TMB;
            }
            else
            {
                tmb = CalculateTMB(peso, alturaM, edad, sexo);
            }

            // Factor de actividad
            decimal factorActividad = MapActividadToFactor(request.NivelActividad);
            decimal mantenimiento = Math.Round(tmb * factorActividad, 2);

            // Objetivo
            var objetivo = (request.Objetivo ?? "mantener").ToLowerInvariant();
            decimal objetivoKcal = mantenimiento;
            if (objetivo.Contains("perd") || objetivo.Contains("perder"))
            {
                objetivoKcal = Math.Max(0, mantenimiento - 500);
            }
            else if (objetivo.Contains("ganar") || objetivo.Contains("aumentar"))
            {
                objetivoKcal = mantenimiento + 300;
            }

            // Macros por defecto
            decimal proteinasGr = Math.Round(1.6m * peso, 2); // g
            decimal proteinasKcal = proteinasGr * 4m;

            decimal grasasKcal = Math.Round(objetivoKcal * 0.25m, 2);
            decimal grasasGr = Math.Round(grasasKcal / 9m, 2);

            decimal restoKcal = objetivoKcal - (proteinasKcal + grasasKcal);
            decimal carbGr = 0m;
            if (restoKcal > 0)
            {
                carbGr = Math.Round(restoKcal / 4m, 2);
            }

            // Calcular IMC
            decimal imc = 0m;
            string categoriaIMC = "No calculado";
            if (alturaM > 0)
            {
                imc = Math.Round(peso / (alturaM * alturaM), 2);
                categoriaIMC = ClasificarIMC(imc);
            }

            var result = new NutritionResultDTO
            {
                IMC = imc,
                CategoriaIMC = categoriaIMC,
                TMB = Math.Round(tmb, 2),
                CaloriasMantenimiento = Math.Round(mantenimiento, 2),
                CaloriasObjetivo = Math.Round(objetivoKcal, 2),
                ProteinasGr = proteinasGr,
                GrasasGr = grasasGr,
                CarbohidratosGr = carbGr
            };

            return await Task.FromResult(result);
        }

        private decimal CalculateTMB(decimal pesoKg, decimal alturaM, int edad, Sexo sexo)
        {
            // Mifflin-St Jeor: 10*peso + 6.25*altura(cm) - 5*edad + s
            decimal alturaCm = alturaM * 100m;
            decimal s = sexo == Sexo.Varon ? 5m : -161m;
            return 10m * pesoKg + 6.25m * alturaCm - 5m * edad + s;
        }

        private Sexo ParseSexo(string sexo)
        {
            if (string.IsNullOrWhiteSpace(sexo)) return Sexo.Varon;
            var s = sexo.Trim().ToLowerInvariant();
            if (s.StartsWith("v") || s.StartsWith("m") && s.Contains("ar")) return Sexo.Varon; // v/varon
            if (s.StartsWith("mu") || s.StartsWith("f") || s.Contains("muj")) return Sexo.Mujer;
            if (s.StartsWith("m") && s.Length == 1) return Sexo.Mujer;
            return Sexo.Varon;
        }

        private decimal MapActividadToFactor(string? nivel)
        {
            if (string.IsNullOrWhiteSpace(nivel)) return 1.2m; // sedentario por defecto
            var n = nivel.Trim().ToLowerInvariant();
            if (n.Contains("sedent")) return 1.2m;
            if (n.Contains("liger") || n.Contains("light")) return 1.375m;
            if (n.Contains("moder")) return 1.55m;
            if (n.Contains("alto")) return 1.725m;
            if (n.Contains("muy") || n.Contains("very")) return 1.9m;
            // si se recibe un número (ej. "1.55") intentar parsear
            if (decimal.TryParse(n.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var f))
            {
                return f;
            }
            return 1.2m;
        }

        private string ClasificarIMC(decimal imc)
        {
            if (imc < 18.5m) return "Bajo peso";
            if (imc < 25m) return "Peso normal";
            if (imc < 30m) return "Sobrepeso";
            if (imc < 35m) return "Obesidad grado I";
            if (imc < 40m) return "Obesidad grado II";
            return "Obesidad grado III";
        }
    }
}
