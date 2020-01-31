using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCGESP.Controllers.CGEAPI
{
	public class UpdateGastoPDFOtrosController : ApiController
	{
		public class ParametrosGastos
		{
			public int id { get; set; }
			public int idinforme { get; set; }
			public string dir { get; set; }
			public string Valida { get; set; }
		}

		public class Resultado
		{
			public string UrlWeb { get; set; }
			public string UrlDisco { get; set; }
		}

		[HttpPost]

		public async Task<Resultado> Post()
		{
			try
			{
				Resultado Ruta;
				string TipoArchivo = "";
				string ruta2 = HttpContext.Current.Server.MapPath("/");
				string ruta3 = System.Web.Hosting.HostingEnvironment.MapPath("/");

				HttpRequest httpRequest = HttpContext.Current.Request;

				ParametrosGastos Datos = new ParametrosGastos
				{
					id = Convert.ToInt16(httpRequest.Params["id"]),
					idinforme = Convert.ToInt32(httpRequest.Params["idinforme"]),
					dir = httpRequest.Params["dir"],
					Valida = httpRequest.Params["Valida"]
				};
				
				if (Datos.Valida == "1")
				{
					Ruta = PostSave(Datos.dir, httpRequest);
					TipoArchivo = "PDF";
				}
				else
				{
					Ruta = PostSaveImage(Datos.dir);
					TipoArchivo = "IMG";
				}

				if (Ruta.UrlWeb.Contains("Error"))
				{
					Ruta.UrlWeb = "";
				}


				SqlCommand comando = new SqlCommand("UpdateGastoPDFOtros");
				comando.CommandType = CommandType.StoredProcedure;

				//Declaracion de parametros
				comando.Parameters.Add("@id", SqlDbType.Int);
				comando.Parameters.Add("@idinforme", SqlDbType.Int);
				comando.Parameters.Add("@dir", SqlDbType.VarChar);
				comando.Parameters.Add("@Valida", SqlDbType.VarChar);

				//Asignacion de valores a parametros
				comando.Parameters["@id"].Value = Datos.id;
				comando.Parameters["@idinforme"].Value = Datos.idinforme;
				comando.Parameters["@dir"].Value = Ruta.UrlWeb;
				comando.Parameters["@Valida"].Value = Datos.Valida;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
				comando.CommandTimeout = 0;
				comando.Connection.Open();
				//DA.SelectCommand = comando;
				comando.ExecuteNonQuery();

				DataTable DT = new DataTable();
				SqlDataAdapter DA = new SqlDataAdapter(comando);
				comando.Connection.Close();
				DA.Fill(DT);


				if (DT.Rows.Count > 0)
				{
					DataRow row = DT.Rows[0];

					string consulta = "";

					if (TipoArchivo == "PDF")
					{
						consulta = "SELECT g_id FROM gastos WHERE g_idinforme = " + Datos.idinforme + " " +
							"AND g_id <> g_idgorigen " +
							"AND g_idgorigen = " + Datos.id + " " +
							"AND ISNULL(g_dirpdf, '') = '';";
					}
					else if (TipoArchivo == "IMG")
					{
						consulta = "SELECT g_id FROM gastos WHERE g_idinforme = " + Datos.idinforme + " " +
							"AND g_id <> g_idgorigen " +
							"AND g_idgorigen = " + Datos.id + " " +
							"AND ISNULL(g_dirotros, '') = '';";
					}
					if (consulta != "")
					{
						DataTable DT2 = new DataTable();
						SqlDataAdapter DA2 = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
						DA2.Fill(DT2);
						if (DT2.Rows.Count > 0)
						{
							foreach (DataRow RowGasto in DT2.Rows)
							{
								int IdGasto = Convert.ToInt32(RowGasto["g_id"]);
								if (TipoArchivo == "PDF")
								{
									string dirPDFAd = "";
									string[] PDF = Ruta.UrlDisco.Split('.');
									dirPDFAd = PDF[0] + "ad" + Convert.ToString(IdGasto) + "." + PDF[1];
									if (File.Exists(dirPDFAd) == false)
									{
										File.Copy(Ruta.UrlDisco, dirPDFAd);
									}
									dirPDFAd = dirPDFAd.Replace(ruta2, "");

									consulta = "UPDATE gastos SET " +
										"g_dirpdf = '" + dirPDFAd.Replace("\\", "/") + "' " +
										"WHERE g_idinforme = " + Datos.idinforme + " AND " +
											"g_idgorigen = " + Datos.id + " AND " +
											"g_id = " + IdGasto + ";";
									DataTable DTq = new DataTable();
									SqlDataAdapter DAq = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
									DAq.Fill(DTq);
								}
								else if (TipoArchivo == "IMG")
								{
									string dirIMGAd = "";
									string[] IMG = Ruta.UrlDisco.Split('.');
									dirIMGAd = IMG[0] + "ad" + Convert.ToString(IdGasto) + "." + IMG[1];
									if (File.Exists(dirIMGAd) == false)
									{
										File.Copy(Ruta.UrlDisco, dirIMGAd);
									}
									dirIMGAd = dirIMGAd.Replace(ruta2, "");
									consulta = "UPDATE gastos SET " +
										"g_dirotros = '" + dirIMGAd.Replace("\\", "/") + "' " +
										"WHERE g_idinforme = " + Datos.idinforme + " AND " +
											"g_idgorigen = " + Datos.id + " AND " +
											"g_id = " + IdGasto + ";";
									DataTable DTq = new DataTable();
									SqlDataAdapter DAq = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
									DAq.Fill(DTq);
								}
							}
						}
					}


					return Ruta;
				}
				else
				{
					return Ruta;
				}

			}
			catch (Exception)
			{

				return null;
			}

		}

		public Resultado PostSave(string Based64BinaryString, HttpRequest FilePDF)
		{
			string result = "";
			try
			{
				string format = "";
				string path = HttpContext.Current.Server.MapPath("/PDF/");
				string name = DateTime.Now.ToString("yyyyMMddhhmmss");
				format = "pdf";
				string rutacompleta = path + name + "." + format;
				result = "PDF/" + name + "." + format;
				try
				{
					//FilePDF.Request.Files[0].SaveAs(rutacompleta);
					//FilePDF.SaveAs(rutacompleta);
					if (FilePDF.Files.Count > 0)
					{
						foreach (string file in FilePDF.Files)
						{
							var postedFile = FilePDF.Files[file];

							postedFile.SaveAs(rutacompleta);
						}
					}

					Resultado rutas = new Resultado
					{
						UrlDisco = rutacompleta,
						UrlWeb = result
					};
					return rutas;
				}
				catch (Exception)
				{
					if (Based64BinaryString.Contains("data:application/pdf;base64,"))
					{
						string str = Based64BinaryString.Replace("data:application/pdf;base64,", " ");//jpg check

						byte[] data = Convert.FromBase64String(str);

						MemoryStream ms = new MemoryStream(data, 0, data.Length);
						ms.Write(data, 0, data.Length);
						//System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
						File.WriteAllBytes(rutacompleta, data);
						//image.Save(rutacompleta);

						Resultado rutas = new Resultado
						{
							UrlDisco = rutacompleta,
							UrlWeb = result
						};
						return rutas;
					}
					else
					{
						Resultado rutas = new Resultado
						{
							UrlDisco = "Error : Formato no valido.",
							UrlWeb = "Error : Formato no valido."
						};
						return rutas;
					}
				}
			}
			catch (Exception ex)
			{
				result = "Error : " + ex;
				Resultado rutas = new Resultado
				{
					UrlDisco = result,
					UrlWeb = result
				};
				return rutas;
			}

		}

		public Resultado PostSaveImage(string Based64BinaryString)
		{
			string result = "";
			try
			{
				string format = "";
				string path = HttpContext.Current.Server.MapPath("/Comprobantes/");
				string name = DateTime.Now.ToString("yyyyMMddhhmmss");


				if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
				{
					format = "jpg";
				}
				if (Based64BinaryString.Contains("data:image/png;base64,"))
				{
					format = "png";
				}


				string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
				str = str.Replace("data:image/png;base64,", " ");//png check
				str = str.Replace("data:text/plain;base64,", " ");//text file check
				str = str.Replace("data:;base64,", " ");//zip file check
				str = str.Replace("data:application/zip;base64,", " ");//zip file check

				byte[] data = Convert.FromBase64String(str);

				MemoryStream ms = new MemoryStream(data, 0, data.Length);
				ms.Write(data, 0, data.Length);
				System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
				string rutacompleta = path + "Image" + name + "." + format;
				image.Save(rutacompleta);



				result = "Comprobantes/" + "Image" + name + "." + format;
				Resultado rutas = new Resultado
				{
					UrlDisco = rutacompleta,
					UrlWeb = result
				};
				return rutas;

			}
			catch (Exception ex)
			{
				result = "Error : " + ex;
				Resultado rutas = new Resultado
				{
					UrlDisco = result,
					UrlWeb = result
				};
				return rutas;
			}
		}


	}
}
