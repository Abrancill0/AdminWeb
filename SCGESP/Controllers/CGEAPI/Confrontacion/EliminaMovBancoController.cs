using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class EliminaMovBancoController : ApiController
    {
        public class ListResult
        {
            public bool EliminadoOk { get; set; }
            public string Descripcion { get; set; }
        }

        public class ParametrosMovBanco
        {
            public string IdMovBanco { get; set; }
            public string IdInforme { get; set; }
            public string IdGasto { get; set; }
        }

        public ListResult Post(ParametrosMovBanco Datos)
        {
            SqlCommand comando = new SqlCommand("DeleteMovBanco")
            {
                CommandType = CommandType.StoredProcedure
            };

            //Declaracion de parametros
            comando.Parameters.Add("@idmovbanco", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@idgasto", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idmovbanco"].Value = Datos.IdMovBanco;
            comando.Parameters["@idinforme"].Value = Datos.IdInforme;
            comando.Parameters["@idgasto"].Value = Datos.IdGasto;

            //Ejecutar comando
            bool exito = false;
            string mensaje = "";
            try
            {
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                comando.ExecuteNonQuery();
                comando.Connection.Close();
                exito = true;
                mensaje = "Movimiento eliminado";
            }
            catch (Exception ex)
            {
                var error = ex;
                exito = false;
                mensaje = "Error al eliminar movimiento. " + Convert.ToString(error);
            }

            ListResult lista = new ListResult
            {
                EliminadoOk = exito,
                Descripcion = mensaje
            };

            return lista;
        }

    }
}
