using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class AdminTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Todos los lunes controla que la semana actual y las tres siguientes estén creados los turnos
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                TurnoNegocio negocio = new TurnoNegocio();
                negocio.AsegurarSemanas();
            }

        }
    }
}