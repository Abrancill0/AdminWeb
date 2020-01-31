using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using System.Web.Http;
using System.IO;

namespace SCGESP.Controllers.CGEAPI
{
    public class AgregarAjusteGastoController : ApiController
    {
        public class Parametros
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public string FGasto { get; set; }
            public string HGasto { get; set; }
            public string Concepto { get; set; }
            public string Negocio { get; set; }
            public string UGasto { get; set; }
            public string UCrea { get; set; }
            public string FormaPago { get; set; }
            public int IdCategoria { get; set; }
            public string Categoria { get; set; }
            public double IvaCategoria { get; set; }
            public double TGastado { get; set; }
            public double TComprobar { get; set; }
            public string Observaciones { get; set; }
            public int ConciliacionBanco { get; set; }
            public int IdMovBanco { get; set; }
            public string BinXML { get; set; }
            public string NombreArc { get; set; }
            public string ExtFile { get; set; }
            public int Tipo { get; set; }
            public int AfectaImpGastado { get; set; }
            public int AfectaImpComprobado { get; set; }
        }

        public class ListResult
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public int IdGastoOrigen { get; set; }
            public bool AgregadoOk { get; set; }
            public string Descripcion { get; set; }
        }
        public class ListResultArchivo
        {
            public bool ArchivoOk { get; set; }
            public string Descripcion { get; set; }
            public string Ruta { get; set; }
        }
        public class ListResultCFDIXML
        {
            public double Total { get; set; }
            public string Emisor { get; set; }
            public string Receptor { get; set; }
            public string UUID { get; set; }
            public string Formapago { get; set; }
            public string TipoDeComprobante { get; set; }
            public string Folio { get; set; }
            public string Serie { get; set; }
            public string NmbEmisor { get; set; }
        }

        public ListResult PostAgregaAjuste(Parametros Datos)
        {

            ListResult resultado = new ListResult();
            try
            {
                ListResultArchivo archivo = new ListResultArchivo();
                ListResultCFDIXML ArcCdfi = new ListResultCFDIXML();
				string path = HttpContext.Current.Server.MapPath("/");
				if (Datos.Tipo == 2)
                {
                    archivo = GuardarXmlAdicional(Datos.IdInforme, Datos.IdGasto, Datos.BinXML, Datos.ExtFile);

                    if (archivo.ArchivoOk == true)
                    {
                        string urlXML = path + (archivo.Ruta ?? "");
                        
                        ArcCdfi = LeerCFDIXML(urlXML);

                        if (Datos.AfectaImpGastado == 1)
                        {
                            Datos.TGastado = ArcCdfi.Total;
                        }
                        else {
                            Datos.TGastado = 0;
                        }
                        
                        if(Datos.AfectaImpComprobado == 1)
                        {
                            Datos.TComprobar = ArcCdfi.Total;
                        }
                        else
                        {
                            Datos.TComprobar = 0;
                        }
                    }
                    else
                    {
                        resultado.AgregadoOk = false;
                        resultado.Descripcion = (Datos.Tipo == 1 ? "Gasto Adicinal / Propina NO Agregada al Gasto." : "Comprobante (CFDI) adicional NO agregado. ") + (archivo.Descripcion ?? " Error al cargar archivo");
                        resultado.IdInforme = Datos.IdInforme;
                        resultado.IdGasto = Datos.IdGasto;
                        resultado.IdGastoOrigen = Datos.IdGasto;
                    }

                }

                string UCrea = Seguridad.DesEncriptar(Datos.UCrea);

                SqlCommand comando = new SqlCommand("AgregarAjusteGasto")
                {
                    CommandType = CommandType.StoredProcedure
                };
                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters.Add("@idgastoorigen", SqlDbType.Int);
                comando.Parameters.Add("@fgasto", SqlDbType.Date);
                comando.Parameters.Add("@hgasto", SqlDbType.VarChar);
                comando.Parameters.Add("@concepto", SqlDbType.VarChar);
                comando.Parameters.Add("@negocio", SqlDbType.VarChar);
                comando.Parameters.Add("@ugasto", SqlDbType.VarChar);
                comando.Parameters.Add("@ucrea", SqlDbType.VarChar);
                comando.Parameters.Add("@formapago", SqlDbType.VarChar);
                comando.Parameters.Add("@idcategoria", SqlDbType.Int);
                comando.Parameters.Add("@categoria", SqlDbType.VarChar);
                comando.Parameters.Add("@ivacategoria", SqlDbType.Decimal);
                comando.Parameters.Add("@tgastado", SqlDbType.Decimal);
                comando.Parameters.Add("@tcomprobar", SqlDbType.Decimal);
                comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
                comando.Parameters.Add("@conciliacionbanco", SqlDbType.Int);
                comando.Parameters.Add("@idmovbanco", SqlDbType.Int);
                comando.Parameters.Add("@dirxml", SqlDbType.VarChar);
                comando.Parameters.Add("@tipoajuste", SqlDbType.Int);
                comando.Parameters.Add("@emisor", SqlDbType.VarChar);
                comando.Parameters.Add("@receptor", SqlDbType.VarChar);
                comando.Parameters.Add("@uuid", SqlDbType.VarChar);
                comando.Parameters.Add("@formapagocfdi", SqlDbType.VarChar);
                comando.Parameters.Add("@tipodecomprobante", SqlDbType.VarChar);
                comando.Parameters.Add("@nmbemisor", SqlDbType.VarChar);
                comando.Parameters.Add("@serie", SqlDbType.VarChar);
                comando.Parameters.Add("@folio", SqlDbType.VarChar);

                //Asignacion de valores a parametros

                string day = Datos.FGasto.Substring(0, 2);
                string month = Datos.FGasto.Substring(3, 2);
                string year = Datos.FGasto.Substring(6, 4);

                DateTime Fecha;

                try
                {
                    Fecha = Convert.ToDateTime(year + "-" + month + "-" + day);
                }
                catch (Exception)
                {
                    Fecha = Convert.ToDateTime(day + "-" + month + "-" + year);
                }

                string concepto = Datos.Concepto;
                string serie = "", folio = "", serieFolio = "";
                if (Datos.Tipo == 2) {
                    serie = ArcCdfi.Serie ?? "";
                    folio = ArcCdfi.Folio ?? "";

                    serieFolio = serie.Trim();
                    serieFolio += serieFolio == "" ? "" : "-";
                    serieFolio += folio.Trim();

                    concepto += " (" + serieFolio + ")";
                }

                comando.Parameters["@idinforme"].Value = Datos.IdInforme;
                comando.Parameters["@idgastoorigen"].Value = Datos.IdGasto;
                comando.Parameters["@fgasto"].Value = Fecha;
                comando.Parameters["@hgasto"].Value = Datos.HGasto;
                comando.Parameters["@concepto"].Value = concepto;
                comando.Parameters["@negocio"].Value = Datos.Negocio;
                comando.Parameters["@ugasto"].Value = Datos.UGasto;
                comando.Parameters["@ucrea"].Value = UCrea;
                comando.Parameters["@formapago"].Value = Datos.FormaPago;
                comando.Parameters["@idcategoria"].Value = Datos.IdCategoria;
                comando.Parameters["@categoria"].Value = Datos.Categoria;
                comando.Parameters["@ivacategoria"].Value = Datos.IvaCategoria;
                comando.Parameters["@tgastado"].Value = Datos.TGastado;
                comando.Parameters["@tcomprobar"].Value = Datos.TComprobar;
                comando.Parameters["@observaciones"].Value = Datos.Observaciones;
                comando.Parameters["@conciliacionbanco"].Value = Datos.ConciliacionBanco;
                comando.Parameters["@idmovbanco"].Value = Datos.IdMovBanco;
                comando.Parameters["@dirxml"].Value = archivo.Ruta ?? "";
                comando.Parameters["@tipoajuste"].Value = Datos.Tipo;
                comando.Parameters["@emisor"].Value = ArcCdfi.Emisor ?? "";
                comando.Parameters["@receptor"].Value = ArcCdfi.Receptor ?? "";
                comando.Parameters["@uuid"].Value = ArcCdfi.UUID ?? "";
                comando.Parameters["@formapagocfdi"].Value = ArcCdfi.Formapago ?? "";
                comando.Parameters["@tipodecomprobante"].Value = ArcCdfi.TipoDeComprobante ?? "";
                comando.Parameters["@nmbemisor"].Value = ArcCdfi.NmbEmisor ?? "";
                comando.Parameters["@serie"].Value = serie;
                comando.Parameters["@folio"].Value = folio;

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
                        try
                        {
							
                            int error = Convert.ToInt16(row["Error"]);
							int IdGastoAdminERP_2 = 0;

							if (error == 0)
                            {
								if ((ArcCdfi.UUID ?? "").Trim() != "")
								{
									int IdGastoAdminERP = Convert.ToInt16(row["IdGastoAdminERP"]);
									if (IdGastoAdminERP > 0)
									{
										IdGastoAdminERP_2 = IdGastoAdminERP;
										try
										{
											DocumentoEntrada entradadoc = new DocumentoEntrada
											{
												Usuario = Datos.UGasto,//Variables.usuario;
												Origen = "AdminWEB",
												Transaccion = 120092,
												Operacion = 21
											};//21:Agregar XML, 22:Eliminar XML
											entradadoc.agregaElemento("FiGfaGasto", IdGastoAdminERP);
											entradadoc.agregaElemento("FiGfaUuid", (ArcCdfi.UUID).Trim());

											DocumentoSalida respuesta = PeticionCatalogo(entradadoc.Documento);
											if (respuesta.Resultado == "0")
											{
												string msnError = "";

												XmlDocument xmErrores = new XmlDocument();
												xmErrores.LoadXml(respuesta.Errores.InnerXml);

												XmlNodeList elemList = xmErrores.GetElementsByTagName("Descripcion");
												for (int i = 0; i < elemList.Count; i++)
												{
													msnError += elemList[i].InnerXml;
												}
												if (msnError != "")
												{
													path = HttpContext.Current.Server.MapPath("/");
													string urlXML = path + (archivo.Ruta ?? "");
													Deletexml(urlXML);
												}
												resultado.AgregadoOk = false;
												resultado.Descripcion = (Datos.Tipo == 1 ? "Gasto Adicinal / Propina NO Agregada al Gasto. " : "Comprobante (CFDI) adicional NO agregado. ") + msnError;
												resultado.IdInforme = Convert.ToInt16(row["idinforme"]);
												resultado.IdGasto = Convert.ToInt16(row["idgasto"]);
												resultado.IdGastoOrigen = Convert.ToInt16(row["idgastoorigen"]);

												//borrar xml
												try
												{
													SqlConnection Conexion = new SqlConnection
													{
														ConnectionString = VariablesGlobales.CadenaConexion
													};

													string query = "UPDATE gastos " + 
																	" SET g_dirxml = '', g_xmlcargado = 0, g_valor = g_total, g_deducible = 0, g_importenodeducible = g_total " + 
																	" WHERE g_idinforme = " + resultado.IdInforme + " AND g_id = " + resultado.IdGasto + "; " +
																	" DELETE FROM xmlinforme WHERE x_idinforme = " + resultado.IdInforme + " AND x_idgasto = " + resultado.IdGasto + "; " +
																	" EXEC UpdateTotalInforme " + resultado.IdInforme + ";";
													SqlDataAdapter DA2;
													DataTable DT2 = new DataTable();
													DA2 = new SqlDataAdapter(query, Conexion);
													DA.Fill(DT2);

												}
												catch (Exception)
												{

													throw;
												}

											}
											else {
												resultado.AgregadoOk = true;
												resultado.Descripcion = Datos.Tipo == 1 ? "Gasto Adicinal / Propina Agregada al Gasto. " : "Comprobante (CFDI) adicional agregado. ";
												resultado.IdInforme = Convert.ToInt16(row["idinforme"]);
												resultado.IdGasto = Convert.ToInt16(row["idgasto"]);
												resultado.IdGastoOrigen = Convert.ToInt16(row["idgastoorigen"]);
											}
										}
										catch (Exception)
										{

											throw;
										}
									}
									else {
										resultado.AgregadoOk = true;
										resultado.Descripcion = Datos.Tipo == 1 ? "Gasto Adicinal / Propina Agregada al Gasto. " : "Comprobante (CFDI) adicional agregado. ";
										resultado.IdInforme = Convert.ToInt16(row["idinforme"]);
										resultado.IdGasto = Convert.ToInt16(row["idgasto"]);
										resultado.IdGastoOrigen = Convert.ToInt16(row["idgastoorigen"]);
									}
								}
								else {
									resultado.AgregadoOk = true;
									resultado.Descripcion = Datos.Tipo == 1 ? "Gasto Adicinal / Propina Agregada al Gasto. " : "Comprobante (CFDI) adicional agregado. ";
									resultado.IdInforme = Convert.ToInt16(row["idinforme"]);
									resultado.IdGasto = Convert.ToInt16(row["idgasto"]);
									resultado.IdGastoOrigen = Convert.ToInt16(row["idgastoorigen"]);
								}

								if (Convert.ToString(row["DirPDFOrigen"]) != "" || Convert.ToString(row["DirIMGOrigen"]) != "") {
									string dirPDF = path + Convert.ToString(row["DirPDFOrigen"]);
									string dirIMG = path + Convert.ToString(row["DirIMGOrigen"]);

									string dirPDFAd = "", dirIMGAd = "";
									if(Convert.ToString(row["DirPDFOrigen"]) != "")
									{
										string[] PDF = dirPDF.Split('.');
										dirPDFAd = PDF[0] + "ad" + resultado.IdGasto + "." + PDF[1];
										if (File.Exists(dirPDFAd) == false) {
											File.Copy(dirPDF, dirPDFAd);
										}
									}
									if (Convert.ToString(row["DirIMGOrigen"]) != "")
									{
										string[] IMG = dirIMG.Split('.');
										dirIMGAd = IMG[0] + "ad" + resultado.IdGasto + "." + IMG[1];
										if(File.Exists(dirIMGAd) == false)
										{
											File.Copy(dirIMG, dirIMGAd);
										}										
									}
									dirPDFAd = dirPDFAd.Replace(path, "");
									dirIMGAd = dirIMGAd.Replace(path, "");

									string consulta = "UPDATE gastos SET " + 
										"g_dirpdf = '" + dirPDFAd + "', " +
										"g_dirotros = '" + dirIMGAd + "' " +
										"WHERE g_idinforme = " + resultado.IdInforme + " AND " + 
											"g_idgorigen = " + resultado.IdGastoOrigen + " AND " + 
											"g_id = " + resultado.IdGasto + ";";
									DA = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
									DA.Fill(DT);
								}
                            }
                            else {
                                resultado.AgregadoOk = false;
                                resultado.Descripcion = Datos.Tipo == 1 ? "Gasto Adicinal / Propina NO Agregada al Gasto. " : "Comprobante (CFDI) adicional NO agregado. ";

                                if (Datos.Tipo == 2)
                                {
                                    path = HttpContext.Current.Server.MapPath("/");
                                    string urlXML = path + (archivo.Ruta ?? "");
                                    if (urlXML != "")
                                        Deletexml(urlXML);
                                    resultado.Descripcion = resultado.Descripcion + Convert.ToString(row["msn"]);
                                }

                                resultado.IdInforme = Datos.IdInforme;
                                resultado.IdGasto = Datos.IdGasto;
                                resultado.IdGastoOrigen = Datos.IdGasto;
                            }
                        }
                        catch (Exception err)
                        {
                            string error = "";
                            if (Convert.ToInt16(row["ErrorNumber"]) > 0)
                            {
                                error = "Error Linea: " + Convert.ToString(row["ErrorLine"]) + ". Message: " + Convert.ToString(row["ErrorMessage"]) + ". " + err.ToString();
                            }
                            else
                            {
                                error = "Error: " + err.ToString();
                            }
                            if (Datos.Tipo == 2)
                            {
                                path = HttpContext.Current.Server.MapPath("/");
                                string urlXML = path + (archivo.Ruta ?? "");
                                if(urlXML != "")
                                    Deletexml(urlXML);
                            }
                            resultado.AgregadoOk = false;
                            resultado.Descripcion = error;
                            resultado.IdInforme = Datos.IdInforme;
                            resultado.IdGasto = Datos.IdGasto;
                            resultado.IdGastoOrigen = Datos.IdGasto;
                        }
                    }
                }
                else
                {
                    resultado.AgregadoOk = false;
                    if (Datos.Tipo == 2)
                    {
                        path = HttpContext.Current.Server.MapPath("/");
                        string urlXML = path + (archivo.Ruta ?? "");
                        if (urlXML != "")
                            Deletexml(urlXML);
                    }
                    resultado.Descripcion = Datos.Tipo == 1 ? "Gasto Adicinal / Propina NO Agregada al Gasto" : "Comprobante (CFDI) adicional NO agregado.";
                    resultado.IdInforme = Datos.IdInforme;
                    resultado.IdGasto = Datos.IdGasto;
                    resultado.IdGastoOrigen = Datos.IdGasto;
                }
            }
            catch (Exception err)
            {
                string error = "Error: " + err.ToString();
                resultado.AgregadoOk = false;
                resultado.Descripcion = (Datos.Tipo == 1 ? "Gasto Adicinal / Propina NO Agregada al Gasto. " : "Comprobante (CFDI) adicional NO agregado. ") + error;
                resultado.IdInforme = Datos.IdInforme;
                resultado.IdGasto = Datos.IdGasto;
                resultado.IdGastoOrigen = Datos.IdGasto;
            }

            return resultado;
        }

        public ListResultArchivo GuardarXmlAdicional(int IdInforme, int IdGasto, string Based64BinaryString, string ExtFile)
        {
            ListResultArchivo result = new ListResultArchivo();
            try
            {
                if (ExtFile == "xml")
                {
                    string format = "";
                    string path = HttpContext.Current.Server.MapPath("/XML/");
                    string name = "GAdcional_" + IdInforme.ToString() + "_" + IdGasto.ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");


                    if (Based64BinaryString.Contains("data:text/xml;base64,"))
                    {
                        format = "xml";
                    }

                    string str = Based64BinaryString.Replace("data:text/xml;base64,", " ");//jpg check

                    byte[] data = Convert.FromBase64String(str);

                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    string rutacompleta = path + name + "." + format;
                    File.WriteAllBytes(rutacompleta, data);
                    string ruta = rutacompleta;
                    ruta = "XML/" + name + "." + format;

                    result.ArchivoOk = true;
                    result.Descripcion = "Archivo Guardado.";
                    result.Ruta = ruta;
                }
                else
                {
                    result.ArchivoOk = false;
                    result.Descripcion = "Archivo No Guardado, formato invalido.";
                    result.Ruta = "";
                }

            }
            catch (Exception ex)
            {
                string error = "Error : " + ex.ToString();

                result.ArchivoOk = false;
                result.Descripcion = "Archivo No Guardado. " + error;
                result.Ruta = "";
            }
            return result;
        }

        public ListResultCFDIXML LeerCFDIXML(string url)
        {
            ListResultCFDIXML ArcCdfi = new ListResultCFDIXML();
            try
            {
                double Total = 0;
                string Emisor = "";
                string Receptor = "";
                string UUID = "";
                string Formapago = "";
                string TipoDeComprobante = "";
                string Folio = "";
                string Serie = "";
                string NmbEmisor = "";

                XmlTextReader reader = new XmlTextReader(url);

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
                                while (reader.MoveToNextAttribute())
                                { // Read the attributes

                                    if (reader.Name == "uuid" || reader.Name == "UUID")
                                    {
                                        UUID = Convert.ToString(reader.Value);
                                    }
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
                                if (reader.Name == "Folio" && ArcCdfi.Folio.Trim() == "")
                                {
                                    Folio = Convert.ToString(reader.Value);
                                }
                                if (reader.Name == "Serie" && ArcCdfi.Serie.Trim() == "")
                                {
                                    Serie = Convert.ToString(reader.Value);
                                }
                            }
                            break;
                    }
                }

                ArcCdfi.Receptor = Receptor;
                ArcCdfi.Emisor = Emisor;
                ArcCdfi.Total = Total;
                ArcCdfi.UUID = UUID;
                ArcCdfi.TipoDeComprobante = TipoDeComprobante;
                ArcCdfi.Serie = Serie;
                ArcCdfi.Folio = Folio;
                ArcCdfi.NmbEmisor = NmbEmisor;
                ArcCdfi.Formapago = Formapago;
            }
            catch (Exception err)
            {
                string error = err.ToString();
                ArcCdfi.Receptor = error;
                ArcCdfi.Emisor = error;
                ArcCdfi.Total = 0;
                ArcCdfi.UUID = "";
                ArcCdfi.TipoDeComprobante = "";
                ArcCdfi.Serie = "";
                ArcCdfi.Folio = "";
            }
            
            return ArcCdfi;
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
