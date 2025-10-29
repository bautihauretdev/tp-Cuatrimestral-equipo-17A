using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Asistencia
    {
        public int IdAsistencia { get; set; }
        public Reserva Reserva { get; set; }
        public bool Asistio { get; set; } // TRUE SI ASISTIO, FALSE SI NO ASISTIO
        public DateTime FechaRegistro { get; set; }

    }
}
