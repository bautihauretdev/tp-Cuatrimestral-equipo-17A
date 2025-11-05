using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class CuotaNegocio
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=EQUIPO17A_GYM_DB;Trusted_Connection=True;";

        public List<Socio> ListarSocios()
        {
            var lista = new List<Socio>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT IdSocio, Nombre, Apellido FROM SOCIOS WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
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
            }
            return lista;
        }

        public Cuota ObtenerCuotaActual(int idSocio)
        {
            Cuota cuota = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT IdCuota, IdPago, Anio, Mes, Monto, Recargo, Estado 
                                 FROM CUOTAS 
                                 WHERE IdSocio = @idSocio AND Mes = @mes AND Anio = @anio";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idSocio", idSocio);
                    cmd.Parameters.AddWithValue("@mes", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@anio", DateTime.Now.Year);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
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
            }
            return cuota;
        }

        public void ActualizarEstadoCuota(int idCuota, string nuevoEstado)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE CUOTAS SET Estado = @estado WHERE IdCuota = @idCuota";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@idCuota", idCuota);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Cuota> ObtenerCuotasPagadas()
        {
            var lista = new List<Cuota>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT c.IdCuota, c.Anio, c.Mes, c.Monto, c.Recargo, c.Estado,
                                s.IdSocio, s.Nombre, s.Apellido
                         FROM CUOTAS c
                         INNER JOIN SOCIOS s ON c.IdSocio = s.IdSocio
                         WHERE c.Estado = 'Pagado'
                         ORDER BY c.Anio DESC, c.Mes DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
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
            }

            return lista;
        }
    }
}