using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
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
                CargarPlanes();
                // Primero: se comprueba si hay edicion por Session 
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
        private void CargarPlanes() 
        {
            try
            {
                PlanNegocio planNegocio = new PlanNegocio(); 
                List<Plan> planes = planNegocio.ListarPlanes(); 
                ddlPlanSocio.DataSource = planes; 
                ddlPlanSocio.DataTextField = "Nombre"; 
                ddlPlanSocio.DataValueField = "IdPlan"; 
                ddlPlanSocio.DataBind(); 
            }
            catch (Exception ex)
            {
                lblErrorAltaSocio.Text = "Error cargando planes: " + ex.Message; 
                lblErrorAltaSocio.Visible = true; 
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
                // AGREGADO: Selecciona el plan actual en el dropdown
                if (ddlPlanSocio.Items.FindByValue(socio.IdPlan.ToString()) != null) 
                    ddlPlanSocio.SelectedValue = socio.IdPlan.ToString();

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
                SocioNegocio socioNegocio = new SocioNegocio();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

                // VALIDACIÓN 1: DNI duplicado
                string dniIngresado = txtDniAltaSocio.Text.Trim(); 
                Socio socioExistente = socioNegocio.ObtenerPorDni(dniIngresado); 

                if (socioExistente != null)
                {
                    if (socioExistente.Activo)
                    {
                        lblErrorAltaSocio.Text = "Ya existe un socio activo con ese DNI."; 
                        lblErrorAltaSocio.Visible = true; 
                        lblMensajeAltaSocio.Visible = false; 
                        return;
                    }
                    else
                    {
                        // Guardamos el ID en un HiddenField
                        hfIdSocioInactivo.Value = socioExistente.IdSocio.ToString();

                        // Abrimos el modal desde el servidor
                        ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModalReactivar();", true);

                        return;
                    }
                }
                // VALIDACIÓN 2: Email duplicado en SOCIOS
                string emailIngresado = txtEmailAltaSocio.Text.Trim();
                if (socioNegocio.ObtenerPorEmail(emailIngresado) != null) 
                {
                    lblErrorAltaSocio.Text = "Ya existe un socio con ese email.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 2b: Email duplicado en USUARIOS
                if (usuarioNegocio.EmailEnUsoPorOtro(emailIngresado, 0)) //pasa 0 porque es alta nueva
                {
                    lblErrorAltaSocio.Text = "Ya existe un usuario con ese email.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 3: Formato de email
                if (!Regex.IsMatch(emailIngresado, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) 
                {
                    lblErrorAltaSocio.Text = "El formato del email no es válido.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 4: Fecha de nacimiento
                DateTime fechaNac;
                if (!DateTime.TryParse(txtFechaNacAltaSocio.Text, out fechaNac)) 
                {
                    lblErrorAltaSocio.Text = "La fecha de nacimiento no es válida.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }
                if (fechaNac > DateTime.Now)
                {
                    lblErrorAltaSocio.Text = "La fecha de nacimiento no puede ser futura.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }
                int edad = DateTime.Now.Year - fechaNac.Year;
                if (fechaNac > DateTime.Now.AddYears(-edad)) edad--; //corrige si no cumplio años este año
                if (edad < 14)
                {
                    lblErrorAltaSocio.Text = "La edad mínima para ser socio es 14 años.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }
                if (edad > 90)
                {
                    lblErrorAltaSocio.Text = "La edad máxima permitida es 90 años.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 5: Telefono
                string telefono = txtTelefonoAltaSocio.Text.Trim();
                if (!Regex.IsMatch(telefono, @"^\d{8,15}$")) // solo numeros y de 8 a 15 digitos 
                {
                    lblErrorAltaSocio.Text = "El teléfono debe ser solo números y entre 8 y 15 dígitos.";
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 6: Campos Obligatorios
                if (string.IsNullOrWhiteSpace(txtNombreAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(txtApellidoAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(txtDniAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(txtFechaNacAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(txtTelefonoAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(txtEmailAltaSocio.Text) ||
                       string.IsNullOrWhiteSpace(ddlPlanSocio.SelectedValue))
                {
                    lblErrorAltaSocio.Text = "Todos los campos son obligatorios."; 
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                // VALIDACIÓN 7: Nombre
                string nombre = txtNombreAltaSocio.Text.Trim();
                if (nombre.Length < 2 || !Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ ]+$")) // letras + espacios
                {
                    lblErrorAltaSocio.Text = "Nombre inválido. Debe ser solo letras, mínimo 2 caracteres."; 
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                //VALIDACIÓN 8: Apellido
                string apellido = txtApellidoAltaSocio.Text.Trim();
                if (apellido.Length < 2 || !Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ ]+$"))
                {
                    lblErrorAltaSocio.Text = "Apellido inválido. Debe ser solo letras, mínimo 2 caracteres."; 
                    lblErrorAltaSocio.Visible = true;
                    lblMensajeAltaSocio.Visible = false;
                    return;
                }

                //si no existe flujo normal de alta
                Socio nuevo = new Socio
                {
                    Nombre = txtNombreAltaSocio.Text,
                    Apellido = txtApellidoAltaSocio.Text,
                    Dni = dniIngresado,
                    FechaNacimiento = DateTime.Parse(txtFechaNacAltaSocio.Text),
                    Telefono = txtTelefonoAltaSocio.Text,
                    Email = txtEmailAltaSocio.Text,
                    IdPlan = int.Parse(ddlPlanSocio.SelectedValue),
                    Activo = true
                };

                int idSocio = socioNegocio.Agregar(nuevo);
                usuarioNegocio.AgregarUsuarioSocio(nuevo.Email, nuevo.Dni, idSocio);

                lblMensajeAltaSocio.Text = "Socio y usuario creados correctamente. Contraseña por defecto: DNI";
                lblMensajeAltaSocio.Visible = true;
                lblErrorAltaSocio.Visible = false;
                Response.Redirect("AdminSocios.aspx");
            }
            catch (Exception ex)
            {
                lblErrorAltaSocio.Text = "Error: " + ex.Message;
                lblErrorAltaSocio.Visible = true;
                lblMensajeAltaSocio.Visible = false;
            }
        }

        protected void btnConfirmarReactivar_Click(object sender, EventArgs e)
        {
            try
            {
                int idSocio = int.Parse(hfIdSocioInactivo.Value);

                SocioNegocio socioNegocio = new SocioNegocio();
                socioNegocio.AltaLogica(idSocio);

                // Mensaje de éxito
                Session["MensajeSocio"] = "Socio reactivado correctamente.";
                Response.Redirect("AdminSocios.aspx");
            }
            catch (Exception ex)
            {
                lblErrorReactivar.Text = "Error al reactivar: " + ex.Message;
                lblErrorReactivar.Visible = true;

                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModalReactivar();", true);
            }
        }


        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                //VALIDACION 1: EDITAR
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
                    IdPlan = int.Parse(ddlPlanSocio.SelectedValue)
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

                //VALIDACION 2: PREGUNTA SI EL SOCIO ORIGINAL EXISTE
                if (socioOriginal == null)
                {
                    lblErrorAltaSocio.Text = "No se encontró el socio a modificar.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                //VALIDACION 3: NO PERMITE EDICION SI EL SOCIO NO ESTA ACTIVO
                if (!socioOriginal.Activo)
                {
                    lblErrorAltaSocio.Text = "El socio está dado de baja. Reactívelo antes de modificar sus datos.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                // VALIDACION 4.1: LEE LOS CAMPOS DEL FORMULARIO
                string nombre = txtNombreAltaSocio.Text?.Trim();
                string apellido = txtApellidoAltaSocio.Text?.Trim();
                string dni = txtDniAltaSocio.Text?.Trim();   //aunque sea readonly 
                string fechaTxt = txtFechaNacAltaSocio.Text?.Trim();
                string telefono = txtTelefonoAltaSocio.Text?.Trim();
                string email = txtEmailAltaSocio.Text?.Trim();
                string planSelected = ddlPlanSocio.SelectedValue;

                //VALIDACION 4.2: VALIDA CAMPOS OBLIGATORIOS NO VACIOS
                if (string.IsNullOrEmpty(nombre) ||
                    string.IsNullOrEmpty(apellido) ||
                    string.IsNullOrEmpty(dni) ||
                    string.IsNullOrEmpty(fechaTxt) ||
                    string.IsNullOrEmpty(telefono) ||
                    string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(planSelected))
                {
                    lblErrorAltaSocio.Text = "Complete todos los campos obligatorios.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }
                
                //VALIDACION 5: FORMATO DE MAIL
                try
                {
                    var mail = new MailAddress(email);
                    if (email.Contains(" "))
                    {
                        throw new FormatException();
                    }
                }
                catch
                {
                    lblErrorAltaSocio.Text = "El email ingresado no tiene un formato válido.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                //VALIDACION 6: PERMITE + Y LONGITUD MINIMA 8 CARAC.
                string telNormalizado = telefono.Replace(" ", "").Replace("-", "");
                if (!Regex.IsMatch(telNormalizado, @"^\+?\d{8,15}$"))
                {
                    lblErrorAltaSocio.Text = "El teléfono ingresado no es válido. Use solo números (y opcionalmente +), mínimo 8 dígitos.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                //VALIDACION 7: FECHA DE NACIMIENTO
                if (!DateTime.TryParse(fechaTxt, out DateTime fechaNacimiento))
                {
                    lblErrorAltaSocio.Text = "La fecha de nacimiento no tiene un formato válido.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }
                if (fechaNacimiento > DateTime.Today)
                {
                    lblErrorAltaSocio.Text = "La fecha de nacimiento no puede ser futura.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }
                int edad = DateTime.Today.Year - fechaNacimiento.Year;
                if (fechaNacimiento > DateTime.Today.AddYears(-edad)) edad--;
                if (edad < 14)
                {
                    lblErrorAltaSocio.Text = "El socio debe ser mayor de 13 años.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }
                if (edad > 100)
                {
                    lblErrorAltaSocio.Text = "La fecha de nacimiento parece incorrecta.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

               

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

                //VALIDACION 8: EMAIL DUPLICADO EN SOCIOS
                Socio socioPorEmail = socioNegocio.ObtenerPorEmail(email);
                if (socioPorEmail != null && socioPorEmail.IdSocio != idSocio)
                {
                    lblErrorAltaSocio.Text = "El email ingresado ya está registrado para otro socio.";
                    lblErrorAltaSocio.Visible = true;
                    return;
                }

                //VALIDACION 9: NO GUARDA SI NO HAY CAMBIOS REALES
                bool huboCambios =
                       !string.Equals(socio.Nombre ?? "", socioOriginal.Nombre ?? "", StringComparison.Ordinal)
                    || !string.Equals(socio.Apellido ?? "", socioOriginal.Apellido ?? "", StringComparison.Ordinal)
                    || !string.Equals(socio.Email ?? "", socioOriginal.Email ?? "", StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(socio.Telefono ?? "", socioOriginal.Telefono ?? "", StringComparison.Ordinal)
                    || socio.IdPlan != socioOriginal.IdPlan
                    || socio.FechaNacimiento != socioOriginal.FechaNacimiento;
                if (!huboCambios)
                {
                    lblErrorAltaSocio.Text = "No se detectaron cambios para guardar.";
                    lblErrorAltaSocio.Visible = true;
                    return;
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

                //Redirige de vuelta a AdminSocios.aspx
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