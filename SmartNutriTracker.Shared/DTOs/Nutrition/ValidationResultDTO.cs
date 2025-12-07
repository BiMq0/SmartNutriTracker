using System.Collections.Generic;

namespace SmartNutriTracker.Shared.DTOs.Nutrition
{
    public class ValidationResultDTO
    {
        public bool IsValid { get; set; }
        public List<string> Errores { get; set; } = new List<string>();

        public ValidationResultDTO() { }

        public ValidationResultDTO(bool isValid, List<string> errores = null)
        {
            IsValid = isValid;
            Errores = errores ?? new List<string>();
        }

        public static ValidationResultDTO Success() => new ValidationResultDTO(true);
        public static ValidationResultDTO Failure(params string[] errores) => 
            new ValidationResultDTO(false, new List<string>(errores));
    }
}
