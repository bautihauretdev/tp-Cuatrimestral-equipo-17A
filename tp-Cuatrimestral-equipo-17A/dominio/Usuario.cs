using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // El HASH, no el password
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }
        public Socio Socio { get; set; }
    }
}
