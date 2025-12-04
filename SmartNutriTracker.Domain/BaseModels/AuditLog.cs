namespace SmartNutriTracker.Back.Models
{
    // AuditLog.cs
    using System;

    namespace SmartNutriTracker.Back.Database
    {
        public class AuditLog
        {
            public int AuditLogId { get; set; }
            public string NombreUsuario { get; set; } = string.Empty; 
            public string Accion { get; set; } = string.Empty;        
            public string Nivel { get; set; } = "INFO";               // Nivel: INFO, WARNING, ERROR
            public string Detalle { get; set; } = string.Empty;     
            public DateTime Fecha { get; set; }                       
        }
    }

}
