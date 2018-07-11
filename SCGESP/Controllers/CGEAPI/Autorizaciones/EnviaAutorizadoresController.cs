using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI.Autorizaciones
{
    public class EnviaAutorizadoresController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string idinforme { get; set; }
            public string RmReqId { get; set; }
            public string[] Autorizadores { get; set; }
			public string comentario { get; set; }

		}

        public class CatalogoUsuariosResult
        {
            public int Resultado { get; set; }
        }

        public string Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);
            int nAutorizadores = Datos.Autorizadores.Count();
            int i = 2;
            foreach (var item in Datos.Autorizadores) {
                string autorizador = item;

                SqlCommand comando = new SqlCommand("EnviaAutorizadores");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters.Add("@a_uautoriza", SqlDbType.VarChar);
                comando.Parameters.Add("@nivel", SqlDbType.Int);
                comando.Parameters.Add("@NumeroNiveles", SqlDbType.Int);
				comando.Parameters.Add("@usuario_actual", SqlDbType.VarChar);
				comando.Parameters.Add("@comentario", SqlDbType.VarChar);

				//Asignacion de valores a parametros
				comando.Parameters["@idinforme"].Value = Datos.idinforme;
                comando.Parameters["@a_uautoriza"].Value = autorizador;
                comando.Parameters["@nivel"].Value = 0;
				comando.Parameters["@NumeroNiveles"].Value = 0; // (nAutorizadores + 1);
				comando.Parameters["@usuario_actual"].Value = UsuarioDesencripta;
				comando.Parameters["@comentario"].Value = Datos.comentario;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                if (DT.Rows.Count > 0 && i == 1)
                {

                    foreach (DataRow row in DT.Rows)
                    {
                        string mensaje = Convert.ToString(row["msn"]);
                        string titulo = Convert.ToString(row["titulo"]);

                        EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", autorizador, "", titulo, mensaje, 0);

                    }

                }

                i++;
            }
            return "OK";
        }


        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp
            {
                Timeout = -1
            };
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}