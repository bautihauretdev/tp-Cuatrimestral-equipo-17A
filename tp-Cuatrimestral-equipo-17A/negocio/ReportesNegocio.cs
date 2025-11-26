using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ReportesNegocio
    {
        // REPORTE 01 - TOTAL SOCIOS ACTIVOS (VISTA)
        public int ObtenerTotalSociosActivos()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TotalSociosActivos FROM vw_TotalSociosActivos");
                var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return Convert.ToInt32(lector["TotalSociosActivos"]);

                return 0;
            }
            finally 
            { 
                datos.cerrarConexion(); 
            }
        }

        // REPORTE 02 - INGRESOS MES (VISTA)
        public decimal ObtenerIngresosMes()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TotalIngresosMes FROM vw_IngresosMes");
                var lector = datos.ejecutarLectura();

                if (lector.Read() && lector["TotalIngresosMes"] != DBNull.Value)
                    return Convert.ToDecimal(lector["TotalIngresosMes"]);

                return 0;
            }
            finally 
            { 
                datos.cerrarConexion(); 
            }
        }

        // REPORTE 03 - SOCIOS MOROSOS (SP)
   
        public int ObtenerSociosMorosos()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_SociosMorosos");
                
               var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return Convert.ToInt32(lector["CantidadMorosos"]);

                return 0;
            }
            finally 
            {
                datos.cerrarConexion(); 
            }
        }

        // REPORTE 04 - DIA MAS CONCURRENCIA (SP)
        
        public string DiaMayorConcurrencia()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_DiaMasConcurrencia");

                var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return lector["DiaSemana"].ToString();

                return "—";
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 05 - FRANJA HORARIA MÁS CONCURRENCIA (SP)
        public string FranjaMayorConcurrencia()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_FranjaHorariaMasConcurrida");

                var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return lector["FranjaHoraria"].ToString();

                return "—";
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 06 - DÍA MENOS CONCURRENCIA (SP)
        public string DiaMenorConcurrencia()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_DiaMenosConcurrencia");

                var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return lector["DiaSemana"].ToString();

                return "—";
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 07 - FRANJA MENOS CONCURRENCIA (SP)
        public string FranjaMenorConcurrencia()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_FranjaHorariaMenosConcurrida");

                var lector = datos.ejecutarLectura();

                if (lector.Read())
                    return lector["FranjaHoraria"].ToString();

                return "—";
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 08 - OCUPACIÓN PROMEDIO TURNOS (SP)
        public decimal OcupacionPromedio()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_Reporte_OcupacionPromedioTurnos");

                object resultado = datos.ejecutarScalar(); // puede ser null o DBNull

                if (resultado == null || resultado == DBNull.Value)
                    return 0m;

                return Convert.ToDecimal(resultado);
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 09 - TOP RESERVAS (SP)
        public List<TopReservasDTO> TopSociosReservas()
        {
            List<TopReservasDTO> lista = new List<TopReservasDTO>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_Reporte_TopSociosReservas");

                var lector = datos.ejecutarLectura();

                while (lector.Read())
                {
                    lista.Add(new TopReservasDTO
                    {
                        Nombre = lector["Nombre"].ToString() + " " + lector["Apellido"].ToString(),
                        Reservas = Convert.ToInt32(lector["TotalReservas"])
                    });
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 10 - TOP MOROSOS (SP)

        public List<MorosoDTO> TopMorosos()
        {
            List<MorosoDTO> lista = new List<MorosoDTO>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_Reporte_TopMorosos");

                var lector = datos.ejecutarLectura();

                while (lector.Read())
                {
                    lista.Add(new MorosoDTO
                    {
                        Nombre = lector["Nombre"].ToString() + " " + lector["Apellido"].ToString(),
                        Deudas = Convert.ToInt32(lector["CuotasAdeudadas"])
                    });
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        // REPORTE 11 - PRÓXIMOS PAGOS VENCER (SP)
        public List<ProximoPagoDTO> ProximosVencimientos()
        {
            List<ProximoPagoDTO> lista = new List<ProximoPagoDTO>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_Reporte_ProxPagosVencer");

                var lector = datos.ejecutarLectura();

                while (lector.Read())
                {
                    var fechaVenc = Convert.ToDateTime(lector["FechaVencimiento"]);
                    int diasRestantes = (fechaVenc - DateTime.Now).Days;

                    lista.Add(new ProximoPagoDTO
                    {
                        Nombre = lector["Nombre"].ToString() + " " + lector["Apellido"].ToString(),
                        Monto = 0, // Tu SP no devuelve Monto → si querés lo agrego
                        DiasRestantes = diasRestantes
                    });
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }
        // DTOs
        public class TopReservasDTO
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int Reservas { get; set; }
        }

        public class MorosoDTO
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int Deudas { get; set; }
        }

        public class ProximoPagoDTO
        {
            public string Nombre { get; set; }
            public decimal Monto { get; set; }
            public int DiasRestantes { get; set; }
        }
    }
}

