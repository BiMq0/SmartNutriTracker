using System;
using System.ComponentModel.DataAnnotations;

namespace SmartNutriTracker.Back.Database
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Accion { get; set; } = null!;
        public string Nivel { get; set; } = null!;
        public string Detalle { get; set; } = null!;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}