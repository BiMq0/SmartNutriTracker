using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SmartNutriTracker.Shared.DTOs.Alimentos
{
    public class CreateAlimentoDTO
    {
        [Required(ErrorMessage = "El nombre del alimento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        [NoNumericCharacters(ErrorMessage = "El nombre del alimento no puede contener números.")]
        public string Nombre { get; set; } = null!;

        [Range(0, 10000, ErrorMessage = "Las calorías deben ser entre 0 y 10,000.")]
        //alimentos con cero calorias como agua 0 kal, cafe negro sin azucar entre 1 y 5 kal, infusioones sin azucar calorias casi cero         
        public int Calorias { get; set; }

        [Range(0, 1000, ErrorMessage = "Las proteínas deben ser entre 0 y 1000.")]
        //aliemntos sin proteinas aceites de de cocina, azucar de mesa y miel por cada 100gr tiene 0.1 
        public decimal Proteinas { get; set; }

        [Range(0, 1000, ErrorMessage = "Los carbohidratos deben ser entre 0 y 1000.")]
        // verduras acuosas como 5 gramos de apio
        public decimal Carbohidratos { get; set; }

        [Range(0, 1000, ErrorMessage = "Las grasas deben ser entre 0 y 1000.")]
        // Alimentos sin grasas: frutas, verduras, cereales integrales
        public decimal Grasas { get; set; }
    }

    //validacion personalizada para no permitir numeros en el nombre
    public class NoNumericCharactersAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string nombre)
            {
                // Retorna false si encuentra algún dígito (0-9)
                return !Regex.IsMatch(nombre, @"\d");
            }
            return true;
        }
    }
}