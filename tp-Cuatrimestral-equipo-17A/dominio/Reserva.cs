using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public Turno Turno { get; set; }
        public Socio Socio { get; set; }
        public string Estado { get; set; }  // "Activa"|"Cancelada"
    }
}
