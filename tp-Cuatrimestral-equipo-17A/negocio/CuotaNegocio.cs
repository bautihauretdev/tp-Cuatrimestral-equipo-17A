using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class CuotaNegocio
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=EQUIPO17A_GYM_DB;Trusted_Connection=True;"; // Ajústalo

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
    }
}
