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
        private const string SESSION_KEY_EDIT_ID = "EditarSocioId";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Primero: comprobar si hay edición por Session (preferible)
                object sessionId = Session[SESSION_KEY_EDIT_ID];
                if (sessionId != null)
                {
                    int idSesion;
                    if (int.TryParse(sessionId.ToString(), out idSesion))
                    {
                        CargarParaEdicion(idSesion);
                        return;
                    }
                }
                // Fallback: compatibilidad con query string
                string idStr = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idStr))
                {
                    int id;
                    if (int.TryParse(idStr, out id))
                    {
                        CargarParaEdicion(id);
                    }
                }
            }
        }

        private void CargarParaEdicion(int idSocio)
        {
            try
            {
                SocioNegocio sn = new SocioNegocio();
                Socio socio = sn.ObtenerPorId(idSocio);
                if (socio == null) return;

                //RELLENA LOS CAMPOS
                hfIdEditar.Value = socio.IdSocio.ToString();
                txtNombreAltaSocio.Text = socio.Nombre;
                txtApellidoAltaSocio.Text = socio.Apellido;
                txtDniAltaSocio.Text = socio.Dni;
                txtFechaNacAltaSocio.Text = socio.FechaNacimiento != DateTime.MinValue ? socio.FechaNacimiento.ToString("yyyy-MM-dd") : "";
                txtTelefonoAltaSocio.Text = socio.Telefono;
                txtEmailAltaSocio.Text = socio.Email;

                //AJUSTES EN MODO EDICION
                txtDniAltaSocio.ReadOnly = true; //DNI no editable
                btnGuardarAltaSocio.Visible = false; //oculta boton alta
                btnGuardarCambios.Visible = true;  //muestra guardar cambios
                lblMensajeAltaSocio.Text = "Editando socio existente";
                lblMensajeAltaSocio.Visible = true;
                lblErrorAltaSocio.Visible = false;
            }
            catch (Exception ex)
            {
                lblErrorAltaSocio.Text = "Error cargando socio para edición: " + ex.Message;
                lblErrorAltaSocio.Visible = true;
            }
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

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfIdEditar.Value))
                {
                    lblErrorAltaSocio.Text = "No se pudo identificar el socio a modificar.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                int idSocio;
                if (!int.TryParse(hfIdEditar.Value, out idSocio))
                {
                    lblErrorAltaSocio.Text = "Id de socio inválido.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                //Carga usuario asociado para detectar cambio de email 
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                SocioNegocio socioNegocio= new SocioNegocio();

                //Lee campos del formulario y construye objeto Socio
                Socio socio = new Socio
                {
                    IdSocio = idSocio,
                    Nombre = txtNombreAltaSocio.Text.Trim(),
                    Apellido = txtApellidoAltaSocio.Text.Trim(),
                    Dni = txtDniAltaSocio.Text.Trim(), // no editable 
                };

                DateTime fecha;
                if (DateTime.TryParse(txtFechaNacAltaSocio.Text, out fecha))
                    socio.FechaNacimiento = fecha;
                else
                    socio.FechaNacimiento = DateTime.MinValue;

                socio.Telefono = txtTelefonoAltaSocio.Text.Trim();
                socio.Email = txtEmailAltaSocio.Text.Trim();
                socio.Activo = true; // mantenemos activo por defecto

                //Obtiene el socio original para comparar email 
                Socio socioOriginal = socioNegocio.ObtenerPorId(idSocio);
                string emailOriginal = socioOriginal != null ? socioOriginal.Email : null;

                //Si el email cambio, actualiza tabla USUARIOS tambien
                bool emailCambia = !string.Equals(emailOriginal ?? "", socio.Email ?? "", StringComparison.OrdinalIgnoreCase);
                if (emailCambia)
                {
                    //Verifica que el nuevo email no este en uso por otro usuario
                    if (usuarioNegocio.EmailEnUsoPorOtro(socio.Email, idSocio))
                    {
                        lblErrorAltaSocio.Text = "El email ingresado ya está en uso por otro usuario.";
                        lblErrorAltaSocio.Visible = true;
                        return;
                    }
                }

                // Guarda cambios en SOCIOS
                socioNegocio.Modificar(socio);

                //Si cambio email actualiza USUARIOS
                if (emailCambia)
                {
                    usuarioNegocio.ActualizarEmailPorIdSocio(idSocio, socio.Email);
                }

                //Limpia la Session usada para edición
                Session.Remove(SESSION_KEY_EDIT_ID);

                lblMensajeAltaSocio.Text = "Datos del socio actualizados correctamente.";
                lblMensajeAltaSocio.Visible = true;
                lblErrorAltaSocio.Visible = false;

                // Redirigir de vuelta a AdminSocios (opcional)
                Response.Redirect("AdminSocios.aspx");
            }
            catch (Exception ex)
            {
                lblErrorAltaSocio.Text = "Error al guardar cambios: " + ex.Message;
                lblErrorAltaSocio.Visible = true;
            }
        }
    }
}