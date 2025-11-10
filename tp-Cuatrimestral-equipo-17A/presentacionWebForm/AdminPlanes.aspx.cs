using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class AdminPlanes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptarAltaPlan_Click(object sender, EventArgs e)
        {
            // (!) PENDIENTE AGREGAR VALIDACIONES
            // QUE NO EXISTA YA UN PLAN CON ESE NOMBRE
            // QUE NO EXISTA YA UN PLAN CON ESA CANTIDAD DE HORAS

            // QUE NO SE PUEDAN INGRESAR CAMPOS VACIOS
            try
            {
                Plan nuevo = new Plan
                {
                    Nombre = txtNombrePlanAgregar.Text,
                    PrecioMensual = decimal.Parse(txtMontoPlanAgregar.Text),
                    MaxHorasSemana = int.Parse(txthorasPlanAgregar.Text),
                    Activo = true
                };
                PlanNegocio planNegocio = new PlanNegocio();
                int idPlan = planNegocio.Agregar(nuevo);
            }
            catch (FormatException)
            {
                // Maneja el caso de formato inválido, por ejemplo si el usuario escribe letras
                lblErrorPlanAgregar.Text = "Verificá que el monto y las horas sean números válidos.";
                lblErrorPlanAgregar.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorPlanAgregar.Text = "Ocurrió un error: " + ex.Message;
                lblErrorPlanAgregar.Visible = true;
            }
        }
    }
}