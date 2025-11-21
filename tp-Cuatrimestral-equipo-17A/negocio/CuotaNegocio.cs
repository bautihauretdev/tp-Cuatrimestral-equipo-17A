using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class CuotaNegocio
    {
        public List<Socio> ListarSocios()
        {
            var lista = new List<Socio>();
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta(@"SELECT IdSocio, Nombre, Apellido FROM SOCIOS WHERE Activo = 1");
                var dr = acceso.ejecutarLectura();
                while (dr.Read())
                {
                    Socio socio = new Socio
                    {
                        IdSocio = (int)dr["IdSocio"],
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString()
                    };
                    lista.Add(socio);
                }
            }
            finally
            {
                acceso.cerrarConexion();
            }

            return lista;
        }

        public Cuota ObtenerCuotaActual(int idSocio)
        {
            Cuota cuota = null;
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta(@"SELECT IdCuota, IdPago, Anio, Mes, Monto, Recargo, Estado 
                                        FROM CUOTAS 
                                        WHERE IdSocio = @idSocio AND Mes = @mes AND Anio = @anio");
                acceso.setearParametro("@idSocio", idSocio);
                acceso.setearParametro("@mes", DateTime.Now.Month);
                acceso.setearParametro("@anio", DateTime.Now.Year);

                var dr = acceso.ejecutarLectura();
                if (dr.Read())
                {
                    cuota = new Cuota
                    {
                        IdCuota = (int)dr["IdCuota"],
                        IdPago = dr["IdPago"] != DBNull.Value ? (int?)dr["IdPago"] : null,
                        Anio = (int)dr["Anio"],
                        Mes = (int)dr["Mes"],
                        Monto = (decimal)dr["Monto"],
                        Recargo = (decimal)dr["Recargo"],
                        Estado = dr["Estado"].ToString()
                    };
                }
            }
            finally
            {
                acceso.cerrarConexion();
            }

            return cuota;
        }

        public void ActualizarEstadoCuota(int idCuota, string nuevoEstado)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta(@"UPDATE CUOTAS SET Estado = @estado WHERE IdCuota = @idCuota");
                acceso.setearParametro("@estado", nuevoEstado);
                acceso.setearParametro("@idCuota", idCuota);

                acceso.ejecutarAccion();
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }

        public List<Cuota> ObtenerCuotasPagadas()
        {
            var lista = new List<Cuota>();
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta(@"
                    SELECT c.IdCuota, c.Anio, c.Mes, c.Monto, c.Recargo, c.Estado,
                           s.IdSocio, s.Nombre, s.Apellido
                    FROM CUOTAS c
                    INNER JOIN SOCIOS s ON c.IdSocio = s.IdSocio
                    WHERE c.Estado = 'Pagado'
                    ORDER BY c.Anio DESC, c.Mes DESC");

                var dr = acceso.ejecutarLectura();
                while (dr.Read())
                {
                    Cuota cuota = new Cuota
                    {
                        IdCuota = (int)dr["IdCuota"],
                        Anio = (int)dr["Anio"],
                        Mes = (int)dr["Mes"],
                        Monto = (decimal)dr["Monto"],
                        Recargo = (decimal)dr["Recargo"],
                        Estado = dr["Estado"].ToString(),
                        Socio = new Socio
                        {
                            IdSocio = (int)dr["IdSocio"],
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString()
                        }
                    };
                    lista.Add(cuota);
                }
            }
            finally
            {
                acceso.cerrarConexion();
            }

            return lista;
        }
    }
}