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
	public class UpdateGastoXMLController : ApiController
	{
		public class ParametrosGastos
		{
			public int id { get; set; }
			public int idinforme { get; set; }
			public string dir { get; set; }
			public string Usuario { get; set; }

		}

		public string Post(ParametrosGastos Datos)
		{
			string Ruta = "";
			Ruta = PostSave(Datos.dir);
			double Total = 0;
			string Emisor = "";
			string Receptor = "";
			string UUID = "";
			string Formapago = "";
			string TipoDeComprobante = "";
			string Folio = "";
			string Serie = "";
			string NmbEmisor = "";

			if (Ruta != "")
			{

				string path = HttpContext.Current.Server.MapPath("/");

				XmlTextReader reader = new XmlTextReader(path + Ruta);

				while (reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element: // The node is an element.

							if (reader.Name == "cfdi:Receptor")
							{
								while (reader.MoveToNextAttribute()) // Read the attributes.

									if (reader.Name == "rfc" || reader.Name == "Rfc")
									{
										Receptor = Convert.ToString(reader.Value);
									}
							}

							if (reader.Name == "cfdi:Emisor")
							{
								while (reader.MoveToNextAttribute())
								{ // Read the attributes.

									if (reader.Name == "rfc" || reader.Name == "Rfc")
									{
										Emisor = Convert.ToString(reader.Value);
									}
									if (reader.Name == "Nombre" || reader.Name == "nombre")
									{
										NmbEmisor = Convert.ToString(reader.Value);
									}
								}
							}

							if (reader.Name == "tfd:TimbreFiscalDigital")
							{
								while (reader.MoveToNextAttribute()) // Read the attributes

									if (reader.Name == "uuid" || reader.Name == "UUID")
									{
										UUID = Convert.ToString(reader.Value);
									}
							}

							if (reader.Name == "cfdi:Comprobante")
							{
								while (reader.MoveToNextAttribute())
								{ // Read the attributes

									if (reader.Name == "total" || reader.Name == "Total")
									{
										Total = Convert.ToDouble(reader.Value);
									}
									else
									{
										if (reader.Name == "FormaPago" || reader.Name == "FormaPago")
										{
											Formapago = Convert.ToString(reader.Value);
										}
									}
									if (reader.Name == "serie" || reader.Name == "Serie")
									{
										Serie = Convert.ToString(reader.Value);
									}
									if (reader.Name == "folio" || reader.Name == "Folio")
									{
										Folio = Convert.ToString(reader.Value);
									}
								}

							}

							while (reader.MoveToNextAttribute())
							{ // Read the attributes.

								if (reader.Name == "total" || reader.Name == "Total")
								{
									Total = Convert.ToDouble(reader.Value);
								}
								if (reader.Name == "emisor")
								{
									Emisor = Convert.ToString(reader.Value);
								}
								if (reader.Name == "receptor")
								{
									Receptor = Convert.ToString(reader.Value);
								}
								if (reader.Name == "uuid" || reader.Name == "UUID")
								{
									UUID = Convert.ToString(reader.Value);
								}
								if (reader.Name == "FormaPago")
								{
									Formapago = Convert.ToString(reader.Value);
								}
								if (reader.Name == "TipoDeComprobante")
								{
									TipoDeComprobante = Convert.ToString(reader.Value);
								}
								if (reader.Name == "Folio" && Folio.Trim() == "")
								{
									Folio = Convert.ToString(reader.Value);
								}
								if (reader.Name == "Serie" && Serie.Trim() == "")
								{
									Serie = Convert.ToString(reader.Value);
								}
							}
							break;
					}
				}

			}

			if (string.IsNullOrEmpty(UUID))
			{
				Deletexml(Ruta);

				return "XML invalido";
			}
			//validar xml en AdminERP
			string usuarioxml = "";
			int idrequisicion = 0;
			int idgasto = 0;
			SqlDataAdapter DA1;
			DataTable DT1 = new DataTable();

			SqlConnection Conexion = new SqlConnection
			{
				ConnectionString = VariablesGlobales.CadenaConexion
			};
			string consulta = "SELECT r_idrequisicion, r_idgasto, i_uresponsable FROM informe WHERE i_id = " + Datos.idinforme + "; ";

			DA1 = new SqlDataAdapter(consulta, Conexion);
			DA1.Fill(DT1);

			if (DT1.Rows.Count > 0)
			{
				// DataRow row = DT.Rows[0];
				foreach (DataRow row in DT1.Rows)
				{
					usuarioxml = Convert.ToString(row["i_uresponsable"]);
					idrequisicion = Convert.ToInt32(row["r_idrequisicion"]);
					idgasto = Convert.ToInt32(row["r_idgasto"]);
				}
			}
			else
			{
				usuarioxml = "fjsolis";
			}

			DocumentoEntrada entradadoc = new DocumentoEntrada
			{
				Usuario = usuarioxml,//Variables.usuario;
				Origen = "AdminWEB",
				Transaccion = 120870,
				Operacion = 6
			};
			entradadoc.agregaElemento("FiCfdUuid", UUID);

			DocumentoSalida respuesta = PeticionCatalogo(entradadoc.Documento);

			int FicfdTipoDocumento = 0;
			int FiCfdNumeroDocumento = 0;
			string FiCfdUuid = "";

			try
			{
				DataTable DTCFDI = new DataTable();

				if (respuesta.Resultado == "1")
				{
					DTCFDI = respuesta.obtieneTabla("Llave");
					for (int i = 0; i < DTCFDI.Rows.Count; i++)
					{
						FicfdTipoDocumento = Convert.ToInt32(DTCFDI.Rows[i]["FicfdTipoDocumento"]);
						FiCfdNumeroDocumento = Convert.ToInt32(DTCFDI.Rows[i]["FiCfdNumeroDocumento"]);
						FiCfdUuid = Convert.ToString(DTCFDI.Rows[i]["FiCfdUuid"]);
					}
				}
			}
			catch (Exception)
			{

				throw;
			}

			if (FicfdTipoDocumento > 0 && FiCfdNumeroDocumento > 0) {
				Deletexml(Ruta);

				return "El XML No se puede cargar, el comprobante " + FiCfdUuid + " ya existe en AdminERP. Documento : " + FiCfdNumeroDocumento.ToString();
			}
			//FIN valida xml en AdminERP


			SqlCommand comando = new SqlCommand("UpdateGastoXML");
			comando.CommandType = CommandType.StoredProcedure;

			//Declaracion de parametros
			comando.Parameters.Add("@id", SqlDbType.Int);
			comando.Parameters.Add("@idinforme", SqlDbType.Int);
			comando.Parameters.Add("@dir", SqlDbType.VarChar);
			comando.Parameters.Add("@Monto", SqlDbType.Float);
			comando.Parameters.Add("@Emisor", SqlDbType.VarChar);
			comando.Parameters.Add("@Receptor", SqlDbType.VarChar);
			comando.Parameters.Add("@UUID", SqlDbType.VarChar);
			comando.Parameters.Add("@FormaPago", SqlDbType.VarChar);
			comando.Parameters.Add("@TipoDeComprobante", SqlDbType.VarChar);
			comando.Parameters.Add("@NmbEmisor", SqlDbType.VarChar);
			comando.Parameters.Add("@Serie", SqlDbType.VarChar);
			comando.Parameters.Add("@Folio", SqlDbType.VarChar);

			//Asignacion de valores a parametros
			comando.Parameters["@id"].Value = Datos.id;
			comando.Parameters["@idinforme"].Value = Datos.idinforme;
			comando.Parameters["@dir"].Value = Ruta;
			comando.Parameters["@Monto"].Value = Total;
			comando.Parameters["@Emisor"].Value = Emisor;
			comando.Parameters["@Receptor"].Value = Receptor;
			comando.Parameters["@UUID"].Value = UUID;
			comando.Parameters["@FormaPago"].Value = Formapago;
			comando.Parameters["@TipoDeComprobante"].Value = TipoDeComprobante.ToUpper();
			comando.Parameters["@NmbEmisor"].Value = NmbEmisor;
			comando.Parameters["@Serie"].Value = Serie;
			comando.Parameters["@Folio"].Value = Folio;

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
				DataRow row = DT.Rows[0];

				string Mensaje = row[0].ToString();

				//if (Mensaje == "Gasto Actualizado")
				//{

				//}
				if (Mensaje == "No se puede guardar el comprobante, el importe es igual o mayor a $ 2000.00 y la forma de pago es efectivo.")
				{
					Deletexml(Ruta);
				}
				if (Mensaje == "El UUID ingresado ya existe")
				{
					Deletexml(Ruta);
				}

				return Mensaje;
			}
			else
			{
				Deletexml(Ruta);

				return null;
			}
		}

		public string PostSave(string Based64BinaryString)
		{
			string result = "";
			try
			{
				string format = "";
				string path = HttpContext.Current.Server.MapPath("/XML/");
				string name = DateTime.Now.ToString("yyyyMMddhhmmss");


				if (Based64BinaryString.Contains("data:text/xml;base64,"))
				{
					format = "xml";
				}

				string str = Based64BinaryString.Replace("data:text/xml;base64,", " ");//jpg check

				byte[] data = Convert.FromBase64String(str);

				MemoryStream ms = new MemoryStream(data, 0, data.Length);
				ms.Write(data, 0, data.Length);
				//System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
				string rutacompleta = path + name + "." + format;
				File.WriteAllBytes(rutacompleta, data);
				//image.Save(rutacompleta);
				result = rutacompleta;
				result = "XML/" + name + "." + format;

			}
			catch (Exception ex)
			{
				result = "Error : " + ex;
			}
			return result;
		}

		public string Deletexml(string RutaXml)
		{

			try
			{
				if (File.Exists(RutaXml) == Convert.ToBoolean(1))
				{
					File.Delete(RutaXml);

					return "OK";
				}
				else
				{

					return "Error";
				}
			}
			catch (Exception)
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

