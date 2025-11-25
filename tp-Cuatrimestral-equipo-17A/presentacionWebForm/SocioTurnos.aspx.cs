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
    public partial class SocioTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["lunesActual"] = ObtenerLunes(DateTime.Today);
                CargarCalendario();
            }
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

                    Turno turno = lista.FirstOrDefault(x => x.Fecha == fechaBuscada);

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
    }
}