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
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI
{
    public class EliminaComprobantesController : ApiController
    {
        public class ParametrosGastos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
            public string comprobante { get; set; }
        }

        public string Post(ParametrosGastos Datos)
        {
           // string Ruta = "";
           //// Ruta = PostSave(Datos.dir);
           // double Total = 0;
           // string Emisor = "";
           // string Receptor = "";
           // string UUID = "";
           // string Formapago = "";

           // if (Ruta != "")
           // {
           //     string path = HttpContext.Current.Server.MapPath("/");
           // }

            SqlCommand comando = new SqlCommand("DeleteComprobantes");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@comprobante", SqlDbType.VarChar);
            

            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.id;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@comprobante"].Value = Datos.comprobante;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            //comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //aqui verificar en base al mensaje,que es lo que se va regresar
            if (DT.Rows.Count > 0)
            {

                //@g_dirxml as RutaXML,@g_dirpdf as RutaPDF ,@g_dirotros as RutaComp

                DataRow row = DT.Rows[0];

                var RutaXML = row[0].ToString();
                var RutaPDF = row[1].ToString();
				var RutaComprobantes = row[2].ToString();
				string UUID = row[3].ToString();
				int idGasto = Convert.ToInt16(row[4].ToString());
				string uResponsable = row[5].ToString();

				if (Datos.comprobante == "OTRO")
                {
                    Deletexml(RutaComprobantes);
                }

                if (Datos.comprobante == "PDF")
                {
                    Deletexml(RutaPDF);
                }

                if (Datos.comprobante == "XML")
                {
					if (UUID.Trim() != "") {
						try
						{
							DocumentoEntrada entradadoc = new DocumentoEntrada
							{
								Usuario = uResponsable,//Variables.usuario;
								Origen = "AdminWEB",
								Transaccion = 120092,
								Operacion = 22
							};//21:Agregar XML, 22:Eliminar XML
							entradadoc.agregaElemento("FiGfaGasto", idGasto);
							entradadoc.agregaElemento("FiGfaUuid", UUID);

							DocumentoSalida respuesta = PeticionCatalogo(entradadoc.Documento);
						}
						catch (Exception)
						{

							throw;
						}
					}

                    Deletexml(RutaXML);
                }

                return "";
            }
            else
            {
                //Deletexml(Ruta);

                return null;
            }
        }

        public string Deletexml(string RutaXml)
        {

            try
            {
                string path = HttpContext.Current.Server.MapPath(RutaXml);

                string str = path.Replace("\\api", " ");

                var validaruta = File.Exists(str);

                if (validaruta == Convert.ToBoolean(1))
                {
                    File.Delete(str);

                    return "OK";
                }
                else
                {

                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
		public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp();
			ws.Timeout = -1;
			string respuesta = ws.PeticionCatalogo(doc);
			return new DocumentoSalida(respuesta);
		}

	}
}

