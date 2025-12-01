using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNutriTracker.Shared.DTOs.Usuarios
{
    public class UsuarioNuevoDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }
    }
}