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
                if (string.IsNullOrEmpty(hfIdSocioSeleccionado.Value))
                {
                    lblErrorBusqueda.Text = "Por favor, seleccione un socio antes de editar.";
                    lblErrorBusqueda.Visible = true;
                    return;
                }

                int idSocio;
                if (!int.TryParse(hfIdSocioSeleccionado.Value, out idSocio))
                    return;

                SocioNegocio negocio = new SocioNegocio();
                Socio socio = negocio.ObtenerPorId(idSocio);

                if (!socio.Activo)
                {
                    hfIdSocioInactivo.Value = socio.IdSocio.ToString();

                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "MostrarModalInactivo",
                        @"var modal = new bootstrap.Modal(document.getElementById('modalEditarInactivo'));
                        modal.show();",
                        true
                    );

                    return;
                }

                // Socio activo → va directo a edición
                Session["EditarSocioId"] = idSocio;
                Response.Redirect("AltaSocio.aspx");
            }
        }

        protected void btnReactivarDesdeEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio = int.Parse(hfIdSocioInactivo.Value);
                SocioNegocio socioNegocio = new SocioNegocio();

                socioNegocio.AltaLogica(idSocio);

                // Actualiza el estado visual en pantalla
                lblEstado.Text = "Activo";

                // Mensaje pequeño opcional
                lblErrorBusqueda.Text = "El socio ha sido reactivado. Ahora puede editarlo.";
                lblErrorBusqueda.Visible = true;

                // Cierra modal desde servidor
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "CerrarModal",
                    @"var modal = bootstrap.Modal.getInstance(document.getElementById('modalEditarInactivo'));
                     if(modal) modal.hide();",
                    true
                );
            }
            catch (Exception ex)
            {
                lblErrorBusqueda.Text = "Error al reactivar socio: " + ex.Message;
                lblErrorBusqueda.Visible = true;
            }
        }

    }
}