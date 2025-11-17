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
                string nombre = txtNombrePlanAgregar.Text.Trim();
                string horasTxt = txthorasPlanAgregar.Text.Trim();
                string montoTxt = txtMontoPlanAgregar.Text.Trim();

                // (1) Validar que todos los campos estén completos
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(horasTxt) || string.IsNullOrEmpty(montoTxt))
                {
                    lblErrorPlanAgregar.Text = "Por favor completá todos los campos.";
                    lblErrorPlanAgregar.Visible = true;
                    MantenerModalAbierto();
                    return;
                }

                // (2) Validar formato numérico y valores mayores a 0
                if (!int.TryParse(horasTxt, out int horas) || horas <= 0)
                {
                    lblErrorPlanAgregar.Text = "Las horas por semana deben ser un número mayor a 0.";
                    lblErrorPlanAgregar.Visible = true;
                    MantenerModalAbierto();
                    return;
                }

                if (!decimal.TryParse(montoTxt, out decimal monto) || monto <= 0)
                {
                    lblErrorPlanAgregar.Text = "El monto debe ser un número válido mayor a 0.";
                    lblErrorPlanAgregar.Visible = true;
                    MantenerModalAbierto();
                    return;
                }

                // (3) Validar que no exista un plan con el mismo nombre o mismas horas
                PlanNegocio planNegocio = new PlanNegocio();
                List<Plan> planesExistentes = planNegocio.ListarPlanes();

                bool nombreExiste = planesExistentes.Any(p =>
                    p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                bool horasExiste = planesExistentes.Any(p => p.MaxHorasSemana == horas);

                if (nombreExiste)
                {
                    lblErrorPlanAgregar.Text = $"Ya existe un plan con el nombre \"{nombre}\".";
                    lblErrorPlanAgregar.Visible = true;
                    MantenerModalAbierto();
                    return;
                }

                if (horasExiste)
                {
                    lblErrorPlanAgregar.Text = $"Ya existe un plan con {horas} horas semanales.";
                    lblErrorPlanAgregar.Visible = true;
                    MantenerModalAbierto();
                    return;
                }

                // Si todo está OK, creamos y guardamos el plan
                Plan nuevo = new Plan
                {
                    Nombre = nombre,
                    PrecioMensual = monto,
                    MaxHorasSemana = horas,
                    Activo = true
                };

                int idPlan = planNegocio.Agregar(nuevo);

                // Refrescar lista y limpiar campos
                CargarPlanes();
                ddlPlan.SelectedIndex = 0;
                txtHorasSemana.Text = "";
                txtMonto.Text = "";
                txtNombrePlanAgregar.Text = "";
                txtMontoPlanAgregar.Text = "";
                txthorasPlanAgregar.Text = "";
                lblErrorPlanAgregar.Visible = false;
            }
            catch (Exception ex)
            {
                lblErrorPlanAgregar.Text = "Ocurrió un error: " + ex.Message;
                lblErrorPlanAgregar.Visible = true;

                // Mantiene el modal abierto sin limpiar campos.
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "NoLimpiarAgregar",
                    "limpiarCamposAgregar = false;", true);

                MantenerModalAbierto();
            }
        }

        private void MantenerModalAbierto()
        {
            ScriptManager.RegisterStartupScript(
                Page,
                Page.GetType(),
                "ShowModalAgregar",
                "var modal = new bootstrap.Modal(document.getElementById('modalAgregarPlan')); modal.show();",
                true
            );
        }


        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
            {
                // Limpiar campos si no hay plan seleccionado
                txtHorasSemana.Text = "";
                txtMonto.Text = "";
                return;
            }

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
            // Valida selección
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            // Validar rango de fechas: solo del 25 al 31
            int diaActual = DateTime.Now.Day;
            if (diaActual < 25 || diaActual > 31)
            {
                lblErrorAbrirEditar.Text = "La edición de planes estará disponible a partir del día 25 y hasta que termine el mes";
                lblErrorAbrirEditar.Visible = true;
                return;
            }

            // Si está dentro del rango limpia el label
            lblErrorAbrirEditar.Visible = false;

            // Cargar los datos del plan
            int planId = int.Parse(ddlPlan.SelectedValue);
            PlanNegocio negocio = new PlanNegocio();
            Plan plan = negocio.ObtenerPlanPorId(planId);

            if (plan != null)
            {
                txtNombrePlanEditar.Text = plan.Nombre;
                txthorasPlanEditar.Text = plan.MaxHorasSemana.ToString();
                txtMontoPlanEditar.Text = plan.PrecioMensual.ToString("0.00");

                // Mostrar modal
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
                decimal nuevoMonto;
                bool esNumero = decimal.TryParse(txtMontoPlanEditar.Text, out nuevoMonto);

                if (!esNumero || nuevoMonto <= 0)
                {
                    // Mostrar mensaje de error en el Label
                    lblErrorPlanEditar.Text = "Por favor ingresá un monto válido mayor a 0.";
                    lblErrorPlanEditar.Visible = true;

                    // Mantener el modal abierto
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModalEditar",
                        "var modal = new bootstrap.Modal(document.getElementById('modalEditarPlan')); modal.show();", true);

                    return;
                }

                // Si todo está bien, ocultar el label
                lblErrorPlanEditar.Visible = false;

                // Guardar el cambio
                plan.PrecioMensual = nuevoMonto;
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