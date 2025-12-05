using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNutriTracker.Shared.Endpoints
{
    public static class UsuariosEndpoints
    {
        public const string BASE = "api/user/";
        public const string OBTENER_TODOS_USUARIOS = "ObtenerUsuarios";
        public const string INICIAR_SESION = "IniciarSesion";
        public const string REGISTRAR_USUARIO = "RegistrarUsuario";
        public const string CERRAR_SESION = "CerrarSesion";
    }
}