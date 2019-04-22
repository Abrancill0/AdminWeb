using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class EnviaAutorizacionController : ApiController
    {
        public class ParametrosGastos
        {
            public int idinforme { get; set; }

        }

        public string PostInsertGasto(ParametrosGastos Datos)
        {
            SqlCommand comando = new SqlCommand("EnviaAutorizacion");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros

            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.idinforme;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            // comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);


            if (DT.Rows.Count > 0)
            {

                foreach (DataRow row in DT.Rows)
                {
                    string usuarioResponsable = Convert.ToString(row["usuarioResponsable"]);
                    string usuarioAutoriza = Convert.ToString(row["usuarioAutoriza"]);
                    string mensaje = Convert.ToString(row["msn"]);
                    string titulo = Convert.ToString(row["titulo"]);
                    
                    EnvioCorreosELE.Envio(usuarioResponsable, "", "", usuarioAutoriza, "", titulo, mensaje, 0);

                }

                return "";
            }
            else
            {
                return null;
            }
        }


    }
}
