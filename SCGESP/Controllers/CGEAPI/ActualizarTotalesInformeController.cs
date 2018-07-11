using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class ActualizarTotalesInformeController : ApiController
    {
        public class ListResult
        {
            public bool ActualizadoOk { get; set; }
            public string Descripcion { get; set; }
        }

        public class Parametros
        {
            public int IdInforme { get; set; }
        }
        public ListResult PostActualizarTotales(Parametros Datos)
        {
            ListResult resultado = new ListResult();
            try
            {
                SqlCommand comando = new SqlCommand("UpdateTotalInforme")
                {
                    CommandType = CommandType.StoredProcedure
                };
                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters["@idinforme"].Value = Datos.IdInforme;
                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                resultado.ActualizadoOk = true;
                resultado.Descripcion = "Totales Actualizados.";
            }
            catch (Exception ex)
            {
                string error = "Error : " + ex.ToString();
                resultado.ActualizadoOk = false;
                resultado.Descripcion = "Error al actializar totales. " + error;
            }
            
            return resultado;
        }
    }
}
