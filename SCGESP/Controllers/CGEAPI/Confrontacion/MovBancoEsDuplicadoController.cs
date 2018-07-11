using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class MovBancoEsDuplicadoController : ApiController
    {
        public class ListResult
        {
            public string Duplicado { get; set; }
            public int IdMovimiento { get; set; }
            public string Fecha { get; set; }
         
        }

        public class ParametrosMovBanco
        {
            public string Tarjeta { get; set; }
            public string Banco { get; set; }
            public string Fecha { get; set; }
            public decimal Importe { get; set; }
        }

        public IEnumerable<ListResult> Post(ParametrosMovBanco Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "SELECT idmovimiento, " +
                "IIF(idmovimiento = 0, NULL, " +
                "(SELECT IIF(m_fultimarecarga IS NULL, m_fcrea, m_fultimarecarga) " +
                "FROM movbancarios WHERE m_id = idmovimiento)) AS fecha " +
                "FROM( " +
                    "SELECT ISNULL(MAX(m_id), 0) AS idmovimiento " +
                    "FROM movbancarios " +
                    "WHERE m_tarjeta = '" + Datos.Tarjeta + "' " +
                    "AND m_banco = '" + Datos.Banco + "' " +
                    "AND m_fmovimiento >= '" + Convert.ToDateTime(Datos.Fecha).ToString("yyyy-MM-dd") + "' AND m_importe = " + Datos.Importe + " " +
                ") AS DATOS";


            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);


            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    int RowIdMovimiento = Convert.ToInt32(row["idmovimiento"]);
                    if (RowIdMovimiento > 0)
                    {
                        string RowFecha = Convert.ToString(row["fecha"]);
                        ListResult ent = new ListResult
                        {
                            IdMovimiento = RowIdMovimiento,
                            Fecha = RowFecha,
                            Duplicado = "Si",

                        };
                        lista.Add(ent);
                    }
                    else {
                        ListResult ent = new ListResult
                        {
                            IdMovimiento = 0,
                            Fecha = null,
                            Duplicado = "No",

                        };
                        lista.Add(ent);
                    }                    
                }

                return lista;
            }
            else
            {
                return null;
            }
        }

    }
}
