using Microsoft.AspNetCore.SignalR;

namespace SmartNutriTracker.Back.Hubs
{
    public class NotificacionHub : Hub
    {
        public async Task UnirseAGrupo(string rol)
        {
            if (rol == "3") 
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Estudiantes");
            }
            else if (rol == "2") 
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Docentes");
            }
        }

        public async Task EnviarNotificacion()
        {
            await Clients.All.SendAsync("RecibirNotificacion");
        }
    }
}
