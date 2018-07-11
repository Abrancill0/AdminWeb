using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.EleAPI
{
    public class AutorizaInformeController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string idinforme { get; set; }
			public string comentario { get; set; }
		}

        public class ObtieneInformeResult
        {
            public int i_id { get; set; }
            public int r_idrequisicion { get; set; }
            public string g_dirxml { get; set; }
            public string g_dirpdf { get; set; }
            public double g_total { get; set; }
            public int idMaterial { get; set; }
            public double iva { get; set; }
			public int RmReqGasto { get; set; }
		}

        public string Post(datos Datos)
        {
            string usuariodesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            SqlCommand comando = new SqlCommand("AutorizaInformes");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@i_id", SqlDbType.Int);
			comando.Parameters.Add("@Usuario", SqlDbType.VarChar);
			comando.Parameters.Add("@comentario", SqlDbType.VarChar);


			//Asignacion de valores a parametros
			comando.Parameters["@i_id"].Value = Datos.idinforme;
			comando.Parameters["@Usuario"].Value = usuariodesencripta;
			comando.Parameters["@comentario"].Value = Datos.comentario;

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
                    string mensaje = Convert.ToString(row["msn"]);
                    string titulo = Convert.ToString(row["titulo"]);
                    string autorizador = Convert.ToString(row["a_uautoriza"]);

                    EnvioCorreosELE.Envio(usuariodesencripta, "", "", autorizador, "", titulo, mensaje, 0);

                }

            }

            List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

            //if (DT.Rows.Count > 0)
            //{ }
            return "";
        }


    }
}
