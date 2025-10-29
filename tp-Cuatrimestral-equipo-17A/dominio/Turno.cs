using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public int CapacidadMaxima { get; set; }
        public int Ocupados { get; set; }   // Cantidad socios que asisten, acumulable
    }
}
