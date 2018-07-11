using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ValidaExistenComprobantesController : ApiController
    {
        public class ListResult
        {
            public int IdInforme { get; set; }
            public int NoGastos { get; set; }
            public int NoGastosConXML { get; set; }
            public int NoGastosConPDF { get; set; }
            public int NoGastosConIMG { get; set; }
            public int NoGastosDeducible { get; set; }
            public int NoGastosNoDeducible { get; set; }
            public int NoGastosConComprobante { get; set; }
        }
        public class Parametros
        {
            public int IdInforme { get; set; }
        }
        public ListResult Post(Parametros Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("validaExistenComprobantes");
                comando.CommandType = CommandType.StoredProcedure;
                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                //Asignacion de valores a parametros
                comando.Parameters["@idinforme"].Value = Datos.IdInforme;
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();

                DA.Fill(DT);

                List<ListResult> Respuesta = new List<ListResult>();

                if (DT.Rows.Count > 0)
                {
                    int RowNGastos = 0, RowNXML = 0, RowNPDF = 0,
                        RowNOtros = 0, RowNGastosDeducibles = 0,
                        RowNGastosNoDeducibles = 0, RowNGConComprobante = 0;
                    // DataRow row = DT.Rows[0];
                    foreach (DataRow row in DT.Rows)
                    {
                        RowNGastos = Convert.ToInt32(row["ngastos"]);
                        RowNXML = Convert.ToInt32(row["nxml"]);
                        RowNPDF = Convert.ToInt32(row["npdf"]);
                        RowNOtros = Convert.ToInt32(row["notros"]);
                        RowNGastosDeducibles = Convert.ToInt32(row["nfiscal"]);
                        RowNGastosNoDeducibles = Convert.ToInt32(row["nnofiscal"]);
                        RowNGConComprobante = Convert.ToInt32(row["ngccomprobante"]);
                    }
                    ListResult resultado = new ListResult
                    {
                        IdInforme = Datos.IdInforme,
                        NoGastos = RowNGastos,
                        NoGastosConXML = RowNXML,
                        NoGastosConPDF = RowNPDF,
                        NoGastosConIMG = RowNOtros,
                        NoGastosDeducible = RowNGastosDeducibles,
                        NoGastosNoDeducible = RowNGastosNoDeducibles,
                        NoGastosConComprobante = RowNGConComprobante
                    };
                    return resultado;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception err)
            {
                var error = err.ToString();
                return null;
            }
        }
    }
}
