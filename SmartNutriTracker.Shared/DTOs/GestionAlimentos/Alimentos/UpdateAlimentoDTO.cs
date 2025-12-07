using SmartNutriTracker.Domain.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class UpdateAlimentoDTO
    {
        public UpdateAlimentoDTO() { }

        // Constructor adicional para cargar datos desde la entidad
        public UpdateAlimentoDTO(Alimento entity)
        {
            AlimentoId = entity.AlimentoId;
            Nombre = entity.Nombre;
            Calorias = entity.Calorias;
            Proteinas = entity.Proteinas;
            Carbohidratos = entity.Carbohidratos;
            Grasas = entity.Grasas;
        }

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
