using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Menus
{
    public class CreateMenuDTO
    {
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DateNotInPast(ErrorMessage = "La fecha no puede ser anterior a hoy.")]
        public DateTime Fecha { get; set; }

        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un alimento.")]
        public List<int> AlimentoIds { get; set; } = new();
    }

    //validacion de fecha personalizadaa
    public class DateNotInPastAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                //solo e hoy para adelante
                return date.Date >= DateTime.Today;
            }
            return false;
        }
    }
}
