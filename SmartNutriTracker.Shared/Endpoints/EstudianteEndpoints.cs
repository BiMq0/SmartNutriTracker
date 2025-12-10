namespace SmartNutriTracker.Shared.Endpoints;

public static class EstudiantesEndpoints
{
    public const string REGISTRAR_ESTUDIANTE = "/api/estudiantes";
    public const string OBTENER_ESTUDIANTES = "/api/estudiantes";
    public const string OBTENER_ESTUDIANTE_POR_ID = "/api/estudiantes/{id}";
    public const string ACTUALIZAR_PERFIL_ESTUDIANTE = "/api/estudiantes/{id}/perfil"; // Nuevo endpoint
    public const string OBTENER_PERFIL_ESTUDIANTE = "/api/estudiantes/perfil/{id}";
    public const string OBTENER_DASHBOARD = "/api/estudiantes/dashboard";
}
