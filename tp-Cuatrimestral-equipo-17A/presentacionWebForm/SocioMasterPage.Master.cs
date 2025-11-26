using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacionWebForm
{
    public partial class SocioMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarNotificaciones();
        }

        private void CargarNotificaciones()
        {
            Usuario usuarioLogueado = Session["usuario"] as Usuario;
            if (usuarioLogueado == null || usuarioLogueado.Socio == null)
                return;

            int idSocio = usuarioLogueado.Socio.IdSocio;

            NotificacionNegocio notifNegocio = new NotificacionNegocio();
            List<Notificacion> notifs = notifNegocio.ObtenerNoLeidasPorSocio(idSocio);

            if (notifs.Count > 0)
            {
                // Bind al Repeater
                rptNotificaciones.DataSource = notifs;
                rptNotificaciones.DataBind();

                // Oculta el mensaje "sin notificaciones"
                lblSinNotificaciones.Visible = false;

                // Muestra cantidad en la campanita
                lblCantNotif.Text = notifs.Count.ToString();
                lblCantNotif.Visible = true;

            }
            else
            {
                rptNotificaciones.DataSource = null;
                rptNotificaciones.DataBind();

                lblSinNotificaciones.Visible = true;
                lblCantNotif.Visible = false;
            }
        }

        protected void btnMarcarLeidas_Click(object sender, EventArgs e)
        {
            Usuario usuarioLogueado = Session["usuario"] as Usuario;
            if (usuarioLogueado == null || usuarioLogueado.Socio == null)
                return;

            int idSocio = usuarioLogueado.Socio.IdSocio;

            NotificacionNegocio negocio = new NotificacionNegocio();
            negocio.MarcarComoLeidas(idSocio);

            // Refrescar para ocultar contador
            CargarNotificaciones();
        }
    }
}