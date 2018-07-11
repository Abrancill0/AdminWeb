using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaInformesSinConfrontarController : ApiController
    {
        public class ListResult
        {
            public int IdInforme { get; set; }
            public string NmbInforme { get; set; }
            public int NoInforme { get; set; }
            public decimal Total { get; set; }
            public decimal Totalg { get; set; }
        }
        public class ParametrosMovBanco
        {
            public string Usuario { get; set; }
        }
        public IEnumerable<ListResult> Post(ParametrosMovBanco Datos)
        {
            SqlCommand comando = new SqlCommand("SelectInformeSinConfrontar");
            comando.CommandType = CommandType.StoredProcedure;
            //Declaracion de parametros
            comando.Parameters.Add("@usuario", SqlDbType.VarChar);
            //Asignacion de valores a parametros
            comando.Parameters["@usuario"].Value = Datos.Usuario;
            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();

            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    int RowIdInforme = Convert.ToInt32(row["idinforme"]);
                    string RowNmbInforme = Convert.ToString(row["nmbinforme"]);
                    int RowNoInforme = Convert.ToInt32(row["ninforme"]);
                    decimal RowTotal = Convert.ToDecimal(row["total"]);
                    decimal RowTotalg = Convert.ToDecimal(row["totalg"]);
                    ListResult resultado = new ListResult
                    {
                        IdInforme = RowIdInforme,
                        NmbInforme = RowNmbInforme,
                        NoInforme = RowNoInforme,
                        Total = RowTotal,
                        Totalg = RowTotalg
                    };
                    lista.Add(resultado);
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
