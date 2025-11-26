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
                lblSocioSeleccionado.Text = "Ingrese DNI para buscar.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
                return;
            }

            CuotaNegocio negocio = new CuotaNegocio();
            Socio socioEncontrado = negocio.BuscarSocioPorDniONombre(criterio);

            if (socioEncontrado != null && socioEncontrado.Plan != null)
            {
                
                hfIdSocioSeleccionado.Value = socioEncontrado.IdSocio.ToString();

                
                lblSocioSeleccionado.Text = socioEncontrado.Nombre + " " + socioEncontrado.Apellido;

               
                txtPlan.Text = socioEncontrado.Plan.Nombre;
                txtMonto.Text = socioEncontrado.Plan.PrecioMensual.ToString("C");


                decimal recargo = 0;
                if (DateTime.Now.Day > 5)
                {
                    recargo = socioEncontrado.Plan.PrecioMensual * 0.05m;
                }

                txtRecargo.Text = recargo.ToString("C");

                
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtMes.Text = DateTime.Now.ToString("MM/yyyy");
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
            decimal monto = 0;
            decimal recargo = 0;

            decimal.TryParse(txtMonto.Text.Replace("$", ""), out monto);

            // Validación automática de recargo (5% si ya pasó el día 5)
            if (DateTime.Now.Day > 5)
            {
                recargo = monto * 0.05m;
            }


            string formaPago = rbEfectivo.Checked ? "Efectivo" : "Transferencia";

            CuotaNegocio negocio = new CuotaNegocio();
            negocio.GuardarCobro(socioId, monto, recargo, formaPago);

            hfIdSocioSeleccionado.Value = "";
            txtSearchSocio.Text = "";
            lblSocioSeleccionado.Text = "";
            txtPlan.Text = "";
            txtRecargo.Text = "$0.00";
            txtMonto.Text = "$0.00";
            txtMes.Text = DateTime.Now.ToString("MM/yyyy");
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            rbEfectivo.Checked = true;
            rbTransferencia.Checked = false;

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
                FormaPago = string.IsNullOrEmpty(c.FormaPago) ? "No registrado" : c.FormaPago
            }).ToList();

            gvHistorialCobros.DataSource = datos;
            gvHistorialCobros.DataBind();
        }
    }
}
