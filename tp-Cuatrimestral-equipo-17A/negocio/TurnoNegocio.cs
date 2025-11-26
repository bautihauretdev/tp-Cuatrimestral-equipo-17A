using dominio;
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
                // (La primera vez -BD vacía- va a hacer las 4 semanas y luego ya va a ir creando únicamente la 4ta)
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


        // Controla que exista la semana que queremos (le pasamos la fecha del lunes de esa semana)
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


        // Damos de alta los turnos de una semana completa (Lunes a Sábado, de 08:00 a 22:00)
        public void CrearSemana(DateTime lunes)
        {
            try
            {
                // Donde Lunes = 0 y Sábado = 5
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


        // Estamos listando los turnos de una semana en particular
        // Se usa para el repeater (en CargarCalendario() que va al Page_Load)
        public List<Turno> ObtenerTurnosSemana(DateTime lunes)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                DateTime domingo = lunes.AddDays(6);

                datos.setearConsulta("SELECT IdTurno, Fecha, CapacidadMaxima, Ocupados FROM TURNOS WHERE Fecha >= @lunes AND Fecha <= @domingo ORDER BY Fecha");
                datos.setearParametro("@lunes", lunes);
                datos.setearParametro("@domingo", domingo);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Turno
                    {
                        IdTurno = (int)datos.Lector["IdTurno"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        CapacidadMaxima = (int)datos.Lector["CapacidadMaxima"],
                        Ocupados = (int)datos.Lector["Ocupados"]
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener turnos de la semana: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        // Estamos listando todos los turnos existentes
        public List<Turno> ObtenerTodosTurnos()
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdTurno, Fecha, CapacidadMaxima, Ocupados FROM TURNOS ORDER BY Fecha");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Turno
                    {
                        IdTurno = (int)datos.Lector["IdTurno"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        CapacidadMaxima = (int)datos.Lector["CapacidadMaxima"],
                        Ocupados = (int)datos.Lector["Ocupados"]
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los turnos: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public static DateTime ObtenerPrimerLunesConTurnos()
        {
            // Trae la primera fecha de turnos y calcula su lunes
            TurnoNegocio negocio = new TurnoNegocio();
            var primerTurno = negocio.ObtenerTodosTurnos().OrderBy(t => t.Fecha).FirstOrDefault();
            if (primerTurno == null) return DateTime.Today;
            int dif = (int)primerTurno.Fecha.DayOfWeek - (int)DayOfWeek.Monday;
            if (dif < 0) dif += 7;
            return primerTurno.Fecha.AddDays(-dif).Date;
        }


        public static DateTime ObtenerUltimoLunesConTurnos()
        {
            // Trae la última fecha de turnos y calcula su lunes
            TurnoNegocio negocio = new TurnoNegocio();
            var ultimoTurno = negocio.ObtenerTodosTurnos().OrderByDescending(t => t.Fecha).FirstOrDefault();
            if (ultimoTurno == null) return DateTime.Today;
            int dif = (int)ultimoTurno.Fecha.DayOfWeek - (int)DayOfWeek.Monday;
            if (dif < 0) dif += 7;
            return ultimoTurno.Fecha.AddDays(-dif).Date;
        }

        //public void ActualizarTurnosPorRango(DateTime fechaDesde, DateTime fechaHasta, TimeSpan horaDesde, TimeSpan horaHasta, int nuevaCapacidad)
        //{
        //    AccesoDatos datos = new AccesoDatos();

        //    try
        //    {
        //        datos.setearConsulta(
        //            "UPDATE TURNOS " +
        //            "SET CapacidadMaxima = @Cap " +
        //            "WHERE Fecha >= @Desde AND Fecha <= @Hasta " +
        //            "AND CAST(Fecha AS time) >= @HoraDesde AND CAST(Fecha AS time) <= @HoraHasta"
        //        );

        //        datos.setearParametro("@Cap", nuevaCapacidad);
        //        datos.setearParametro("@Desde", fechaDesde);
        //        datos.setearParametro("@Hasta", fechaHasta.AddDays(1).AddSeconds(-1)); // Incluye TODO el día de "fecha hasta"
        //                                                                               // .AddDays(1).AddSeconds(-1) Es para sumarle 1 día y restarle 1 segundo
        //                                                                              //  y que quede la fecha que necesitamos a las 23:59:59
        //        datos.setearParametro("@HoraDesde", horaDesde);
        //        datos.setearParametro("@HoraHasta", horaHasta);

        //        datos.ejecutarAccion();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al actualizar turnos por rango: " + ex.Message);
        //    }
        //    finally
        //    {
        //        datos.cerrarConexion();
        //    }
        //}

        public void ActualizarTurnosPorRango(
            DateTime fechaDesde,
            DateTime fechaHasta,
            TimeSpan horaDesde,
            TimeSpan horaHasta,
            int nuevaCapacidad)
        {
            // Traer todos los turnos afectados
            List<Turno> turnosAfectados = new List<Turno>();
            AccesoDatos datosTurnos = new AccesoDatos();

            try
            {
                datosTurnos.setearConsulta(@"
                SELECT IdTurno, Fecha, CapacidadMaxima, Ocupados
                FROM TURNOS
                WHERE Fecha >= @Desde AND Fecha <= @Hasta
                  AND CAST(Fecha AS time) >= @HoraDesde
                  AND CAST(Fecha AS time) <= @HoraHasta
                ");

                datosTurnos.setearParametro("@Desde", fechaDesde);
                datosTurnos.setearParametro("@Hasta", fechaHasta.AddDays(1).AddSeconds(-1));// Incluye TODO el día de "fecha hasta"
                                                                                            // .AddDays(1).AddSeconds(-1) Es para sumarle 1 día y restarle 1 segundo
                                                                                            //  y que quede la fecha que necesitamos a las 23:59:59
                datosTurnos.setearParametro("@HoraDesde", horaDesde);
                datosTurnos.setearParametro("@HoraHasta", horaHasta);

                datosTurnos.ejecutarLectura();

                while (datosTurnos.Lector.Read())
                {
                    Turno t = new Turno();
                    t.IdTurno = (int)datosTurnos.Lector["IdTurno"];
                    t.Fecha = (DateTime)datosTurnos.Lector["Fecha"];
                    t.CapacidadMaxima = (int)datosTurnos.Lector["CapacidadMaxima"];
                    t.Ocupados = (int)datosTurnos.Lector["Ocupados"];
                    turnosAfectados.Add(t);
                }
            }
            finally
            {
                datosTurnos.cerrarConexion();
            }

            // Procesar cada turno
            foreach (Turno turno in turnosAfectados)
            {
                int capFinal = nuevaCapacidad < 0 ? 0 : nuevaCapacidad;

                // Obtener socios anotados en ese turno (del último al primero)
                List<int> sociosOrdenadosDesc = new List<int>();
                AccesoDatos datosSocios = new AccesoDatos();

                try
                {
                    // Ordenamos de forma descendiente según la fecha que se registraron los socios
                    datosSocios.setearConsulta(@"
                        SELECT IdSocio
                        FROM TURNOS_SOCIOS
                        WHERE IdTurno = @IdTurno
                        ORDER BY FechaRegistro DESC 
                    ");

                    datosSocios.setearParametro("@IdTurno", turno.IdTurno);
                    datosSocios.ejecutarLectura();

                    while (datosSocios.Lector.Read())
                    {
                        sociosOrdenadosDesc.Add((int)datosSocios.Lector["IdSocio"]);
                    }
                }
                finally
                {
                    datosSocios.cerrarConexion();
                }

                int cantidadSocios = sociosOrdenadosDesc.Count;

                // Calcula excedentes si la capacidad nueva es menor
                int excedentes = 0;
                if (capFinal < cantidadSocios)
                    excedentes = cantidadSocios - capFinal;

                // Si sobran socios, eliminar del último al primero y notificar
                if (excedentes > 0)
                {
                    for (int i = 0; i < excedentes && i < sociosOrdenadosDesc.Count; i++)
                    {
                        int idSocioAEliminar = sociosOrdenadosDesc[i];

                        // Borrar relación turno-socio
                        AccesoDatos datosDelete = new AccesoDatos();
                        try
                        {
                            datosDelete.setearConsulta(@"
                                DELETE FROM TURNOS_SOCIOS
                                WHERE IdTurno = @IdTurno AND IdSocio = @IdSocio
                            ");
                            datosDelete.setearParametro("@IdTurno", turno.IdTurno);
                            datosDelete.setearParametro("@IdSocio", idSocioAEliminar);
                            datosDelete.ejecutarAccion();
                        }
                        finally
                        {
                            datosDelete.cerrarConexion();
                        }

                        // Notificar cancelación a ese socio
                        EnviarNotificacionCancelacion(idSocioAEliminar, turno);
                    }
                }

                // Recalcular Ocupados = socios que quedaron (no excedentes)
                int nuevoOcupados = Math.Max(0, Math.Min(cantidadSocios - excedentes, capFinal));

                // Actualizar turno en la tabla TURNOS
                AccesoDatos datosUpdate = new AccesoDatos();
                try
                {
                    datosUpdate.setearConsulta(@"
                        UPDATE TURNOS
                        SET CapacidadMaxima = @CapacidadMaxima,
                            Ocupados = @Ocupados
                        WHERE IdTurno = @IdTurno
                            ");

                    datosUpdate.setearParametro("@CapacidadMaxima", capFinal);
                    datosUpdate.setearParametro("@Ocupados", nuevoOcupados);
                    datosUpdate.setearParametro("@IdTurno", turno.IdTurno);

                    datosUpdate.ejecutarAccion();
                }
                finally
                {
                    datosUpdate.cerrarConexion();
                }
            }
        }

        private void EnviarNotificacionCancelacion(int idSocio, Turno turno)
        {
            string mensaje = $"Tu turno del {turno.Fecha:dd/MM HH:mm} fue cancelado por cambio de capacidad.";

            NotificacionNegocio notifNegocio = new NotificacionNegocio();
            notifNegocio.AgregarNotificacion(idSocio, mensaje);
        }


        // Se usa desde SocioTurnos
        public void ActualizarOcupados(int idTurno, int nuevosOcupados)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE TURNOS SET Ocupados = @Ocupados WHERE IdTurno = @Id");
                datos.setearParametro("@Ocupados", nuevosOcupados);
                datos.setearParametro("@Id", idTurno);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
