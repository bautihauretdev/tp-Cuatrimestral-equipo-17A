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

        protected void btnEditarPlan_Click(object sender, EventArgs e)
        {
            // Si no hay plan seleccionado, no hacer nada
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            int planId = int.Parse(ddlPlan.SelectedValue);
            PlanNegocio negocio = new PlanNegocio();
            Plan plan = negocio.ObtenerPlanPorId(planId);

            if (plan != null)
            {
                // Carga los datos del plan en los TextBox de la ventana flotante
                txtNombrePlanEditar.Text = plan.Nombre;
                txthorasPlanEditar.Text = plan.MaxHorasSemana.ToString();
                txtMontoPlanEditar.Text = plan.PrecioMensual.ToString("0.00");

                // Mostrar modal en Bootstrap 5 sin jQuery
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModalEditar",
                    "var modal = new bootstrap.Modal(document.getElementById('modalEditarPlan')); modal.show();", true);
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            int planId = int.Parse(ddlPlan.SelectedValue);
            PlanNegocio negocio = new PlanNegocio();
            Plan plan = negocio.ObtenerPlanPorId(planId);

            if (plan != null)
            {
                // Solo actualizamos el monto
                plan.PrecioMensual = decimal.Parse(txtMontoPlanEditar.Text);

                negocio.Modificar(plan);

                // Refrescamos el dropdown y los datos visibles
                CargarPlanes();
                ddlPlan.SelectedValue = plan.IdPlan.ToString();
                txtHorasSemana.Text = plan.MaxHorasSemana.ToString();
                txtMonto.Text = plan.PrecioMensual.ToString("C", new System.Globalization.CultureInfo("es-AR"));
            }
        }

        protected void btnEliminarPlan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            // Mostrar nombre del plan en el modal
            PlanNegocio negocio = new PlanNegocio();
            int planId = int.Parse(ddlPlan.SelectedValue);
            Plan plan = negocio.ObtenerPlanPorId(planId);

            if (plan != null)
            {
                // Poner el nombre en el <span> del modal
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModalEliminar",
                    $"document.getElementById('planNombreEliminar').innerText = '{plan.Nombre}'; " +
                    $"var modal = new bootstrap.Modal(document.getElementById('modalEliminarPlan')); modal.show();", true);
            }
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            int planId = int.Parse(ddlPlan.SelectedValue);
            PlanNegocio negocio = new PlanNegocio();
            negocio.BajaLogica(planId);

            // Recargar dropdown y limpiar campos
            CargarPlanes();
            ddlPlan.SelectedIndex = 0;
            txtHorasSemana.Text = "";
            txtMonto.Text = "";
        }
    }
}