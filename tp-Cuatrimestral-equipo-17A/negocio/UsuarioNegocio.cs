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
    }
}
