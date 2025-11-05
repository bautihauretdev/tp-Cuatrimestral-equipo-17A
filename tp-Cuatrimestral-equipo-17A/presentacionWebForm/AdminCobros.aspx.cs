using negocio;
using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class AdminCobros : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarSocios();
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtMes.Text = DateTime.Now.ToString("MM/yyyy");
                CargarHistorialCobros();
            }
        }

        private void CargarSocios()
        {
            var cuotaNegocio = new CuotaNegocio();
            var socios = cuotaNegocio.ListarSocios();

            var sociosLista = socios.Select(s => new
            {
                IdSocio = s.IdSocio,
                NombreCompleto = s.Nombre + " " + s.Apellido
            }).ToList();

            ddlSocio.DataSource = sociosLista;
            ddlSocio.DataTextField = "NombreCompleto";
            ddlSocio.DataValueField = "IdSocio";
            ddlSocio.DataBind();
            ddlSocio.Items.Insert(0, new ListItem("Seleccionar socio...", ""));
        }

        protected void ddlSocio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSocio.SelectedValue == "")
                return;

            int socioId = int.Parse(ddlSocio.SelectedValue);
            CuotaNegocio negocio = new CuotaNegocio();
            Cuota cuota = negocio.ObtenerCuotaActual(socioId);

            if (cuota != null)
            {
                txtPlan.Text = cuota.Socio?.Plan?.Nombre.ToString() ?? "Plan no disponible";
                txtRecargo.Text = cuota.Recargo.ToString("C");
                txtMonto.Text = (cuota.Monto + cuota.Recargo).ToString("C");
            }
            else
            {
                txtPlan.Text = "";
                txtRecargo.Text = "$0.00";
                txtMonto.Text = "$0.00";
            }
        }

        protected void btnGuardarCobro_Click(object sender, EventArgs e)
        {
            if (ddlSocio.SelectedValue == "")
                return;

            int socioId = int.Parse(ddlSocio.SelectedValue);
            CuotaNegocio negocio = new CuotaNegocio();
            Cuota cuota = negocio.ObtenerCuotaActual(socioId);

            if (cuota != null)
            {
                negocio.ActualizarEstadoCuota(cuota.IdCuota, "Pagado");
            }

            // Limpiar campos
            ddlSocio.SelectedIndex = 0;
            txtPlan.Text = "";
            txtRecargo.Text = "";
            txtMonto.Text = "";
            txtMes.Text = DateTime.Now.ToString("MM/yyyy");
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

            CargarHistorialCobros();
        }

        private void CargarHistorialCobros()
        {
            CuotaNegocio negocio = new CuotaNegocio();
            List<Cuota> cuotasPagadas = negocio.ObtenerCuotasPagadas();

            var datos = cuotasPagadas.Select(c => new
            {
                Socio = c.Socio.Nombre + " " + c.Socio.Apellido,
                FechaCobro = c.FechaPago.HasValue ? c.FechaPago.Value.ToString("dd/MM/yyyy") : "Sin fecha",
                Periodo = $"{c.Mes:D2}/{c.Anio}",
                Monto = c.Monto + c.Recargo,
                FormaPago = "N/A" // si no tenés forma de pago en Cuota
            }).ToList();

            gvHistorialCobros.DataSource = datos;
            gvHistorialCobros.DataBind();
        }
    }
}