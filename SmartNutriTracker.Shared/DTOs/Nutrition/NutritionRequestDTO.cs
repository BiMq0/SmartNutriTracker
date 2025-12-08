using System;

namespace SmartNutriTracker.Shared.DTOs.Nutrition
{
    public class NutritionRequestDTO
    {
        public int? EstudianteId { get; set; }

        // Si no se provee EstudianteId, se pueden enviar los siguientes campos
        public decimal? PesoKg { get; set; }
        // Altura en metros (ej. 1.70)
        public decimal? AlturaM { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        // "Varon" o "Mujer" (o variantes)
        public string? Sexo { get; set; }

        // Nivel de actividad: Sedentario, Ligero, Moderado, Alto, MuyAlto
        public string? NivelActividad { get; set; }

        // Objetivo: Perder, Mantener, Ganar
        public string? Objetivo { get; set; }
    }
}
