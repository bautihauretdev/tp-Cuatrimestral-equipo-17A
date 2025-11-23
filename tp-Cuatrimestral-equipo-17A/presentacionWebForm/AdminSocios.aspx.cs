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
            if (!IsPostBack)
            {
                CargarListadoSocios();
                txtBuscarDNI.Text = "";
            }
        }

        private void CargarListadoSocios()
        {
            try
            {
                SocioNegocio negocio = new SocioNegocio();
                var lista = negocio.ListarSociosConPlan();

                rptSocios.DataSource = lista;
                rptSocios.DataBind();

            }
            catch (Exception ex)
            {
                lblErrorBusqueda.Text = "Error al cargar la lista de socios: " + ex.Message;
                lblErrorBusqueda.Visible = true;
            }
        }

        protected void rptSocios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectSocio")
            {
                int idSocio = int.Parse(e.CommandArgument.ToString());
                CargarSocioEnModal(idSocio);
            }
        }

        private void CargarSocioEnModal(int idSocio)
        {
            try
            {
                SocioNegocio negocio = new SocioNegocio();
                Socio socio = negocio.ObtenerSocioYPlanPorId(idSocio);

                if (socio == null)
                {
                    lblErrorBusqueda.Text = "No se pudo cargar la información del socio.";
                    lblErrorBusqueda.Visible = true;
                    return;
                }

                //guarda el ID
                hfIdSocioSeleccionado.Value = socio.IdSocio.ToString();

                //llena la tarjeta de socio
                lblNombre.Text = socio.Nombre;
                lblApellido.Text = socio.Apellido;
                lblDNI.Text = socio.Dni;

                lblFechaNacimiento.Text = socio.FechaNacimiento == DateTime.MinValue
                                          ? "—"
                                          : socio.FechaNacimiento.ToString("dd/MM/yyyy");

                lblTelefono.Text = socio.Telefono;
                lblEmail.Text = socio.Email;

                lblNombreCompleto.Text = $"{socio.Nombre} {socio.Apellido}";

                lblPlanActual.Text = socio.Plan?.Nombre ?? "Sin plan";

                lblEstado.Text = socio.Activo ? "Activo" : "Inactivo";

                //abre el modal
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "MostrarModalSocio",
                    @"var modal = new bootstrap.Modal(document.getElementById('modalSocioDetalle')); 
              modal.show();",
                    true
                );
            }
            catch (Exception ex)
            {
                lblErrorBusqueda.Text = "Error al cargar información del socio: " + ex.Message;
                lblErrorBusqueda.Visible = true;
            }
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
            string dniIngresado= txtBuscarDNI.Text.Trim();

            //VALIDACION 1:SOLO NUMEROS
            if (!dniIngresado.All(char.IsDigit))
            {
                lblErrorBusqueda.Text = "El DNI debe contener solo números.";
                lblErrorBusqueda.Visible = true;
                return;
            }

            //VALIDACION 2: LOGINTUD MINIMA
            if (dniIngresado.Length < 7)
            {
                lblErrorBusqueda.Text = "El DNI debe tener al menos 7 números.";
                lblErrorBusqueda.Visible = true;
                return;
            }

            try
            {
                string dni = txtBuscarDNI.Text.Trim();
                SocioNegocio negocio = new SocioNegocio();

                //si no se ingresa nada se muestran todos los socios
                if (string.IsNullOrWhiteSpace(dni))
                {
                    CargarListadoSocios();
                    lblErrorBusqueda.Visible = false;
                    return;
                }

                //busca socio por DNI
                Socio socio = negocio.ObtenerSocioYPlanPorDni(dni);

                if (socio != null)
                {
                    //lista temporal para mostrar un solo socio
                    var lista = new List<Socio> { socio };

                    rptSocios.DataSource = lista;
                    rptSocios.DataBind();

                    lblErrorBusqueda.Visible = false;
                }
                else
                {
                    lblErrorBusqueda.Text = "No se encontró ningún socio con ese DNI.";
                    lblErrorBusqueda.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorBusqueda.Text = "Error al buscar socio: " + ex.Message;
                lblErrorBusqueda.Visible = true;
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