using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Cuota
    {
        public int IdCuota { get; set; } 
        public int? IdPago { get; set; }
        public Socio Socio { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Monto { get; set; } // monto SIN recargo
        public decimal Recargo { get; set; } // recargo en caso que corresponda
        public string Estado { get; set; }  // "Pagado", "ConRecargo" o "Deudor"
        public DateTime? FechaPago { get; set; } // NULL SI NO PAGADA
    }
}
