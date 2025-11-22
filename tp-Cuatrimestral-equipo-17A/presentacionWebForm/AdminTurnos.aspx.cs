using dominio;
using negocio;
using System;
using System.Collections.Generic;
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
                CargarCalendario();
            }
        }


        private void CargarCalendario()
        {
            TurnoNegocio negocio = new TurnoNegocio();

            // Obtenemos el lunes actual
            DateTime hoy = DateTime.Today;
            int dif = (int)hoy.DayOfWeek - (int)DayOfWeek.Monday;
            if (dif < 0) dif += 7;
            DateTime lunes = hoy.AddDays(-dif);

            // Mostramos "fecha lunes / domingo de la semana" arriba del calendario
            lblCalendarioRango.Text = lunes.ToString("dd MMM") + " - " + lunes.AddDays(6).ToString("dd MMM");

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
                        hora.Turnos.Add(new TurnoCalendario
                        {
                            IdTurno = turno.IdTurno,
                            EstadoTexto = turno.Ocupados.ToString() + "/" + turno.CapacidadMaxima.ToString()
                        });
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
        }


        protected void btnTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idTurno = int.Parse(btn.CommandArgument);
        }
    }
}