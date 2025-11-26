using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// (!) COMENTARIO DE NATI
// Dejo "HoraCalendario" y "TurnoCalendario" provisorio acá, después hago una clase auxiliar aparte para agregarlos
public class HoraCalendario
{
    public string HoraTexto { get; set; }
    public List<TurnoCalendario> Turnos { get; set; }
}

public class TurnoCalendario
{
    public int IdTurno { get; set; }
    public string EstadoTexto { get; set; }
    public string FechaHoraTexto { get; set; }
    public bool ReservadoPorSocio { get; set; }
}
// (!) LEER ARRIBA COMENTARIO DE NATI


namespace presentacionWebForm
{
    public partial class AdminTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Todos los lunes controla que la semana actual y las tres siguientes estén creados los turnos
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                TurnoNegocio negocio = new TurnoNegocio();
                negocio.AsegurarSemanas();
            }

            if (!IsPostBack)
            {
                ViewState["lunesActual"] = ObtenerLunes(DateTime.Today);
                CargarCalendario();
            }

            CargarMetricas();
        }


        private void CargarMetricas()
        {
            // SOCIOS ACTIVOS
            AccesoDatos datosActivos = new AccesoDatos();
            int sociosActivos = 0;

            try
            {
                datosActivos.setearConsulta(@"
                    SELECT COUNT(IdSocio)
                    FROM SOCIOS
                    WHERE Activo = 1
                ");
                sociosActivos = Convert.ToInt32(datosActivos.ejecutarScalar());
            }
            finally
            {
                datosActivos.cerrarConexion();
            }

            lblSociosActivosNum.Text = sociosActivos.ToString();

            // SOCIOS QUE VIENEN HOY
            AccesoDatos datosHoy = new AccesoDatos();
            int sociosHoy = 0;

            try
            {
                datosHoy.setearConsulta(@"
                    SELECT COUNT(DISTINCT TS.IdSocio)
                    FROM TURNOS_SOCIOS TS
                    INNER JOIN TURNOS T ON T.IdTurno = TS.IdTurno
                    WHERE CONVERT(date, T.Fecha) = CONVERT(date, GETDATE())
                ");
                sociosHoy = Convert.ToInt32(datosHoy.ejecutarScalar());
            }
            finally
            {
                datosHoy.cerrarConexion();
            }

            lblSociosHoyNum.Text = sociosHoy.ToString();


            // HORARIOS COMPLETOS
            AccesoDatos datosCompletos = new AccesoDatos();
            int horariosCompletos = 0;

            try
            {
                datosCompletos.setearConsulta(@"
                    SELECT COUNT(IdTurno)
                    FROM TURNOS
                    WHERE CapacidadMaxima > 0
                      AND Ocupados >= CapacidadMaxima
                ");
                horariosCompletos = Convert.ToInt32(datosCompletos.ejecutarScalar());
            }
            finally
            {
                datosCompletos.cerrarConexion();
            }

            lblHorariosCompletosNum.Text = horariosCompletos.ToString();
        }


        private DateTime ObtenerLunes(DateTime fecha)
        {
            int dif = (int)fecha.DayOfWeek - (int)DayOfWeek.Monday;
            if (dif < 0) dif += 7;
            return fecha.AddDays(-dif);
        }


        private void CargarCalendario()
        {
            TurnoNegocio negocio = new TurnoNegocio();

            // Obtenemos el lunes actual
            DateTime lunes = (DateTime)ViewState["lunesActual"];

            // Mostramos "fecha lunes / domingo de la semana" arriba del calendario
            lblCalendarioRango.Text = lunes.ToString("dd MMM") + " - " + lunes.AddDays(6).ToString("dd MMM");

            // Actualizar los labels de los días con "Nombre + número"
            lblLunes.Text = "Lunes " + lunes.Day;
            lblMartes.Text = "Martes " + lunes.AddDays(1).Day;
            lblMiercoles.Text = "Miércoles " + lunes.AddDays(2).Day;
            lblJueves.Text = "Jueves " + lunes.AddDays(3).Day;
            lblViernes.Text = "Viernes " + lunes.AddDays(4).Day;
            lblSabado.Text = "Sábado " + lunes.AddDays(5).Day;
            lblDomingo.Text = "Domingo " + lunes.AddDays(6).Day;

            // Traemos los turnos desde negocio
            List<Turno> lista = negocio.ObtenerTurnosSemana(lunes);

            // Estructura para el Repeater (08:00 a 22:00)
            List<HoraCalendario> horas = new List<HoraCalendario>();

            // Horas
            for (int h = 8; h <= 22; h++)
            {
                HoraCalendario hora = new HoraCalendario();
                hora.HoraTexto = h.ToString("00") + ":00";
                hora.Turnos = new List<TurnoCalendario>();

                // Días
                for (int d = 0; d < 7; d++)
                {
                    DateTime fechaBuscada = lunes.AddDays(d).Date.AddHours(h);

                    Turno turno = lista.FirstOrDefault(x =>
                        x.Fecha.Year == fechaBuscada.Year &&
                        x.Fecha.Month == fechaBuscada.Month &&
                        x.Fecha.Day == fechaBuscada.Day &&
                        x.Fecha.Hour == fechaBuscada.Hour
                    );

                    if (turno != null)
                    {
                        // Una vez creado el turno, el admin lo pone en 0 para "cerrar" el Gym
                        if (turno.CapacidadMaxima == 0)
                        {
                            hora.Turnos.Add(new TurnoCalendario
                            {
                                IdTurno = turno.IdTurno,
                                EstadoTexto = "No disponible"
                            });
                        }
                        else
                        {
                            hora.Turnos.Add(new TurnoCalendario
                            {
                                IdTurno = turno.IdTurno,
                                EstadoTexto = turno.Ocupados.ToString() + "/" + turno.CapacidadMaxima.ToString()
                            });
                        }
                    }
                    else
                    {
                        hora.Turnos.Add(new TurnoCalendario
                        {
                            IdTurno = 0,
                            EstadoTexto = "No disponible"
                        });
                    }
                }

                horas.Add(hora);
            }

            rptHoras.DataSource = horas;
            rptHoras.DataBind();

            // Bloqueamos las flechas si no hay semanas previas o posteriores
            DateTime primerLunes = TurnoNegocio.ObtenerPrimerLunesConTurnos();
            DateTime ultimoLunes = TurnoNegocio.ObtenerUltimoLunesConTurnos();

            btnSemanaAnterior.Enabled = ((DateTime)ViewState["lunesActual"]) > primerLunes;
            btnSemanaSiguiente.Enabled = ((DateTime)ViewState["lunesActual"]) < ultimoLunes;
        }


        protected void btnTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            // Puede venir 0 cuando es "No disponible"
            if (string.IsNullOrEmpty(btn.CommandArgument) || btn.CommandArgument == "0")
                return;

            int idTurno = int.Parse(btn.CommandArgument);

            CargarDetalleTurno(idTurno);

            // Abrir modal de detalle de socios
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "ShowDetalleTurno",
                "var m = new bootstrap.Modal(document.getElementById('modalDetalleTurno')); m.show();",
                true
            );
        }

        // Acá mostramos los socios que pidieron el turno
        private void CargarDetalleTurno(int idTurno)
        {
            // Título del modal con fecha/hora del turno
            TurnoNegocio negocio = new TurnoNegocio();
            Turno turno = negocio.ObtenerTodosTurnos().FirstOrDefault(t => t.IdTurno == idTurno);

            if (turno != null)
            {
                lblDetalleTurnoTitulo.Text = "Socios del turno " + turno.Fecha.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                lblDetalleTurnoTitulo.Text = "Socios del turno";
            }

            // Traer los socios anotados a ese turno
            List<Socio> socios = new List<Socio>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT S.IdSocio, S.Nombre, S.Apellido, S.Dni, S.Email
                    FROM TURNOS_SOCIOS TS
                    INNER JOIN SOCIOS S ON S.IdSocio = TS.IdSocio
                    WHERE TS.IdTurno = @IdTurno
                    ORDER BY S.Apellido, S.Nombre
                ");

                datos.setearParametro("@IdTurno", idTurno);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    socios.Add(new Socio
                    {
                        IdSocio = (int)datos.Lector["IdSocio"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["Dni"].ToString(),
                        Email = datos.Lector["Email"].ToString()
                    });
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            if (socios.Count > 0)
            {
                rptSociosTurno.DataSource = socios;
                rptSociosTurno.DataBind();
                lblSinSociosTurno.Visible = false;
            }
            else
            {
                rptSociosTurno.DataSource = null;
                rptSociosTurno.DataBind();
                lblSinSociosTurno.Visible = true;
            }
        }


        protected void btnSemanaAnterior_Click(object sender, EventArgs e)
        {
            DateTime lunes = (DateTime)ViewState["lunesActual"];
            lunes = lunes.AddDays(-7);
            ViewState["lunesActual"] = lunes;
            CargarCalendario();
        }


        protected void btnSemanaSiguiente_Click(object sender, EventArgs e)
        {
            DateTime lunes = (DateTime)ViewState["lunesActual"];
            lunes = lunes.AddDays(7);
            ViewState["lunesActual"] = lunes;
            CargarCalendario();
        }

        // Cuando queremos guardar los cambios dentro de Editar Turno
        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            // Oculta la label del error 
            lblErrorEditarTurno.Visible = false;
            lblErrorEditarTurno.Text = "";

            try
            {
                DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
                DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);

                TimeSpan horaDesde = TimeSpan.Parse(txtHoraDesde.Text);
                TimeSpan horaHasta = TimeSpan.Parse(txtHoraHasta.Text);

                // Valida que todos los campos tienen que tener valor
                if (string.IsNullOrWhiteSpace(txtFechaDesde.Text) ||
                    string.IsNullOrWhiteSpace(txtFechaHasta.Text) ||
                    string.IsNullOrWhiteSpace(txtHoraDesde.Text) ||
                    string.IsNullOrWhiteSpace(txtHoraHasta.Text) ||
                    string.IsNullOrWhiteSpace(txtCapacidad.Text))
                {
                    lblErrorEditarTurno.Text = "Todos los campos son obligatorios.";
                    lblErrorEditarTurno.Visible = true;
                    return; // Evita seguir y NO entra en el try
                }

                // Validar que fechaDesde sea menor o igual que fechaHasta
                if (fechaDesde > fechaHasta)
                {
                    lblErrorEditarTurno.Text = "La 'Fecha Desde' debe ser menor o igual que la 'Fecha Hasta'.";
                    lblErrorEditarTurno.Visible = true;
                    AbrirModalConError();
                    return;
                }

                // Validar que las horas sean exactas "en punto"
                if (horaDesde.Minutes != 0 || horaHasta.Minutes != 0)
                {
                    lblErrorEditarTurno.Text = "Las horas deben ser exactas en punto (ej: 08:00).";
                    lblErrorEditarTurno.Visible = true;
                    AbrirModalConError();
                    return;
                }

                // Validar rango permitido 08:00 a 22:00
                if (horaDesde.Hours < 8 || horaDesde.Hours > 22 ||
                    horaHasta.Hours < 8 || horaHasta.Hours > 22)
                {
                    lblErrorEditarTurno.Text = "Las horas deben estar entre 08:00 y 22:00.";
                    lblErrorEditarTurno.Visible = true;
                    AbrirModalConError();
                    return;
                }

                // Validar que horaDesde sea menor que horaHasta
                if (horaDesde > horaHasta)
                {
                    lblErrorEditarTurno.Text = "La 'Hora Desde' debe ser menor o igual que la 'Hora hasta'.";
                    lblErrorEditarTurno.Visible = true;
                    AbrirModalConError();
                    return;
                }

                // Validar CAPACIDAD que sea número
                int capacidad;
                if (!int.TryParse(txtCapacidad.Text, out capacidad))
                {
                    lblErrorEditarTurno.Text = "La capacidad debe ser un número.";
                    lblErrorEditarTurno.Visible = true;
                    AbrirModalConError();
                    return;
                }

                // Ejecutar cambio
                TurnoNegocio negocio = new TurnoNegocio();
                negocio.ActualizarTurnosPorRango(fechaDesde, fechaHasta, horaDesde, horaHasta, capacidad);

                // Recargar el calendario para que refleje los cambios
                CargarCalendario();
            }
            catch (Exception ex)
            {
                lblErrorEditarTurno.Text = "Error al guardar cambios: " + ex.Message;
                lblErrorEditarTurno.Visible = true;
                AbrirModalConError();
            }
        }

        // Método para abrir el modal sin limpiar campos al haber error
        private void AbrirModalConError()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModalEditarTurno",
                "var modal = new bootstrap.Modal(document.getElementById('modalEditarTurno')); modal.show();", true);
        }
    }
}