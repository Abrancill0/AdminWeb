using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;


namespace SCGESP.Controllers.EleAPI
{
    public class AutorizaInformeController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string idinforme { get; set; }
			public string idrequisicion { get; set; }
			public string comentario { get; set; }
			public string comentario_respuesta { get; set; }
			public string usuario_fecha_responde { get; set; }
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
            try
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
                        int autorizador_final = Convert.ToInt16(row["a_autorizador_final"]);
                        if (autorizador_final == 1)
                        {
                            titulo = "Respuesta de solicitud para autorización para el descuento vía nómina de la requisición #" + Datos.idrequisicion;
                            string body_mensaje = Mensaje(autorizador, Datos.idrequisicion, Datos.comentario_respuesta, Datos.usuario_fecha_responde);
                            EnvioCorreosELE.Envio(usuariodesencripta, "", "", autorizador, "", titulo, body_mensaje, 0);
                        }
                        else
                        {
                            EnvioCorreosELE.Envio(usuariodesencripta, "", "", autorizador, "", titulo, mensaje, 0);
                        }

                    }

                }

                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

                //if (DT.Rows.Count > 0)
                //{ }
                return "";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

            
        }

		public static string Mensaje(string usuario_destino, string idrequisicion, string comentario, string usuario_responde)
		{
			string mensaje = "Buen día estimado, ";
			mensaje += "En respuesta a tu petición de la requisición ";
			mensaje += idrequisicion + " la cual se encuentra fuera de políticas, te comento que: ";
			mensaje += comentario;
			try
			{
				string msn = "";
				string nombre = "";
				DocumentoEntrada entrada = new DocumentoEntrada
				{
					Usuario = usuario_destino,
					Origen = "Programa CGE",  //Datos.Origen; 
					Transaccion = 100004,
					Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
				};
				entrada.agregaElemento("SgUsuId", usuario_destino);

				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

				DataTable DTCorreo = new DataTable();

				if (respuesta.Resultado == "1")
				{
					DTCorreo = respuesta.obtieneTabla("Llave");

					for (int i = 0; i < DTCorreo.Rows.Count; i++)
					{
						nombre = Convert.ToString(DTCorreo.Rows[i]["SgUsuNombre"]);
					}
					
					msn = "Buen día estimado " + nombre;
					msn += "<br />";
					msn += "<br />";
					msn += "En respuesta a tu petición de la requisición ";
					msn += "<b><u>&nbsp;" + idrequisicion + "&nbsp;</u></b>";
					msn += ", la cual se encuentra fuera de políticas, te comento que: <br />";
					msn += "<b><i>" + comentario + "</i></b><br />";
					msn += "<br />";
					msn += "Saludos cordiales";
					msn += "<br /><br />";
					msn += (usuario_responde.Replace("/ ","(") + ")");
					return msn;
				}
				else
				{
					return mensaje;
				}
			}
			catch (Exception)
			{

				return mensaje;
			}

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
