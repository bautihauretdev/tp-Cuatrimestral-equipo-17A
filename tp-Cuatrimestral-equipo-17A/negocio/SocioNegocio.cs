using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class SocioNegocio
    {
        
        public Socio ObtenerPorDni(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            Socio socio = null;
            try
            {
                datos.setearConsulta("SELECT IdSocio, Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, IdPlan, Activo FROM SOCIOS WHERE Dni = @Dni");
                datos.setearParametro("@Dni", dni);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    socio = new Socio
                    {
                        IdSocio = (int)datos.Lector["IdSocio"],
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? (string)datos.Lector["Nombre"] : "",
                        Apellido = datos.Lector["Apellido"] != DBNull.Value ? (string)datos.Lector["Apellido"] : "",
                        Dni = datos.Lector["Dni"] != DBNull.Value ? (string)datos.Lector["Dni"] : "",
                        FechaNacimiento = datos.Lector["FechaNacimiento"] != DBNull.Value ? (DateTime)datos.Lector["FechaNacimiento"] : DateTime.MinValue,
                        Telefono = datos.Lector["Telefono"] != DBNull.Value ? (string)datos.Lector["Telefono"] : "",
                        Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "",
                        IdPlan = datos.Lector["IdPlan"] != DBNull.Value ? (int)datos.Lector["IdPlan"] : 0,
                        Activo = datos.Lector["Activo"] != DBNull.Value ? (bool)datos.Lector["Activo"] : true
                    };
                }
                return socio;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public Socio ObtenerPorId(int idSocio)
        {
            AccesoDatos datos = new AccesoDatos();
            Socio socio = null;

            try
            {
                datos.setearConsulta("SELECT IdSocio, Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, IdPlan, Activo FROM SOCIOS WHERE IdSocio = @IdSocio");
                datos.setearParametro("@IdSocio", idSocio);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    socio = new Socio
                    {
                        IdSocio = (int)datos.Lector["IdSocio"],
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? (string)datos.Lector["Nombre"] : "",
                        Apellido = datos.Lector["Apellido"] != DBNull.Value ? (string)datos.Lector["Apellido"] : "",
                        Dni = datos.Lector["Dni"] != DBNull.Value ? (string)datos.Lector["Dni"] : "",
                        FechaNacimiento = datos.Lector["FechaNacimiento"] != DBNull.Value ? (DateTime)datos.Lector["FechaNacimiento"] : DateTime.MinValue,
                        Telefono = datos.Lector["Telefono"] != DBNull.Value ? (string)datos.Lector["Telefono"] : "",
                        Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "",
                        IdPlan = datos.Lector["IdPlan"] != DBNull.Value ? (int)datos.Lector["IdPlan"] : 0,
                        Activo = datos.Lector["Activo"] != DBNull.Value ? (bool)datos.Lector["Activo"] : true
                    };
                }

                return socio;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public Socio ObtenerPorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            Socio socio = null;
            try
            {
                datos.setearConsulta("SELECT IdSocio, Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, IdPlan, Activo FROM SOCIOS WHERE Email = @Email");
                datos.setearParametro("@Email", email);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    socio = new Socio
                    {
                        IdSocio = (int)datos.Lector["IdSocio"],
                        Nombre = datos.Lector["Nombre"] != DBNull.Value ? (string)datos.Lector["Nombre"] : "",
                        Apellido = datos.Lector["Apellido"] != DBNull.Value ? (string)datos.Lector["Apellido"] : "",
                        Dni = datos.Lector["Dni"] != DBNull.Value ? (string)datos.Lector["Dni"] : "",
                        FechaNacimiento = datos.Lector["FechaNacimiento"] != DBNull.Value ? (DateTime)datos.Lector["FechaNacimiento"] : DateTime.MinValue,
                        Telefono = datos.Lector["Telefono"] != DBNull.Value ? (string)datos.Lector["Telefono"] : "",
                        Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : "",
                        IdPlan = datos.Lector["IdPlan"] != DBNull.Value ? (int)datos.Lector["IdPlan"] : 0,
                        Activo = datos.Lector["Activo"] != DBNull.Value ? (bool)datos.Lector["Activo"] : true
                    };
                }
                return socio;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int Agregar(Socio socio)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"INSERT INTO SOCIOS (Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, IdPlan, Activo)
                               VALUES (@Nombre, @Apellido, @Dni, @FechaNacimiento, @Telefono, @Email, @IdPlan, @Activo);
                               SELECT SCOPE_IDENTITY();");
                datos.setearParametro("@Nombre", socio.Nombre);
                datos.setearParametro("@Apellido", socio.Apellido);
                datos.setearParametro("@Dni", socio.Dni);
                datos.setearParametro("@FechaNacimiento", socio.FechaNacimiento == DateTime.MinValue ? (object)DBNull.Value : socio.FechaNacimiento);
                datos.setearParametro("@Telefono", socio.Telefono);
                datos.setearParametro("@Email", socio.Email);
                datos.setearParametro("@IdPlan", socio.IdPlan);
                datos.setearParametro("@Activo", socio.Activo);

                object result = datos.ejecutarLectura().Read() ? datos.Lector[0] : null;
                return result != null ? Convert.ToInt32(result) : 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Socio socio)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"UPDATE SOCIOS
                                       SET Nombre = @Nombre,
                                           Apellido = @Apellido,
                                           Dni = @Dni,
                                           FechaNacimiento = @FechaNacimiento,
                                           Telefono = @Telefono,
                                           Email = @Email,
                                           IdPlan = @IdPlan,
                                           Activo = @Activo
                                       WHERE IdSocio = @IdSocio");

                datos.setearParametro("@Nombre", socio.Nombre);
                datos.setearParametro("@Apellido", socio.Apellido);
                datos.setearParametro("@Dni", socio.Dni);
                datos.setearParametro("@FechaNacimiento", socio.FechaNacimiento == DateTime.MinValue ? (object)DBNull.Value : socio.FechaNacimiento);
                datos.setearParametro("@Telefono", socio.Telefono);
                datos.setearParametro("@Email", socio.Email);
                datos.setearParametro("@IdPlan", socio.IdPlan);
                datos.setearParametro("@Activo", socio.Activo);
                datos.setearParametro("@IdSocio", socio.IdSocio);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void BajaLogica(int idSocio)
        {
            // BAJA LOGICA SOCIOS
            AccesoDatos datosSocio = new AccesoDatos();
            try
            {
                datosSocio.setearConsulta("UPDATE SOCIOS SET Activo = 0 WHERE IdSocio = @IdSocio");
                datosSocio.setearParametro("@IdSocio", idSocio);
                datosSocio.ejecutarAccion();
            }
            finally
            {
                datosSocio.cerrarConexion();
            }

            //BAJA LOGICA USUARIOS
            AccesoDatos datosUsuario = new AccesoDatos();
            try
            {
                datosUsuario.setearConsulta("UPDATE USUARIOS SET Activo = 0 WHERE IdSocio = @IdSocio");
                datosUsuario.setearParametro("@IdSocio", idSocio);
                datosUsuario.ejecutarAccion();
            }
            finally
            {
                datosUsuario.cerrarConexion();
            }
        }
    }
}
