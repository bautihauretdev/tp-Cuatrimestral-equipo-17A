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
    public partial class AltaSocio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardarAltaSocio_Click(object sender, EventArgs e)
        {
            try
            {
                Socio nuevo = new Socio
                {
                    Nombre = txtNombreAltaSocio.Text,
                    Apellido = txtApellidoAltaSocio.Text,
                    Dni = txtDniAltaSocio.Text,
                    FechaNacimiento = DateTime.Parse(txtFechaNacAltaSocio.Text),
                    Telefono = txtTelefonoAltaSocio.Text,
                    Email = txtEmailAltaSocio.Text,
                    Activo = true
                };
                SocioNegocio socioNegocio = new SocioNegocio();
                int idSocio = socioNegocio.Agregar(nuevo);

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                usuarioNegocio.AgregarUsuarioSocio(nuevo.Email, nuevo.Dni, idSocio);

                lblMensajeAltaSocio.Text = "Socio y usuario creados correctamente. Contraseña por defecto: DNI";
                lblMensajeAltaSocio.Visible = true;
                lblErrorAltaSocio.Visible = false;
            }
            catch (Exception ex)
            {
                lblErrorAltaSocio.Text = "Error: " + ex.Message;
                lblErrorAltaSocio.Visible = true;
                lblMensajeAltaSocio.Visible = false;
            }
        }
    }
}