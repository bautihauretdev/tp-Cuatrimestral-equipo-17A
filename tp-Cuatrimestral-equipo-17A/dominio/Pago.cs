using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Pago
    {
        public int IdPago { get; set; }    
        public int IdSocio { get; set; }
        public int IdCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }   
        public string FormaPago { get; set; }         

        public string Observaciones { get; set; }


    }
}
