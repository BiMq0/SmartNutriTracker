using SmartNutriTracker.Domain.Models.BaseModels;
 
namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class AlimentosDTO
    {
        public int AlimentoId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carbohidratos { get; set; }
        public decimal Grasas { get; set; }
 
        public AlimentosDTO() { }
 
        public AlimentosDTO(Alimento entity)
        {
            AlimentoId = entity.AlimentoId;
            Nombre = entity.Nombre;
            Calorias = entity.Calorias;
            Proteinas = entity.Proteinas;
            Carbohidratos = entity.Carbohidratos;
            Grasas = entity.Grasas;
        }
    }
}