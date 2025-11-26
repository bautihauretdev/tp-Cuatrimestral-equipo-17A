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
                Socio socio = socioNegocio.ObtenerSocioYPlanPorId(usuario.Socio.IdSocio);

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

                
                txtboxPlan.Text = socio.Plan != null ? socio.Plan.Nombre : "Sin plan asignado";

                
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
            try
            {
                Usuario usuario = Session["usuario"] as Usuario;

                if (usuario == null)
                {
                    lblMensaje.Text = "No hay usuario en sesión.";
                    lblMensaje.Visible = true;
                    return;
                }

                int idSocio = usuario.Socio.IdSocio;

                SocioNegocio socioNegocio = new SocioNegocio();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

                // Valida que no este en uso por otro usuario el mail
                if (usuarioNegocio.EmailEnUsoPorOtro(txtEmail.Text, idSocio))
                {
                    lblMensaje.Text = "El email ya está en uso por otro usuario.";
                    lblMensaje.Visible = true;
                    return;
                }

                //Actializa los datos del socio 
                socioNegocio.ActualizarPerfil(
                    idSocio,
                    txtNombre.Text,
                    txtApellido.Text,
                    txtEmail.Text
                );

                // actualiza el mail
                usuarioNegocio.ActualizarEmailPorIdSocio(idSocio, txtEmail.Text);

                //Se actualiza contraseña solo si el socio cambio algo
                if (!string.IsNullOrWhiteSpace(txtboxpsswrd.Text) &&
                    txtboxpsswrd.Text != "********")
                {
                    usuarioNegocio.ActualizarPassword(idSocio, txtboxpsswrd.Text);
                }

                //Se vuelve al modo lec
                txtNombre.ReadOnly = true;
                txtApellido.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtboxpsswrd.ReadOnly = true;

                btnGuardarCambios.Visible = false;
                btnEditarDatos.Visible = true;

                lblMensaje.Text = "Datos actualizados correctamente.";
                lblMensaje.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al actualizar: " + ex.Message;
                lblMensaje.Visible = true;
            }
        }

        protected void btnEditarDatos_Click(object sender, EventArgs e)
        {

            // Habilitar solo los editables
            txtNombre.ReadOnly = false;
            txtApellido.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtboxpsswrd.ReadOnly = false;

            //saca la clase bloqueante "perfil-readonly" css
            txtNombre.CssClass = txtNombre.CssClass.Replace("perfil-readonly", "").Trim();
            txtApellido.CssClass = txtApellido.CssClass.Replace("perfil-readonly", "").Trim();
            txtEmail.CssClass = txtEmail.CssClass.Replace("perfil-readonly", "").Trim();
            txtboxpsswrd.CssClass = txtboxpsswrd.CssClass.Replace("perfil-readonly", "").Trim();

            // Mostrar botón guardar
            btnGuardarCambios.Visible = true;
            btnEditarDatos.Visible = false;

            lblMensaje.Visible = false;
        }
    }
}