namespace SmartNutriTracker.Back.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = "";
        public string Accion { get; set; } = "";
        public string Nivel { get; set; } = "";
        public string Detalle { get; set; } = "";
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
