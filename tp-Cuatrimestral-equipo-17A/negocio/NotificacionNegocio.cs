using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class NotificacionNegocio
    {
        public void AgregarNotificacion(int idSocio, string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO NOTIFICACIONES (IdSocio, Mensaje, FechaEnvio, Leido)
                    VALUES (@IdSocio, @Mensaje, @FechaEnvio, 0)
                ");
                datos.setearParametro("@IdSocio", idSocio);
                datos.setearParametro("@Mensaje", mensaje);
                datos.setearParametro("@FechaEnvio", DateTime.Now);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Notificacion> ObtenerNoLeidasPorSocio(int idSocio)
        {
            List<Notificacion> lista = new List<Notificacion>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT IdNotificacion, IdSocio, Mensaje, FechaEnvio, Leido
                    FROM NOTIFICACIONES
                    WHERE IdSocio = @IdSocio AND Leido = 0
                    ORDER BY FechaEnvio DESC
                ");
                datos.setearParametro("@IdSocio", idSocio);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Notificacion
                    {
                        IdNotificacion = (int)datos.Lector["IdNotificacion"],
                        Socio = new Socio { IdSocio = (int)datos.Lector["IdSocio"] },
                        Mensaje = (string)datos.Lector["Mensaje"],
                        FechaEnvio = (DateTime)datos.Lector["FechaEnvio"],
                        Leido = (bool)datos.Lector["Leido"]
                    });
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void MarcarComoLeidas(int idSocio)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    UPDATE NOTIFICACIONES
                    SET Leido = 1
                    WHERE IdSocio = @IdSocio AND Leido = 0
                ");
                datos.setearParametro("@IdSocio", idSocio);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
