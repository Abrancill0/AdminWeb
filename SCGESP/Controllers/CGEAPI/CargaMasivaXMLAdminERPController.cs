using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace SCGESP.Controllers.CGEAPI
{
    public class CargaMasivaXMLAdminERPController : ApiController
    {
		public class Parametros
		{
			public string Usuario { get; set; }
		}
		public XmlDocument Post(Parametros Datos)
		{
			SqlDataAdapter DA;
			DataTable DT = new DataTable();

			SqlConnection Conexion = new SqlConnection
			{
				ConnectionString = VariablesGlobales.CadenaConexion
			};
			string consulta = "SELECT i_id, r_idrequisicion, r_idgasto, i_ninforme, i_nmb, i_uresponsable, " + 
								"i_nombreresponsabe, i_estatus, i_rechazado, " +
								"g_id, g_nombreCategoria, g_concepto, " +
								"x_nmbemisor, x_uuid, x_urlxml, x_fcreo " +
							"FROM informe i INNER JOIN " +
								"gastos g ON g_idinforme = i_id INNER JOIN " +
								"xmlinforme x ON x_idinforme = i_id AND x_idgasto = g_id " +
							"WHERE i_estatus <= 3 AND MONTH(i_fcrea) >= 7; ";

			DA = new SqlDataAdapter(consulta, Conexion);
			DA.Fill(DT);
			int i_id, r_idrequisicion, r_idgasto, i_ninforme, i_estatus, i_rechazado, g_id;
			string i_nmb, i_uresponsable, i_nombreresponsabe, g_nombreCategoria, g_concepto, x_nmbemisor, x_uuid, x_urlxml, x_fcreo;
			string resultXML = "", estatus = "", fecha_actual = "", msnError = "";
			XmlDocument xmlRespuesta = new XmlDocument();
			DocumentoSalida respuesta;
			DocumentoEntrada entradadoc;
			if (DT.Rows.Count > 0)
			{
				foreach (DataRow row in DT.Rows)
				{//
					i_id = Convert.ToInt16(row["i_id"]);
					r_idrequisicion = Convert.ToInt16(row["r_idrequisicion"]);
					r_idgasto = Convert.ToInt16(row["r_idgasto"]);
					i_ninforme = Convert.ToInt16(row["i_ninforme"]);
					i_nmb = Convert.ToString(row["i_nmb"]);
					i_uresponsable = Convert.ToString(row["i_uresponsable"]);
					i_nombreresponsabe = Convert.ToString(row["i_nombreresponsabe"]);
					i_estatus = Convert.ToInt16(row["i_estatus"]);
					i_rechazado = Convert.ToInt16(row["i_rechazado"]);
					if (i_estatus == 2)
						estatus = "Activo (Captura de gastos)";
					else if (i_estatus == 3)
						estatus = "Autorización (Por enviar a AdminERP)";
					if (i_rechazado == 1)
						estatus = "Rechazado";

					g_id = Convert.ToInt16(row["g_id"]);
					g_nombreCategoria = Convert.ToString(row["g_nombreCategoria"]);
					g_concepto = Convert.ToString(row["g_concepto"]);
					x_nmbemisor = Convert.ToString(row["x_nmbemisor"]);
					x_uuid = Convert.ToString(row["x_uuid"]);
					x_urlxml = Convert.ToString(row["x_urlxml"]);
					x_fcreo = Convert.ToString(row["x_fcreo"]);
					resultXML += "<xml>";
					resultXML += "<idInforme>" + i_id + "</idInforme>" +
						"<idRequisicion>" + r_idrequisicion + "</idRequisicion>" +
						"<idGastoAdminERP>" + r_idgasto + "</idGastoAdminERP>" +
						"<nInforme>" + i_ninforme + "</nInforme>" +
						"<jInforme>" + i_nmb + "</jInforme>" +
						"<uResponsable>" + i_uresponsable + "</uResponsable>" +
						"<nmbResponsabe>" + i_nombreresponsabe + "</nmbResponsabe>" +
						"<estatus>" + estatus + "</estatus>" +
						"<idGasto>" + g_id + "</idGasto>" +
						"<nmbCategoria>" + g_nombreCategoria + "</nmbCategoria>" +
						"<jGasto>" + g_concepto + "</jGasto>" +
						"<nmbEmisor>" + x_nmbemisor + "</nmbEmisor>" +
						"<uuid>" + x_uuid + "</uuid>" +
						"<fCargaAdminWEB>" + x_fcreo + "</fCargaAdminWEB>" +
						"<ubicacion>" + x_urlxml + "</ubicacion>";
					if (r_idgasto > 0)
					{

						Datos.Usuario = i_uresponsable.Trim() == "" ? Datos.Usuario : i_uresponsable.Trim();

						try
						{
							entradadoc = new DocumentoEntrada
							{
								Usuario = Datos.Usuario,//Variables.usuario;
								Origen = "AdminWEB",
								Transaccion = 120092,
								Operacion = 21
							};//21:Agregar XML, 22:Eliminar XML
							entradadoc.agregaElemento("FiGfaGasto", r_idgasto);
							entradadoc.agregaElemento("FiGfaUuid", x_uuid);
							respuesta = PeticionCatalogo(entradadoc.Documento);
							if (respuesta.Resultado == "0")
							{
								msnError = "";

								XmlDocument xmErrores = new XmlDocument();
								xmErrores.LoadXml(respuesta.Errores.InnerXml);

								XmlNodeList elemList = xmErrores.GetElementsByTagName("Descripcion");
								for (int i = 0; i < elemList.Count; i++)
								{
									msnError += elemList[i].InnerXml;
								}
								fecha_actual = DateTime.Now.ToString("G");
								resultXML += "<cargado>NO</cargado>" +
								"<dCarga>UUID NO cargado. " + msnError + "</dCarga>" +
								"<fecha>" + fecha_actual + "</fecha>";
							}
							else
							{
								fecha_actual = DateTime.Now.ToString("G");
								resultXML += "<cargado>SI</cargado>" +
								"<dCarga>UUID agregado a AdminERP</dCarga>" +
								"<fecha>" + fecha_actual + "</fecha>";
							}
						}
						catch (Exception ex)
						{
							fecha_actual = DateTime.Now.ToString("G");
							resultXML += "<cargado>NO</cargado>" +
							"<dCarga>Error al cargar. Exception: " + ex.ToString() + "</dCarga>" +
							"<fecha>" + fecha_actual + "</fecha>";
						}
					}
					else
					{
						fecha_actual = DateTime.Now.ToString("G");
						resultXML += "<cargado>NO</cargado>" +
						"<dCarga>Se requiere número de gasto para cargar el UUID</dCarga>" +
						"<fecha>" + fecha_actual + "</fecha>";
					}
					resultXML += "</xml>";
				}
				resultXML = "<xmls>" + resultXML + "</xmls>";
				
				xmlRespuesta.LoadXml(resultXML);
			}
				
				return xmlRespuesta;
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
