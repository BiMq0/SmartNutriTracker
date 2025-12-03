using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Shared.DTOs.Usuarios
{
    public class UsuarioRegistroDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int RolId { get; set; }

        public UsuarioRegistroDTO() { }
        public UsuarioRegistroDTO(Usuario usuario)
        {
            Id = usuario.UsuarioId;
            Nombre = usuario.Username;
            RolId = usuario.RolId;
        }
    }
}