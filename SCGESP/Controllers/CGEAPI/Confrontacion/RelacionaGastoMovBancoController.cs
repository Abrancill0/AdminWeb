using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class RelacionaGastoMovBancoController : ApiController
    {
        public class ListResult
        {
            public bool RelacionOk { get; set; }
            public string Descripcion { get; set; }
        }
        public class ParametrosMovBanco
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public int IdMovBanco { get; set; }
            public double Importe { get; set; }
        }
        public ListResult Post(ParametrosMovBanco Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "UPDATE gastos SET g_idmovbanco = " + Datos.IdMovBanco + ", " +
                              //" g_total = " + Datos.Importe + ", " +
                              //" g_valor = IIF(RTRIM(LTRIM(ISNULL(g_dirxml, ''))) = '', " + Datos.Importe + ", g_valor) " +
                              " g_valor = IIF(RTRIM(LTRIM(ISNULL(g_dirxml, ''))) = '', g_total, g_valor) " +
                              " WHERE g_idinforme = " + Datos.IdInforme +
                              " AND g_id = " + Datos.IdGasto + "; " +
                              "UPDATE movbancarios SET " +
                              " m_idinforme = " + Datos.IdInforme + ", " +
                              " m_idgasto = " + Datos.IdGasto +
                              " WHERE m_id = " + Datos.IdMovBanco + ";";
            try
            {
                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);
                ListResult resultado = new ListResult
                {
                    RelacionOk = true,
                    Descripcion = "Gasto relacionado a movimiento bancario."
                };
                return resultado;
            }
            catch (Exception err)
            {
                var error = Convert.ToString(err);
                ListResult resultado = new ListResult
                {
                    RelacionOk = false,
                    Descripcion = "Error al relacionar. " + error
                };
                return resultado;
            }
        }
    }
}
