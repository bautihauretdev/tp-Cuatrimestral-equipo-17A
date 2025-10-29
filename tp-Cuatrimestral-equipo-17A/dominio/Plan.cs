using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Plan
    {
        public int IdPlan { get; set; }
        public NombrePlan Nombre { get; set; }   // "Básico", "Medio" o "Premium"
        public decimal PrecioMensual { get; set; }
        public int MaxHorasSemana { get; set; }  //  horas limitadas por semana

    }
}
