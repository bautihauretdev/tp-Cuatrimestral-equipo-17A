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
    public partial class SocioMiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosUsuarioLogueado();
            }

        }

        private void CargarDatosUsuarioLogueado()
        {
            try
            {
                // asume que en el login lo guardamos en el objeto Usuario en Session["usuario"]
                Usuario usuario = Session["usuario"] as Usuario;

                if (usuario == null)
                {
                    lblMensaje.Text = "No hay usuario logueado.";
                    lblMensaje.Visible = true;
                    return;
                }

                if (usuario.Socio == null || usuario.Socio.IdSocio <= 0)
                {
                    lblMensaje.Text = "No se encontró información del socio asociado al usuario.";
                    lblMensaje.Visible = true;
                    return;
                }

                SocioNegocio socioNegocio = new SocioNegocio();
                Socio socio = socioNegocio.ObtenerPorId(usuario.Socio.IdSocio);

                if (socio == null)
                {
                    lblMensaje.Text = "No se pudo cargar los datos del socio.";
                    lblMensaje.Visible = true;
                    return;
                }

               
                txtNombre.Text = socio.Nombre ?? "";
                txtApellido.Text = socio.Apellido ?? "";
                txtDni.Text = socio.Dni ?? "";

              
                txtFechaNacimiento.Text = socio.FechaNacimiento != DateTime.MinValue
                    ? socio.FechaNacimiento.ToString("yyyy-MM-dd")
                    : "";

                txtTelefono.Text = socio.Telefono ?? "";
                txtEmail.Text = socio.Email ?? "";

                // La contraseña no se muestra (se deja enmascarada).
                txtboxPlan.Text = socio.Plan != null ? socio.Plan.ToString() : ""; // FALTA IMPLEMENTAR PLAN

                lblMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar perfil: " + ex.Message;
                lblMensaje.Visible = true;
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {

        }

        protected void btnEditarDatos_Click(object sender, EventArgs e)
        {

        }
    }
}