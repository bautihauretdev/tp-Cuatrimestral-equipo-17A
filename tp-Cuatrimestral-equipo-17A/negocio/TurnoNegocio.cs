using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class TurnoNegocio
    {

        // Controla que estén creados los turnos de la semana en vigencia y las 3 siguientes
        public void AsegurarSemanas()
        {
            try
            {
                DateTime hoy = DateTime.Today;

                // Obtener lunes de la semana actual
                int dif = (int)hoy.DayOfWeek - (int)DayOfWeek.Monday;
                if (dif < 0) dif += 7;
                DateTime lunesActual = hoy.AddDays(-dif);

                // Controla la existencia de la semana y de ser necesario la crea
                // La primera vez va a hacer las 4 semanas y luego ya va a ir creando únicamente la 4ta
                for (int i = 0; i < 4; i++)
                {
                    DateTime lunesSemana = lunesActual.AddDays(i * 7);

                    if (!ExisteSemana(lunesSemana))
                        CrearSemana(lunesSemana);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en AsegurarSemanas: " + ex.Message);
            }
        }

        private bool ExisteSemana(DateTime lunes)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                DateTime domingo = lunes.AddDays(6);

                datos.setearConsulta(
                    "SELECT COUNT(IdTurno) FROM TURNOS WHERE Fecha >= @lunes AND Fecha <= @domingo"
                );
                datos.setearParametro("@lunes", lunes);
                datos.setearParametro("@domingo", domingo);

                int cantidad = Convert.ToInt32(datos.ejecutarScalar());
                datos.cerrarConexion();

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ExisteSemana: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void CrearSemana(DateTime lunes)
        {
            // Creamos los turnos de una semana completa (Lunes a Sábado, de 08:00 a 22:00)
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    DateTime fechaDia = lunes.AddDays(i);

                    // Horarios desde 08:00 hasta 22:00 (porque el último turno inicia 22:00)
                    for (int hora = 8; hora <= 22; hora++)
                    {
                        DateTime fechaTurno = new DateTime(
                            fechaDia.Year,
                            fechaDia.Month,
                            fechaDia.Day,
                            hora,
                            0,
                            0
                        );

                        AccesoDatos datos = new AccesoDatos();

                        datos.setearConsulta("INSERT INTO TURNOS (Fecha, CapacidadMaxima, Ocupados) VALUES (@Fecha, @CapMax, 0)");

                        datos.setearParametro("@Fecha", fechaTurno);
                        datos.setearParametro("@CapMax", 30); // Es la cantidad máxima de gente que puede entrar

                        datos.ejecutarAccion();
                        datos.cerrarConexion();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en CrearSemana: " + ex.Message);
            }
        }
    }
}
