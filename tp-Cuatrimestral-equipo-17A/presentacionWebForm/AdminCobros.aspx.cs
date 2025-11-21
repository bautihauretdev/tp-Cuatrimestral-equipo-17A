using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            string termino = txtSearchSocio.Text?.Trim();
            if (string.IsNullOrEmpty(termino))
            {
                lblSocioSeleccionado.Text = "Ingrese DNI o nombre para buscar.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
                return;
            }

            Socio socioEncontrado = null;
            var socioNegocio = new SocioNegocio();

            if (Regex.IsMatch(termino, @"^\d+$"))
            {
                socioEncontrado = socioNegocio.ObtenerPorDni(termino);
            }

            if (socioEncontrado == null)
            {
                var cuotaNegocio = new CuotaNegocio();
                var listaSocios = cuotaNegocio.ListarSocios();
                socioEncontrado = listaSocios.FirstOrDefault(s =>
                    (s.Nombre + " " + s.Apellido).IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0
                    || s.Nombre.IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0
                    || s.Apellido.IndexOf(termino, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (socioEncontrado != null)
            {
                hfIdSocioSeleccionado.Value = socioEncontrado.IdSocio.ToString();
                lblSocioSeleccionado.Text = socioEncontrado.Nombre + " " + socioEncontrado.Apellido;
                CargarDatosSocioParaCobro(socioEncontrado.IdSocio);
            }
            else
            {
                lblSocioSeleccionado.Text = "Socio no encontrado.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
            }
        }

        private void CargarDatosSocioParaCobro(int socioId)
        {
            CuotaNegocio negocio = new CuotaNegocio();
            Cuota cuota = negocio.ObtenerCuotaActual(socioId);

            if (cuota != null)
            {
                txtPlan.Text = cuota.Socio?.Plan?.Nombre ?? "Plan no disponible";
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