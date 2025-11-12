using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class UsuarioNegocio
    {
        public Usuario Login(string email, string password)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuario usuario = null;

            try
            {
                datos.setearConsulta("SELECT IdUsuario, Email, PasswordHash, Rol, Activo, IdSocio FROM USUARIOS WHERE Email = @email AND PasswordHash = @password AND Activo = 1");
                datos.setearParametro("@email", email);
                datos.setearParametro("@password", password);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = (int)datos.Lector["IdUsuario"],
                        Email = (string)datos.Lector["Email"],
                        PasswordHash = (string)datos.Lector["PasswordHash"],
                        Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), (string)datos.Lector["Rol"]),
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    if (datos.Lector["IdSocio"] != DBNull.Value)
                    {
                        usuario.Socio = new Socio
                        {
                            IdSocio = (int)datos.Lector["IdSocio"]
                        };
                    }
                }

                return usuario;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void AgregarUsuarioSocio(string email, string dni, int idSocio)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"INSERT INTO USUARIOS (Email, PasswordHash, Rol, Activo, IdSocio)
                               VALUES (@Email, @PasswordHash, 'Socio', 1, @IdSocio)");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@PasswordHash", dni); // PENDIENTE: Cambiar esto para usar un hash real
                datos.setearParametro("@IdSocio", idSocio);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //Comprueba si el email ya esta en uso por otro usuario (distinto al socio actual)
        public bool EmailEnUsoPorOtro(string email, int idSocio)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT IdUsuario FROM USUARIOS WHERE Email = @Email AND (IdSocio IS NULL OR IdSocio <> @IdSocio)");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@IdSocio", idSocio);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        
        //Actualiza el email del usuario asociado a un IdSocio
        public void ActualizarEmailPorIdSocio(int idSocio, string nuevoEmail)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE USUARIOS SET Email = @Email WHERE IdSocio = @IdSocio");
                datos.setearParametro("@Email", nuevoEmail);
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
