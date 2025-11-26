using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class CuotaNegocio
    { // LISTA TODOS LOS SOCIOS ACTIVOS EN EL SISTEMA 
        public List<Socio> ListarSocios()
        {
            var lista = new List<Socio>();
            AccesoDatos acceso = new AccesoDatos();

            try
            { // SU CONSULTA O SEA LOS ATRIBUTOS O DATOS QUE VA A DEVOLVER
                acceso.setearConsulta(@"SELECT IdSocio, Nombre, Apellido 
                                        FROM SOCIOS 
                                        WHERE Activo = 1");
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
        // OBTIENE LA CUOTA DEL SOCIO PARA EL MES Y EL AÑO ACTUAL
        public Cuota ObtenerCuotaActual(int idSocio)
        {
            Cuota cuota = null;
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta(@"SELECT IdCuota, IdPago, Anio, Mes, Monto, Recargo, Estado 
                                        FROM CUOTAS 
                                        WHERE IdSocio = @idSocio 
                                          AND Mes = @mes 
                                          AND Anio = @anio");
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
        // Actualiza el estado de una cuota (ej: Pendiente -> Pagado)
        public void ActualizarEstadoCuota(int idCuota, string nuevoEstado)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            { // ctualiza el campo Estado de la cuota
                acceso.setearConsulta(@"UPDATE CUOTAS 
                                        SET Estado = @estado 
                                        WHERE IdCuota = @idCuota");
                acceso.setearParametro("@estado", nuevoEstado);
                acceso.setearParametro("@idCuota", idCuota);

                acceso.ejecutarAccion();
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }
        // Devuelve todas las cuotas pagadas con datos del socio
        public List<Cuota> ObtenerCuotasPagadas()
        {
            var lista = new List<Cuota>();
            AccesoDatos acceso = new AccesoDatos();

            try
            { // CUOTAS PAGADAS + DATOS DEL SOCIO 
                acceso.setearConsulta(@"
    SELECT c.IdCuota, c.Anio, c.Mes, c.Monto, c.Recargo, c.Estado, c.FechaPago, c.FormaPago,
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
                        FechaPago = dr["FechaPago"] != DBNull.Value ? (DateTime?)dr["FechaPago"] : null,
                        FormaPago = dr["FormaPago"].ToString(),
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
        // BUSCA SOCIOS ACTIVO POR DNI EXACTO
        public Socio BuscarSocioPorDniONombre(string criterio)
        {
            Socio socio = null;
            AccesoDatos acceso = new AccesoDatos();

            try
            { // CONSULTA PARA LA BUSQUEDA Y DEVUELVE MONTO TOTAL A COBRARLE, SI DEBE RECARGO O NO Y EL PLAN ADQUIRIDO 
                acceso.setearConsulta(@"
                    SELECT s.IdSocio, s.Nombre, s.Apellido, s.Dni, s.IdPlan,
                           p.Nombre AS NombrePlan, p.PrecioMensual
                    FROM SOCIOS s
                    INNER JOIN PLANES p ON s.IdPlan = p.IdPlan
                    WHERE s.Activo = 1 
                      AND (s.Dni = @criterio OR s.Nombre LIKE @criterio + '%')");

                acceso.setearParametro("@criterio", criterio);

                var dr = acceso.ejecutarLectura();
                if (dr.Read())
                {
                    socio = new Socio
                    {
                        IdSocio = (int)dr["IdSocio"],
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        Dni = dr["Dni"].ToString(),
                        IdPlan = (int)dr["IdPlan"],
                        Plan = new Plan
                        {
                            IdPlan = (int)dr["IdPlan"],
                            Nombre = dr["NombrePlan"].ToString(),
                            PrecioMensual = (decimal)dr["PrecioMensual"]
                        }
                    };
                }
            }
            finally
            {
                acceso.cerrarConexion();
            }

            return socio;
        }
        // permite guardar los cobros
        public void GuardarCobro(int idSocio, decimal monto, decimal recargo, string formaPago)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            { // si ya existe cuota del mes/año actual → UPDATE, si no → INSERT
                acceso.setearConsulta(@"
            IF EXISTS (SELECT 1 FROM CUOTAS WHERE IdSocio = @idSocio AND Mes = @mes AND Anio = @anio)
            BEGIN
                UPDATE CUOTAS
                SET Estado = 'Pagado',
                    Monto = @monto,
                    Recargo = @recargo,
                    FechaPago = GETDATE(),
                    FormaPago = @formaPago
                WHERE IdSocio = @idSocio AND Mes = @mes AND Anio = @anio
            END
            ELSE
            BEGIN
                INSERT INTO CUOTAS (IdSocio, Anio, Mes, Monto, Recargo, Estado, FechaPago, FormaPago)
                VALUES (@idSocio, @anio, @mes, @monto, @recargo, 'Pagado', GETDATE(), @formaPago)
            END");

                acceso.setearParametro("@idSocio", idSocio);
                acceso.setearParametro("@mes", DateTime.Now.Month);
                acceso.setearParametro("@anio", DateTime.Now.Year);
                acceso.setearParametro("@monto", monto);
                acceso.setearParametro("@recargo", recargo);
                acceso.setearParametro("@formaPago", formaPago);

                acceso.ejecutarAccion();
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }
        // OBTIENE TODAS LAS CUOTAS DEL SOCIO (SIN IMPORTAR SU ESTADO) ordenadas de más reciente a más antigua

        public List<Cuota> ObtenerCuotasPorSocio(int idSocio)
        {
            var lista = new List<Cuota>();
            AccesoDatos acceso = new AccesoDatos();

            try
            { // CONSULTA PARA OBTENER CUOTAS DE TODO TIPO
                acceso.setearConsulta(@" 
            SELECT c.IdCuota, c.Anio, c.Mes, c.Monto, c.Recargo, c.Estado, c.FechaPago, c.FormaPago
            FROM CUOTAS c
            WHERE c.IdSocio = @idSocio
            ORDER BY c.Anio DESC, c.Mes DESC");

                acceso.setearParametro("@idSocio", idSocio);

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
                        FechaPago = dr["FechaPago"] != DBNull.Value ? (DateTime?)dr["FechaPago"] : null,
                        FormaPago = dr["FormaPago"].ToString()
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
        // OBTIENE LAS CUOTAS PENDIENTES POR SOCIO
        public List<Cuota> ObtenerCuotasDeudorasPorSocio(int idSocio)
        {
            var lista = new List<Cuota>();
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                // trae todas las cuotas pendientes (Estado = 'Deudor') de un socio
                // Ordenadas de la más vieja a la más nueva (ASC)
                acceso.setearConsulta(@"
            SELECT IdCuota, Anio, Mes, Monto, Recargo, Estado
            FROM CUOTAS
            WHERE IdSocio = @idSocio AND Estado = 'Deudor'
            ORDER BY Anio ASC, Mes ASC");

                acceso.setearParametro("@idSocio", idSocio);

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
                        Estado = dr["Estado"].ToString()
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
        // GUARDA LOS COBROS EN ESTADO PENDIENTE 
        public void GuardarCobroPendiente(int idCuota, decimal monto, decimal recargo, string formaPago)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                // actualiza la cuota seleccionada con datos de pago
                // Si hay recargo → Estado = 'ConRecargo'
                // Si no hay recargo → Estado = 'Pagado'
                acceso.setearConsulta(@"
            UPDATE CUOTAS
            SET Monto = @monto,
                Recargo = @recargo,
                Estado = CASE WHEN @recargo > 0 THEN 'ConRecargo' ELSE 'Pagado' END,
                FechaPago = GETDATE(),
                FormaPago = @formaPago
            WHERE IdCuota = @idCuota");


                acceso.setearParametro("@monto", monto);
                acceso.setearParametro("@recargo", recargo);
                acceso.setearParametro("@formaPago", formaPago);
                acceso.setearParametro("@idCuota", idCuota);

                acceso.ejecutarAccion();
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }
    }
}