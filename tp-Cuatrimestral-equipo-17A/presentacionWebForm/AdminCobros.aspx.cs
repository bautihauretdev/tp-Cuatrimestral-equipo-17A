using dominio;
using negocio;
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
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtMes.Text = DateTime.Now.ToString("MM/yyyy");
                CargarHistorialCobros();
            }
        }
        protected void btnBuscarSocio_Click(object sender, EventArgs e)
        {
            string criterio = txtSearchSocio.Text?.Trim();
            if (string.IsNullOrEmpty(criterio))
            {
                lblSocioSeleccionado.Text = "Ingrese DNI o nombre para buscar.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
                return;
            }

            var cuotaNegocio = new CuotaNegocio();
            Socio socioEncontrado = cuotaNegocio.BuscarSocioPorDniONombre(criterio);

            if (socioEncontrado != null && socioEncontrado.Plan != null)
            {
                hfIdSocioSeleccionado.Value = socioEncontrado.IdSocio.ToString();
                lblSocioSeleccionado.Text = socioEncontrado.Nombre + " " + socioEncontrado.Apellido;

                txtPlan.Text = socioEncontrado.Plan.Nombre;
                txtMonto.Text = socioEncontrado.Plan.PrecioMensual.ToString("C");
                txtRecargo.Text = "$0.00"; 
            }
            else
            {
                lblSocioSeleccionado.Text = "Socio no encontrado.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
            }
        }

        protected void btnGuardarCobro_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfIdSocioSeleccionado.Value))
                return;

            int socioId = int.Parse(hfIdSocioSeleccionado.Value);
            CuotaNegocio negocio = new CuotaNegocio();
            Cuota cuota = negocio.ObtenerCuotaActual(socioId);

            if (cuota != null)
            {
                negocio.ActualizarEstadoCuota(cuota.IdCuota, "Pagado");
            }

            hfIdSocioSeleccionado.Value = "";
            txtSearchSocio.Text = "";
            lblSocioSeleccionado.Text = "";
            txtPlan.Text = "";
            txtRecargo.Text = "";
            txtMonto.Text = "";
            txtMes.Text = DateTime.Now.ToString("MM/yyyy");
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

            CargarHistorialCobros();
        }

        private void LimpiarCamposCuota()
        {
            txtPlan.Text = "";
            txtRecargo.Text = "$0.00";
            txtMonto.Text = "$0.00";
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
                FormaPago = "N/A"
            }).ToList();

            gvHistorialCobros.DataSource = datos;
            gvHistorialCobros.DataBind();
        }
    }
}