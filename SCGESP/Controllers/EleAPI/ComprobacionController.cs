using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.EleAPI
{
	public class ComprobacionController : ApiController
	{
		public class datos
		{
			public string Usuario { get; set; }
            public string idinforme { get; set; }
            public string comentariosValidacion { get; set; }
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

				SqlCommand comando = new SqlCommand("ObtieneDocumentos");
				comando.CommandType = CommandType.StoredProcedure;

				//Declaracion de parametros
				comando.Parameters.Add("@i_id", SqlDbType.Int);
				comando.Parameters.Add("@Usuario", SqlDbType.VarChar);

				//Asignacion de valores a parametros
				comando.Parameters["@i_id"].Value = Datos.idinforme;
				comando.Parameters["@Usuario"].Value = usuariodesencripta;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
				comando.CommandTimeout = 0;
				comando.Connection.Open();
				//DA.SelectCommand = comando;
				// comando.ExecuteNonQuery();

				DataTable DT = new DataTable();
				SqlDataAdapter DA = new SqlDataAdapter(comando);
				comando.Connection.Close();
				DA.Fill(DT);

				List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

				string ValidacionEnviaComprobantes = "";

				int RmReqGasto = 0;

				if (DT.Rows.Count > 0)
				{
					string g_dirxml;
					string g_dirpdf;
					int g_id;
					int xmlcargado = 0;
					int pdfcargado = 0;
					int otroscargado = 0;
					int idinforme = 0;
					string g_fgasto;
					string g_concepto;
					string g_negocio;
					string g_formaPago;
					int g_categoria;
					double g_total;
					double g_importenodeducible;
					string g_dirotros;
					int g_importenoaceptable;
					string x_uuid;
					int r_idrequisicion;


					foreach (DataRow row in DT.Rows)
					{
						RmReqGasto = Convert.ToInt32(row["RmReqGasto"]);
						g_dirxml = Convert.ToString(row["g_dirxml"]);
						g_dirpdf = Convert.ToString(row["g_dirpdf"]);
						g_id = Convert.ToInt32(row["g_id"]);
						idinforme = Convert.ToInt32(row["i_id"]);
						xmlcargado = Convert.ToInt32(row["xmlcargado"]);
						pdfcargado = Convert.ToInt32(row["pdfcargado"]);
						otroscargado = Convert.ToInt32(row["otroscargado"]);
						g_fgasto = Convert.ToString(row["g_fgasto"]);
						g_concepto = Convert.ToString(row["g_concepto"]);
						g_negocio = Convert.ToString(row["g_negocio"]);
						g_formaPago = Convert.ToString(row["g_formaPago"]);
						g_categoria = Convert.ToInt32(row["g_categoria"]);
						g_total = Convert.ToDouble(row["g_total"]);
						g_importenodeducible = Convert.ToDouble(row["g_importenodeducible"]);
						g_dirotros = Convert.ToString(row["g_dirotros"]);
						g_importenoaceptable = Convert.ToInt32(row["g_importenoaceptable"]);
						x_uuid = Convert.ToString(row["x_uuid"]);
						r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]);

						//string[] valXML = ValidaXML(x_uuid, usuariodesencripta, r_idrequisicion, RmReqGasto, g_id, idinforme);

						if (ValidacionEnviaComprobantes == "")
						{
							int gastoCargado = xmlcargado + pdfcargado + otroscargado; // + Convert.ToInt32(valXML[2]);
							if (gastoCargado == 0)
							{

								//if (valXML[0] == "ENTRA")
								//{
									ValidacionEnviaComprobantes = EnviaComprobantes(g_id, idinforme, RmReqGasto, g_dirpdf, g_dirxml, usuariodesencripta,
																xmlcargado, pdfcargado, g_fgasto, g_concepto, g_negocio, g_formaPago, g_categoria, g_total,
																g_importenodeducible, g_dirotros, g_importenoaceptable, otroscargado);
								//}
								//else
								//{
								//	if (valXML[1] != "")
								//	{
								//		ValidacionEnviaComprobantes = valXML[1];
								//	}
								//}
							//}
							//else {
							//	if (valXML[1] != "")
							//	{
							//		ValidacionEnviaComprobantes = valXML[1];
							//	}
							}
						}
						else
						{
							break;
						}
					}

				}

				if (ValidacionEnviaComprobantes == "")
				{
					string Resultado = DetalleGasto(usuariodesencripta, Convert.ToInt32(Datos.idinforme), RmReqGasto, Datos.comentariosValidacion);
					if (Resultado == "OK")
					{
						return "OK";
					}
					else
					{
						return Resultado;
					}

				}
				else
				{

					return ValidacionEnviaComprobantes;
				}

			}
			catch (Exception ex)
			{
				return ex.ToString();

			}

		}

		public static string[] ValidaXML(string uuid, string Usuario, int idrequisicion, int RmReqGasto, int idgasto, int idinforme)
		{
			string[] respuesta = new string[3];
			DocumentoEntrada entradadoc = new DocumentoEntrada
			{
				Usuario = Usuario,//Variables.usuario;
				Origen = "AdminWEB",
				Transaccion = 120870,
				Operacion = 6
			};
			entradadoc.agregaElemento("FiCfdUuid", uuid); //obtenerlo de la requisición (RmReqGasto)

			DocumentoSalida salida = PeticionCatalogo(entradadoc.Documento);
			int FicfdTipoDocumento = 0;
			int FiCfdNumeroDocumento = 0;
			string FiCfdUuid = "";

			try
			{
				DataTable DTCFDI = new DataTable();

				if (salida.Resultado == "1")
				{
					DTCFDI = salida.obtieneTabla("Llave");
					for (int i = 0; i < DTCFDI.Rows.Count; i++)
					{
						FicfdTipoDocumento = Convert.ToInt32(DTCFDI.Rows[i]["FicfdTipoDocumento"]);
						FiCfdNumeroDocumento = Convert.ToInt32(DTCFDI.Rows[i]["FiCfdNumeroDocumento"]);
						FiCfdUuid = Convert.ToString(DTCFDI.Rows[i]["FiCfdUuid"]);
					}
					string query = "";
					string mensaje = "";
					if (FicfdTipoDocumento == 95)
					{//Gasto
						if (FiCfdNumeroDocumento == RmReqGasto)
						{
							query = "UPDATE gastos SET g_xmlcargado = 1 WHERE g_idinforme = " + idinforme + " AND g_id = '" + idgasto + "'; ";
						}
						else
						{
							mensaje = "El XML (UUID: " + FiCfdUuid + ") cargado ya se encuentra en el Gasto " + FiCfdNumeroDocumento;
						}
					}
					else if (FicfdTipoDocumento == 96)
					{//Recepción
						mensaje = "El XML (UUID: " + FiCfdUuid + ") cargado ya se encuentra en la Recepción " + FiCfdNumeroDocumento;
					}
					else if (FicfdTipoDocumento == 97)
					{//Requisición
						if (FiCfdNumeroDocumento == idrequisicion)
						{
							query = "UPDATE gastos SET g_xmlcargado = 1 WHERE g_idinforme = " + idinforme + " AND g_id = '" + idgasto + "'; ";
						}
						else
						{
							mensaje = "El XML (UUID: " + FiCfdUuid + ") cargado ya se encuentra en la Requisición " + FiCfdNumeroDocumento;
						}
					}

					try
					{
						if (query != "")
						{
							SqlConnection Conexion = new SqlConnection
							{
								ConnectionString = VariablesGlobales.CadenaConexion
							};
							SqlDataAdapter DA;
							DataSet DT = new DataSet();
							DA = new SqlDataAdapter(query, Conexion);
							DA.Fill(DT, "Tabla");

							respuesta[0] = "NO ENTRA";
							respuesta[1] = "";
							respuesta[2] = "1";

							return respuesta;
						}
						else
						{
							if (FicfdTipoDocumento == 0)
							{
								respuesta[0] = "ENTRA";
								respuesta[1] = "";
								respuesta[2] = "0";
							}
							else
							{
								respuesta[0] = "NO ENTRA";
								respuesta[1] = mensaje;
								respuesta[2] = "1";
							}

							return respuesta;
						}
					}
					catch (Exception err)
					{
						respuesta[0] = "ENTRA";
						respuesta[1] = "";
						respuesta[2] = "0";

						return respuesta;
					}

				}
				else
				{
					respuesta[0] = "ENTRA";
					respuesta[1] = "";
					respuesta[2] = "0";
					return respuesta;
				}
			}
			catch (Exception)
			{

				respuesta[0] = "ENTRA";
				respuesta[1] = "";
				respuesta[2] = "0";
				return respuesta;
			}

		}

		public static string EnviaComprobantes(int g_id, int idinforme, int RmReqGasto, string PDF, string XML, string Usuario, int xmlcargado, int pdfcargado, string g_fgasto, string g_concepto, string g_negocio, string g_formaPago, int g_categoria, double g_total, double g_importenodeducible, string g_dirotros, int g_importenoaceptable, int otroscargado)
		{
			string Resultado = "";
			if (xmlcargado == 0 || pdfcargado == 0 || otroscargado == 0)
			{
				string rutacompletaPDF = "";
				string rutacompeltaXML = "";
				String rutacompletaImg = "";

				//path + XML;
				string path = HttpContext.Current.Server.MapPath("/");

				byte[] facturaPdf = null;
				byte[] facturaXml = null; //System.IO.File.ReadAllBytes(rutacompeltaXML);
				byte[] Imgbyte = null;

				if (xmlcargado == 0 && XML != "" && XML != null)
				{
					rutacompeltaXML = path + XML;
					facturaXml = System.IO.File.ReadAllBytes(rutacompeltaXML);
				}
				if (pdfcargado == 0 && PDF != "" && PDF != null)
				{
					rutacompletaPDF = path + PDF;
					facturaPdf = System.IO.File.ReadAllBytes(rutacompletaPDF);
				}

				DocumentoEntrada entradadoc = new DocumentoEntrada();
				entradadoc.Usuario = Usuario;//Variables.usuario;
				entradadoc.Origen = "Programa FiGastoFactura";
				entradadoc.Transaccion = 120092;
				entradadoc.Operacion = 9;
				entradadoc.agregaElemento("FiGfaGasto", Convert.ToString(RmReqGasto)); //obtenerlo de la requisición (RmReqGasto)

				try
				{
					entradadoc.agregaElemento("FiGfaFecha", Convert.ToDateTime(g_fgasto).ToString("dd/MM/yyyy")); //obtenerlo de la requisición (RmReqGasto)

				}
				catch (Exception)
				{

				}

				entradadoc.agregaElemento("FiGfaProveedor", g_negocio); //Negocio
				entradadoc.agregaElemento("FiGfaMaterial", g_categoria); //IdMaterial
				entradadoc.agregaElemento("FiGfaConcepto", g_concepto); //Concepto
				entradadoc.agregaElemento("FiGfaImporte", g_total); //Importe
				entradadoc.agregaElemento("FiGfaImporteNoDeducible", g_importenodeducible); //Importe no deducible                
				entradadoc.agregaElemento("FiGfaImporteNoAceptable", g_importenoaceptable);

				var ruta = "";

				try
				{
					if (g_dirotros != "" && g_dirotros != null)
					{
						rutacompletaImg = path + g_dirotros;
						Imgbyte = System.IO.File.ReadAllBytes(g_dirotros);

						int index = ruta.IndexOf(".");
						entradadoc.agregaElemento("extension", (g_dirotros.Substring(index, 3)));
						entradadoc.agregaElemento("FiGfaImagen", Convert.ToBase64String(Imgbyte));
						otroscargado = 1;
					}

				}
				catch (Exception ex)
				{
					otroscargado = 0;

				}


				if (xmlcargado == 0 && XML != "" && XML != null)
				{
					entradadoc.agregaElemento("FiGfaXml", Convert.ToBase64String(facturaXml));
				}

				if (pdfcargado == 0 && PDF != "" && PDF != null)
				{
					entradadoc.agregaElemento("FiGfaPdf", Convert.ToBase64String(facturaPdf));
				}

				DocumentoSalida salida = PeticionCatalogo(entradadoc.Documento);
				if (salida.Resultado != "1")
				{
					foreach (XmlNode error in salida.Errores)
					{

						Resultado = error.InnerText + " - Gasto numero " + Convert.ToString(g_id) + " " + g_fgasto;
					}

				}
				else
				{
					string query = "";
					if (xmlcargado == 0 && idinforme > 0)
						query = "UPDATE gastos SET g_xmlcargado = 1 WHERE g_idinforme = " + idinforme + " AND g_dirxml = '" + XML + "'; ";
					if (pdfcargado == 0 && idinforme > 0)
						query = query + "UPDATE gastos SET g_pdfcargado = 1 WHERE g_idinforme = " + idinforme + " AND g_dirpdf = '" + PDF + "'; ";
					if (otroscargado == 1)
						query = query + "UPDATE gastos SET g_otroscargado = 1 WHERE g_idinforme = " + idinforme + " AND g_dirpdf = '" + g_dirotros + "'; ";

					if (query != "")
					{
						try
						{
							SqlConnection Conexion = new SqlConnection
							{
								ConnectionString = VariablesGlobales.CadenaConexion
							};
							SqlDataAdapter DA;
							DataSet DT = new DataSet();
							DA = new SqlDataAdapter(query, Conexion);
							DA.Fill(DT, "Tabla");
							Resultado = "";

						}
						catch (Exception err)
						{
							Resultado = Convert.ToString(err);
						}
					}
				}
			}

			return Resultado;
		}

		public static string DetalleGasto(string usuario, int _informe, int _RmReqGasto, string comentariosValidacion)
		{
			string resultado = null;

			DocumentoEntrada entradaComp = new DocumentoEntrada();

			entradaComp.Usuario = usuario;
			entradaComp.Origen = "CGE";
			entradaComp.Transaccion = 120090;
			entradaComp.Operacion = 21;

			entradaComp.agregaElemento("FiGasId", _RmReqGasto);


			DocumentoSalida salida = PeticionCatalogo(entradaComp.Documento);
			if (salida.Resultado == "1")
			{
				AutorizaInformeFinal(_informe, usuario, comentariosValidacion);

				resultado = "OK";
			}
			else
			{
				resultado = salida.Errores.InnerText + " - " + Convert.ToString(_RmReqGasto);
			}


			return resultado;

		}

		public static string AutorizaInformeFinal(int informe, string usuario, string comentariosValidacion)
		{
			SqlCommand comando1 = new SqlCommand("TerminaInforme");
			comando1.CommandType = CommandType.StoredProcedure;

			//Declaracion de parametros
			comando1.Parameters.Add("@i_id", SqlDbType.Int);
			comando1.Parameters.Add("@Usuario", SqlDbType.VarChar);
            comando1.Parameters.Add("@comentariosValidacion", SqlDbType.VarChar);

            //Asignacion de valores a parametros
            comando1.Parameters["@i_id"].Value = informe;
			comando1.Parameters["@Usuario"].Value = usuario;
            comando1.Parameters["@comentariosValidacion"].Value = comentariosValidacion;

            comando1.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
			comando1.CommandTimeout = 0;
			comando1.Connection.Open();
			//DA.SelectCommand = comando;
			// comando.ExecuteNonQuery();

			DataTable DT1 = new DataTable();
			SqlDataAdapter DA1 = new SqlDataAdapter(comando1);
			comando1.Connection.Close();
			DA1.Fill(DT1);

			if (DT1.Rows.Count > 0)
			{
				return "OK";
			}
			else
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

