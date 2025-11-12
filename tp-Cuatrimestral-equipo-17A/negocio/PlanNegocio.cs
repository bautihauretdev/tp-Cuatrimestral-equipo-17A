using dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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

        public List<Plan> ListarPlanes()
        {
            var lista = new List<Plan>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"SELECT IdPlan, Nombre, PrecioMensual, MaxHorasSemana, Activo FROM PLANES WHERE Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Plan plan = new Plan
                    {
                        IdPlan = (int)datos.Lector["IdPlan"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        PrecioMensual = (decimal)datos.Lector["PrecioMensual"],
                        MaxHorasSemana = (int)datos.Lector["MaxHorasSemana"],
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(plan);
                }
                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Plan ObtenerPlanPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdPlan, Nombre, PrecioMensual, MaxHorasSemana, Activo FROM PLANES WHERE IdPlan = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Plan
                    {
                        IdPlan = (int)datos.Lector["IdPlan"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        PrecioMensual = (decimal)datos.Lector["PrecioMensual"],
                        MaxHorasSemana = (int)datos.Lector["MaxHorasSemana"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void Modificar(Plan plan)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"UPDATE PLANES
                                       SET Nombre = @Nombre,
                                           PrecioMensual = @PrecioMensual,
                                           MaxHorasSemana = @MaxHorasSemana
                                       WHERE IdPlan = @IdPlan");

                datos.setearParametro("@IdPlan", plan.IdPlan);
                datos.setearParametro("@Nombre", plan.Nombre);
                datos.setearParametro("@PrecioMensual", plan.PrecioMensual);
                datos.setearParametro("@MaxHorasSemana", plan.MaxHorasSemana);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void BajaLogica(int idPlan)
        {
            AccesoDatos datosPlan = new AccesoDatos();
            try
            {
                datosPlan.setearConsulta("UPDATE PLANES SET Activo = 0 WHERE IdPlan = @IdPlan");
                datosPlan.setearParametro("@IdPlan", idPlan);
                datosPlan.ejecutarAccion();
            }
            finally
            {
                datosPlan.cerrarConexion();
            }
        }

    }
}

