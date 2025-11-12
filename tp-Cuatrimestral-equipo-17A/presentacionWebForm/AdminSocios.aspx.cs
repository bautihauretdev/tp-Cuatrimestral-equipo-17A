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
    public partial class AdminSocios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAgregarSocio_Click(object sender, EventArgs e)
        {
            Response.Redirect("AltaSocio.aspx");
        }

        private void LimpiarPanelSocio()
        {
            lblNombre.Text = lblApellido.Text = lblDNI.Text = lblFechaNacimiento.Text = lblTelefono.Text = lblEmail.Text = "";
            lblNombreCompleto.Text = lblEstado.Text = "";
            hfIdSocioSeleccionado.Value = "";
        }

        protected void btnBuscarDNI_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    SocioNegocio negocio = new SocioNegocio();
                    string dni = txtSearchDNI.Text.Trim();
                    Socio socio = negocio.ObtenerPorDni(dni);

                    if (socio != null)
                    {
                        lblNombre.Text = socio.Nombre;
                        lblApellido.Text = socio.Apellido;
                        lblDNI.Text = socio.Dni;
                        lblFechaNacimiento.Text = socio.FechaNacimiento.ToString("dd/MM/yyyy");
                        lblTelefono.Text = socio.Telefono;
                        lblEmail.Text = socio.Email;
                        lblNombreCompleto.Text = $"{socio.Nombre} {socio.Apellido}";
                        hfIdSocioSeleccionado.Value = socio.IdSocio.ToString();

                        if (socio.Activo)
                        {
                            lblEstado.Text = "Activo";
                            lblErrorBusqueda.Visible = false;
                        }
                        else
                        {
                            lblEstado.Text = "Inactivo";
                            lblErrorBusqueda.Visible = false;
                        }
                    }
                    else
                    {
                        LimpiarPanelSocio();
                        lblErrorBusqueda.Text = "No se encontró ningún socio con ese DNI.";
                        lblErrorBusqueda.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    LimpiarPanelSocio();
                    lblErrorBusqueda.Text = "Error al buscar socio: " + ex.Message;
                    lblErrorBusqueda.Visible = true;
                }
            }
        }

        protected void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    if (!string.IsNullOrEmpty(hfIdSocioSeleccionado.Value))
                    {
                        int idSocio = int.Parse(hfIdSocioSeleccionado.Value);
                        SocioNegocio negocio = new SocioNegocio();
                        negocio.BajaLogica(idSocio);

                        // Limpia la UI y muestra mensaje
                        LimpiarPanelSocio();
                        lblErrorBusqueda.Text = "Socio dado de baja correctamente.";
                        lblErrorBusqueda.Visible = true;
                    }
                    else
                    {
                        lblErrorBusqueda.Text = "Por favor, busque un socio primero.";
                        lblErrorBusqueda.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblErrorBusqueda.Text = "Error al dar de baja: " + ex.Message;
                    lblErrorBusqueda.Visible = true;
                }
            }
        }

        protected void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfIdSocioSeleccionado.Value))
            {
                int id;
                if (int.TryParse(hfIdSocioSeleccionado.Value, out id))
                { 
                    Session["EditarSocioId"] = id;
                    Response.Redirect("AltaSocio.aspx");
                    return;
                }
            }

            lblErrorBusqueda.Text = "Por favor, seleccione un socio antes de editar.";
            lblErrorBusqueda.Visible = true;
        }
    }
}