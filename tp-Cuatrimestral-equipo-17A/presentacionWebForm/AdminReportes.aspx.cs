using negocio;
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
                CargarReportes();
            }
        }
        private void CargarReportes()
        {
            ReportesNegocio reportesNegocio = new ReportesNegocio();

            // MÉTRICAS
            lblSociosActivos.Text = reportesNegocio.ObtenerTotalSociosActivos().ToString();
            lblIngresosMes.Text = "$" + reportesNegocio.ObtenerIngresosMes().ToString("N0");
            lblMorosos.Text = reportesNegocio.ObtenerSociosMorosos().ToString();
            lblOcupacionProm.Text = reportesNegocio.OcupacionPromedio().ToString("N2") + "%";

            lblDiaMasConcurrencia.Text = reportesNegocio.DiaMayorConcurrencia();
            lblDiaMenosConcurrencia.Text = reportesNegocio.DiaMenorConcurrencia();

            lblFranjaMasConcurrencia.Text = reportesNegocio.FranjaMayorConcurrencia();
            lblFranjaMenosConcurrencia.Text = reportesNegocio.FranjaMenorConcurrencia();

            // LISTADOS
            rptActivos.DataSource = reportesNegocio.TopSociosReservas();
            rptActivos.DataBind();

            rptMorosos.DataSource = reportesNegocio.TopMorosos();
            rptMorosos.DataBind();

            rptPorVencer.DataSource = reportesNegocio.ProximosVencimientos();
            rptPorVencer.DataBind();
        }
    }
}