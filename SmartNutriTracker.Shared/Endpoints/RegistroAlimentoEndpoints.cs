namespace SmartNutriTracker.Shared.Endpoints
{
    public static class RegistroAlimentoEndpoints
    {
        public const string BASE = "api/RegistroAlimento/";
        
        // Registrar consumo
        public const string REGISTRAR_CONSUMO = "RegistrarConsumo";
        
        // Obtener registros
        public const string OBTENER_POR_ESTUDIANTE = "ObtenerPorEstudiante/{estudianteId}";
        public const string OBTENER_POR_ID = "ObtenerPorId/{registroHabitoId}";
        
        // Actualizar registros
        public const string ACTUALIZAR_CONSUMO = "ActualizarConsumo/{registroHabitoId}";
    }
}