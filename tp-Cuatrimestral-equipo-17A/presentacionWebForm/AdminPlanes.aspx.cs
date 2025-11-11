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
            if (!IsPostBack)
            {
                CargarPlanes();
            }
        }

        private void CargarPlanes()
        {
            PlanNegocio negocio = new PlanNegocio();
            List<Plan> planes = negocio.ListarPlanes();

            ddlPlan.DataSource = planes;
            ddlPlan.DataTextField = "Nombre";
            ddlPlan.DataValueField = "IdPlan";
            ddlPlan.DataBind();

            ddlPlan.Items.Insert(0, new ListItem("Seleccionar plan", "")); // opción vacía
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

                CargarPlanes(); // Recargar el listado de planes

                ddlPlan.SelectedIndex = 0; // Dejar el dropdown en "Seleccionar plan"
                txtHorasSemana.Text = "";
                txtMonto.Text = "";

                // Limpiar los campos del modal
                txtNombrePlanAgregar.Text = "";
                txtMontoPlanAgregar.Text = "";
                txthorasPlanAgregar.Text = "";
                lblErrorPlanAgregar.Visible = false;
            }
            catch (FormatException)
            {
                // Validar que el formato sea válido (por ejemplo si el usuario escribe letras)
                lblErrorPlanAgregar.Text = "Verificá que el monto y las horas sean números válidos.";
                lblErrorPlanAgregar.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorPlanAgregar.Text = "Ocurrió un error: " + ex.Message;
                lblErrorPlanAgregar.Visible = true;
            }
        }

        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            int planId = int.Parse(ddlPlan.SelectedValue);
            PlanNegocio negocio = new PlanNegocio();
            Plan plan = negocio.ObtenerPlanPorId(planId);

            if (plan != null)
            {
                txtHorasSemana.Text = plan.MaxHorasSemana.ToString();
                txtMonto.Text = plan.PrecioMensual.ToString("C", new System.Globalization.CultureInfo("es-AR"));
            }
            else
            {
                txtHorasSemana.Text = "";
                txtMonto.Text = "$0.00";
            }
        }

    }
}