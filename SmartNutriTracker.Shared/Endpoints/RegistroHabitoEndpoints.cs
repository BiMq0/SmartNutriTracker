// Shared/Endpoints/RegistroHabitoEndpoints.cs
namespace SmartNutriTracker.Shared.Endpoints
{
    public static class RegistroHabitoEndpoints
    {
        public const string BASE = "api/RegistroHabito/";
        
        public const string REGISTRAR_HABITOS = "RegistrarHabitos";
        public const string OBTENER_HABITOS_POR_ESTUDIANTE = "ObtenerHabitosPorEstudiante/{estudianteId}";
        public const string OBTENER_TODO_POR_ESTUDIANTE = "ObtenerTodoPorEstudiante/{estudianteId}";
        public const string OBTENER_POR_ID = "{registroHabitoId}"; // ✅ AGREGAR ESTA LÍNEA
        public const string ACTUALIZAR = "Actualizar/{registroHabitoId}";
    }
}