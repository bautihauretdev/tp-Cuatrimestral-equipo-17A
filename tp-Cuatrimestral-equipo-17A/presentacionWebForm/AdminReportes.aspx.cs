using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class AdminReportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var topActivos = new List<dynamic>
                {
                    new { Nombre = "Juan Pérez", Asistencias = 1 },
                    new { Nombre = "Sofía Castro", Asistencias = 0 },
                    new { Nombre = "Diego Luna", Asistencias = 0 },
                    new { Nombre = "Valeria Gil", Asistencias = 0 },
                    new { Nombre = "Esteban Quito", Asistencias = 0 }
                };

                rptActivos.DataSource = topActivos;
                rptActivos.DataBind();
            }

        }
    }
}