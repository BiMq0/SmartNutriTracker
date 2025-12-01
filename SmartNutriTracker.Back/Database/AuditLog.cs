
namespace SmartNutriTracker.Back.Database
{
    public class AuditLog
    {
        public string NombreUsuario { get; internal set; }
        public string Accion { get; internal set; }
        public string Nivel { get; internal set; }
        public string Detalle { get; internal set; }
        public DateTime Fecha { get; internal set; }
    }
}