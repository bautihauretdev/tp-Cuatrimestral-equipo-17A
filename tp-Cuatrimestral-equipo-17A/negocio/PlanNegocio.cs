using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class PlanNegocio
    {
        public int Agregar(Plan plan)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"INSERT INTO PLANES(Nombre, PrecioMensual, MaxHorasSemana, Activo)
                               VALUES (@Nombre, @PrecioMensual, @MaxHorasSemana, @Activo);
                               SELECT SCOPE_IDENTITY();");
                datos.setearParametro("@Nombre", plan.Nombre);
                datos.setearParametro("@PrecioMensual", plan.PrecioMensual);
                datos.setearParametro("@MaxHorasSemana", plan.MaxHorasSemana);
                datos.setearParametro("@Activo", plan.Activo);


                object result = datos.ejecutarScalar();
                return Convert.ToInt32(result);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
