using System.Threading.Tasks;

namespace SmartNutriTracker.Back.Services.Audit
{
    public interface IAuditService
    {
        Task LogAsync(string accion, string nivel, string detalle = "");
    }
}
