using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class TurnoNegocio
    {

        public void AsegurarSemanas()
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                DateTime hoy = DateTime.Today;

                // Obtener lunes de la semana actual
                int dif = (int)hoy.DayOfWeek - (int)DayOfWeek.Monday;
                if (dif < 0) dif += 7;
                DateTime lunes = hoy.AddDays(-dif);

                DateTime domingo = lunes.AddDays(6);

                // Validamos si la semana ya existe
                datos.setearConsulta(
                    "SELECT COUNT(IdTurno) FROM TURNOS WHERE Fecha >= @lunes AND Fecha <= @domingo"
                );
                datos.setearParametro("@lunes", lunes);
                datos.setearParametro("@domingo", domingo);

                int cantidad = Convert.ToInt32(datos.ejecutarScalar());
                datos.cerrarConexion();

                if (cantidad == 0)
                {
                    // Si no existe la semana, la crea
                    CrearSemana(lunes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en AsegurarSemanas: " + ex.Message);
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
