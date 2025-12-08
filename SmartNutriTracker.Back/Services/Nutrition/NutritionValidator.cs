using System;
using System.Collections.Generic;
using SmartNutriTracker.Shared.DTOs.Nutrition;

namespace SmartNutriTracker.Back.Services.Nutrition
{
    public interface INutritionValidator
    {
        ValidationResultDTO ValidateNutritionRequest(NutritionRequestDTO request);
    }

    public class NutritionValidator : INutritionValidator
    {
        // Constantes de validación
        private const decimal MIN_PESO = 30m;
        private const decimal MAX_PESO = 300m;
        private const decimal MIN_ALTURA = 1.2m;
        private const decimal MAX_ALTURA = 2.5m;
        private const int MIN_EDAD = 13;
        private const int MAX_EDAD = 100;

        public ValidationResultDTO ValidateNutritionRequest(NutritionRequestDTO request)
        {
            var errores = new List<string>();

            // Validar peso
            if (!request.PesoKg.HasValue)
                errores.Add("El peso es requerido.");
            else if (request.PesoKg < MIN_PESO || request.PesoKg > MAX_PESO)
                errores.Add($"El peso debe estar entre {MIN_PESO} kg y {MAX_PESO} kg. Valor recibido: {request.PesoKg} kg");

            // Validar altura
            if (!request.AlturaM.HasValue)
                errores.Add("La altura es requerida.");
            else if (request.AlturaM < MIN_ALTURA || request.AlturaM > MAX_ALTURA)
                errores.Add($"La altura debe estar entre {MIN_ALTURA} m y {MAX_ALTURA} m. Valor recibido: {request.AlturaM} m");

            // Validar fecha de nacimiento
            if (!request.FechaNacimiento.HasValue)
                errores.Add("La fecha de nacimiento es requerida.");
            else
            {
                var edad = CalculateAge(request.FechaNacimiento.Value);
                if (edad < MIN_EDAD || edad > MAX_EDAD)
                    errores.Add($"La edad debe estar entre {MIN_EDAD} y {MAX_EDAD} años. Edad calculada: {edad} años");
                
                if (request.FechaNacimiento.Value > DateTime.Today)
                    errores.Add("La fecha de nacimiento no puede ser en el futuro.");
            }

            // Validar sexo
            if (string.IsNullOrWhiteSpace(request.Sexo))
                errores.Add("El sexo es requerido.");
            else if (!IsValidSexo(request.Sexo))
                errores.Add("El sexo debe ser 'Varon' o 'Mujer'.");

            // Validar nivel de actividad
            if (string.IsNullOrWhiteSpace(request.NivelActividad))
                errores.Add("El nivel de actividad es requerido.");
            else if (!IsValidNivelActividad(request.NivelActividad))
                errores.Add("El nivel de actividad debe ser uno de: Sedentario, Ligero, Moderado, Alto, MuyAlto");

            // Validar objetivo
            if (string.IsNullOrWhiteSpace(request.Objetivo))
                errores.Add("El objetivo es requerido.");
            else if (!IsValidObjetivo(request.Objetivo))
                errores.Add("El objetivo debe ser uno de: Perder, Mantener, Ganar");

            return errores.Count == 0 
                ? ValidationResultDTO.Success() 
                : ValidationResultDTO.Failure(errores.ToArray());
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }

        private bool IsValidSexo(string sexo)
        {
            var validValues = new[] { "Varon", "Mujer", "varon", "mujer" };
            return Array.Exists(validValues, element => element.Equals(sexo, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidNivelActividad(string nivel)
        {
            var validValues = new[] { "Sedentario", "Ligero", "Moderado", "Alto", "MuyAlto" };
            return Array.Exists(validValues, element => element.Equals(nivel, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidObjetivo(string objetivo)
        {
            var validValues = new[] { "Perder", "Mantener", "Ganar" };
            return Array.Exists(validValues, element => element.Equals(objetivo, StringComparison.OrdinalIgnoreCase));
        }
    }
}
