using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class EliminarInformesController : ApiController
    {
        public class datos
        {
            public int idproyecto { get; set; }
            public int idinforme { get; set; }
            public int idempresa { get; set; }
        }

        public class LoginResult
        {
            public string MSN { get; set; }
        }

        public IEnumerable<LoginResult> Post(datos Datos)
        {
            SqlCommand comando = new SqlCommand("DeleteInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idproyecto", SqlDbType.VarChar);
            comando.Parameters.Add("@idinforme", SqlDbType.VarChar);
            comando.Parameters.Add("@idempresa", SqlDbType.VarChar);

            //Asignacion de valores a parametros
            comando.Parameters["@idproyecto"].Value = Datos.idproyecto;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@idempresa"].Value = Datos.idempresa;
            //comando.Parameters["@pid"].Direction = ParameterDirection.Output;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            LoginResult[] items;

            if (DT.Rows.Count > 0)
            {
                DataRow row = DT.Rows[0];

                items = new LoginResult[]
                {
                        new LoginResult{MSN = Convert.ToString(row["MSN"])}
                };
                return items;

            }
            else
            {
                return null;
            }

        }
    }

}