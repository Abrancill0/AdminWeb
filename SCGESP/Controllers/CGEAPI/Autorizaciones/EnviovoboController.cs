using System;
using Ele.Generales;
using SCGESP.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI.Autorizaciones
{
    public class EnviovoboController : ApiController
    {
        public class ParametrosVOBO
        {
            public string @usuarioActual { get; set; }
            public string @usuariovobo { get; set; }
            public string @idinforme { get; set; }
            public string @comentariosValidacion { get; set; }            
        }


        public string Post(ParametrosVOBO Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("EnviaVoBo");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@usuarioActual", SqlDbType.VarChar);
                comando.Parameters.Add("@usuariovobo", SqlDbType.VarChar);
                comando.Parameters.Add("@idinforme", SqlDbType.VarChar);
                comando.Parameters.Add("@comentariosValidacion", SqlDbType.VarChar);

                string ususariodesencripta = Seguridad.DesEncriptar(Datos.usuarioActual);

                //Asignacion de valores a parametros
                comando.Parameters["@usuarioActual"].Value = ususariodesencripta;
                comando.Parameters["@usuariovobo"].Value = Datos.usuariovobo;
                comando.Parameters["@idinforme"].Value = Datos.idinforme;
                comando.Parameters["@comentariosValidacion"].Value = Datos.comentariosValidacion;


                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

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
                        string usuarioActual = Convert.ToString(row["usuarioActual"]);
                        string usuariovobo = Convert.ToString(row["usuariovobo"]);
                        string correo = Convert.ToString(row["correo"]);

						//Mensaje(usuariovobo);

						EnvioCorreosELE.Envio(usuarioActual, correo, "", usuariovobo, "", titulo, mensaje, 0);

                    }

                    return "OK";
                }
                else
                {
                    return "Error";
                }
            }
            catch (System.Exception ex)
            {

                return ex.ToString();
            }
          

          
        }

		public static string Mensaje(string usuario_destino)
		{

			try
			{
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
				string nombre = "";

				if (respuesta.Resultado == "1")
				{
					DTCorreo = respuesta.obtieneTabla("Llave");

					for (int i = 0; i < DTCorreo.Rows.Count; i++)
					{
						nombre = Convert.ToString(DTCorreo.Rows[i]["SgUsuNombre"]);
					}

					return "";
				}
				else
				{
					return "";
				}
			}
			catch (Exception)
			{

				return "";
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
