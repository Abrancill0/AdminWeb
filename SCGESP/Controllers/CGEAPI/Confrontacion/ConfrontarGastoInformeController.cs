using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConfrontarGastoInformeController : ApiController
    {
        public class ListResult
        {
            public bool ConfrontarOk { get; set; }
            public string Descripcion { get; set; }
        }
        public class ParametrosGastoInforme
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public int IdMovBanco { get; set; }
            public int ChkOk { get; set; }
        }
        public ListResult Post(ParametrosGastoInforme Datos)
        {
            SqlCommand comando = new SqlCommand("ConfrontarGastoInforme")
            {
                CommandType = CommandType.StoredProcedure
            };
            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@idgasto", SqlDbType.Int);
            comando.Parameters.Add("@idmovbanco", SqlDbType.Int);
            comando.Parameters.Add("@chkok", SqlDbType.Int);
            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.IdInforme;
            comando.Parameters["@idgasto"].Value = Datos.IdGasto;
            comando.Parameters["@idmovbanco"].Value = Datos.IdMovBanco;
            comando.Parameters["@chkok"].Value = Datos.ChkOk;

            bool RConfrontarOk = false;
            string RDescripcion = "";
            try
            {
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();

                DA.Fill(DT);

                RConfrontarOk = true;
                RDescripcion = "Gasto-Informe Confrontado";
            }
            catch (Exception ex)
            {

                var error = Convert.ToString(ex);

                RConfrontarOk = false;
                RDescripcion = "Error al Confrontado Gasto-Informe. " + error;
            }
            ListResult resultado = new ListResult
            {
                ConfrontarOk = RConfrontarOk,
                Descripcion = RDescripcion
            };
            return resultado;
        }
    }
}
