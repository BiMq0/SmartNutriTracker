using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class UpdateAlimentoDTO
    {
        [Required]
        public int AlimentoId { get; set; }

        [Required(ErrorMessage = "El nombre del alimento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Las calorías deben estar entre 0 y 10,000.")]
        public int Calorias { get; set; }

        [Range(0, 1000, ErrorMessage = "Las proteínas deben estar entre 0 y 1,000.")]
        public decimal Proteinas { get; set; }

        [Range(0, 1000, ErrorMessage = "Los carbohidratos deben estar entre 0 y 1,000.")]
        public decimal Carbohidratos { get; set; }

        [Range(0, 1000, ErrorMessage = "Las grasas deben estar entre 0 y 1,000.")]
        public decimal Grasas { get; set; }
    }
}
