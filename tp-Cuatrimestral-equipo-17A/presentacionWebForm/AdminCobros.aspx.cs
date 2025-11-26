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
        // se ejecuta automaticamente al cargar la pantalla, el historial de cobros con sus datos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CargarHistorialCobros();
            }

        }

        // Botón para buscar socio por DNI
        protected void btnBuscarSocio_Click(object sender, EventArgs e)
        {
            string criterio = txtSearchSocio.Text?.Trim();
            if (string.IsNullOrEmpty(criterio))
            { // Si no se ingresó criterio


                lblSocioSeleccionado.Text = "Ingrese DNI para buscar.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
                ddlCuotasPendientes.Items.Clear();
                ddlCuotasPendientes.Items.Add(new ListItem("No tiene cuotas pendientes", ""));
                return;
            }

            CuotaNegocio negocio = new CuotaNegocio();
            Socio socioEncontrado = negocio.BuscarSocioPorDniONombre(criterio);

            if (socioEncontrado != null && socioEncontrado.Plan != null)

            { // GUARDA EL ID DEL SOCIO 
                hfIdSocioSeleccionado.Value = socioEncontrado.IdSocio.ToString();
                lblSocioSeleccionado.Text = socioEncontrado.Nombre + " " + socioEncontrado.Apellido;
                // MUESTRA AUTOMATICAMENTE EL PLAN Y EL MONTO TOTAL ( YA SEA CON O SIN RECARGA DEL 5%)
                txtPlan.Text = socioEncontrado.Plan.Nombre;
                txtMonto.Text = socioEncontrado.Plan.PrecioMensual.ToString("C");
                // SE HACE EL CALCULO DEL RECARGO AUTOMATICO (5% EN CAOS DE QUE DIA > 5)
                decimal recargo = DateTime.Now.Day > 5 ? socioEncontrado.Plan.PrecioMensual * 0.05m : 0;
                txtRecargo.Text = recargo.ToString("C");

                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Cargar cuotas pendientes
                var cuotasPendientes = negocio.ObtenerCuotasDeudorasPorSocio(socioEncontrado.IdSocio);
                ddlCuotasPendientes.Items.Clear();

                if (cuotasPendientes.Count > 0)
                {
                    foreach (var cuota in cuotasPendientes)
                    {
                        ddlCuotasPendientes.Items.Add(
                            new ListItem($"{cuota.Mes:D2}/{cuota.Anio} - {cuota.Estado} (${cuota.Monto})", cuota.IdCuota.ToString())
                        );
                    }
                    ddlCuotasPendientes.SelectedIndex = 0; // siempre la más vieja
                }
                else
                {
                    ddlCuotasPendientes.Items.Add(new ListItem("No tiene cuotas pendientes", ""));
                }
            }
            else
            { // socio NO encontrado 
                lblSocioSeleccionado.Text = "Socio no encontrado.";
                hfIdSocioSeleccionado.Value = "";
                LimpiarCamposCuota();
                ddlCuotasPendientes.Items.Clear();
                ddlCuotasPendientes.Items.Add(new ListItem("No tiene cuotas pendientes", ""));
            }
        }

        // BOTON PARA GUARDAR EL COBRO CON TODA SU INFORMACION 
        protected void btnGuardarCobro_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfIdSocioSeleccionado.Value))
                return;

            int socioId = int.Parse(hfIdSocioSeleccionado.Value);
            decimal monto = 0;
            decimal recargo = 0;

            decimal.TryParse(txtMonto.Text.Replace("$", ""), out monto);
            // CALCULAR RECARGO (5%)
            if (DateTime.Now.Day > 5)
            {
                recargo = monto * 0.05m;
            }
            // DETERMINAMOS LA FORMA DE PAGO : sea efectivo o transferencia 
            string formaPago = rbEfectivo.Checked ? "Efectivo" : "Transferencia";
            CuotaNegocio negocio = new CuotaNegocio();
            // si se encuentra cuotas pendientes
            if (ddlCuotasPendientes.Items.Count > 0 && !string.IsNullOrEmpty(ddlCuotasPendientes.SelectedValue))
            {
                int idCuotaSeleccionada = int.Parse(ddlCuotasPendientes.SelectedValue);
                int idCuotaMasVieja = int.Parse(ddlCuotasPendientes.Items[0].Value);
                // VALIDACION : siempre cobrar PRIMERO la cuota mas vieja 
                if (idCuotaSeleccionada != idCuotaMasVieja)
                {
                    lblSocioSeleccionado.Text = "⚠️ Debe cobrarse primero la cuota más vieja.";
                    return;
                }
                // se guarda el cobro de cuota pendiente 
                negocio.GuardarCobroPendiente(idCuotaSeleccionada, monto, recargo, formaPago);
            }
            else
            { // si no se guarda cobro de cuota actual 
                negocio.GuardarCobro(socioId, monto, recargo, formaPago);
            }

            hfIdSocioSeleccionado.Value = "";
            txtSearchSocio.Text = "";
            lblSocioSeleccionado.Text = "";
            txtPlan.Text = "";
            txtRecargo.Text = "$0.00";
            txtMonto.Text = "$0.00";
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            rbEfectivo.Checked = true;
            rbTransferencia.Checked = false;
            // se refresca el historial 
            CargarHistorialCobros();
        }
        private void LimpiarCamposCuota()
        {
            txtPlan.Text = "";
            txtRecargo.Text = "$0.00";
            txtMonto.Text = "$0.00";
        }

        // permite mostrar automatiacmente el historial de los cobros al ejecutar la ventana de cobros de la parte de AdminCobros
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
