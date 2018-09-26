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

                if (DT.Rows.Count > 0 && i == 2)
                {

                    foreach (DataRow row in DT.Rows)
                    {
                        string mensaje = Convert.ToString(row["msn"]);
                        string titulo = Convert.ToString(row["titulo"]);
						int idrequisicion = Convert.ToInt32(row["idrequisicion"]);
						string responsable = Convert.ToString(row["responsable"]);

						//mensaje
						string body_mensaje = Mensaje(autorizador, mensaje, idrequisicion, responsable, Datos.comentario);

						EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", autorizador, "", titulo, body_mensaje, 0);//mensaje

					}

                }

                i++;
            }
            return "OK";
        }

		public static string Mensaje(string usuario_destino, string mensaje, int idrequisicion, string responsable, string comentario)
		{
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

					string buscar_str = "(Enviado por:";
					int pos_str = comentario.IndexOf(buscar_str);
					string body_str = comentario.Substring(0, pos_str);
					msn = "Buen día estimado " + nombre;
					msn += "<br />";
					msn += "<br />";
					msn += "Solicito por favor tu autorización para el descuento vía nómina de la requisición ";
					msn += "<b><u>&nbsp;" + idrequisicion.ToString() + "&nbsp;</u></b>";
					msn += ", la cual se encuentra fuera de políticas debido a la siguiente falta: <br />";
					msn += "<b><i>" + body_str + "</i></b><br />";
					msn += "<br />";
					msn += "Empleado que salió de políticas: <b><u>&nbsp;" + responsable + "&nbsp;</u></b><br />";
					msn += "<br />";
					msn += "Por favor ingresar a la siguiente liga con tu usuario y contraseña, ";
					msn += "<a href='https://gapp.elpotosi.com.mx'>&nbsp;https://gapp.elpotosi.com.mx&nbsp;</a> ";
					msn += "(no utilizar internet Explorer), da clic en la opción de &quot;Autorizaciones de Requisiciones por comprobar&quot;, posteriormente en la opción de &quot;Ver&quot; e ingresa tu comentario en la opción de &quot;Autorizar&quot;";
					msn += "<br /><br />";
					msn += "Saludos cordiales";
					msn += "<br />";
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