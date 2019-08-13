using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
	public class rep_informes_estatusController : ApiController
	{
		public class Parametros
		{
			public string TipoFecha { get; set; }
			public string RepDe { get; set; }
			public string RepA { get; set; }
			public string Estatus { get; set; }
			public string UResponsable { get; set; }
		}
		public class Resultado
		{
			public int i_id { get; set; }
			public int i_ninforme { get; set; }
			public int i_estatus { get; set; }
			public string e_estatus { get; set; }
			public string i_fcrea { get; set; }
			public string i_ucrea { get; set; }
			public string i_uresponsable { get; set; }
			public string i_finicio { get; set; }
			public string i_ffin { get; set; }
			public decimal i_total { get; set; }
			public decimal i_totalg { get; set; }
			public string responsable { get; set; }
			public string i_nmb { get; set; }
			public int r_idrequisicion { get; set; }
			public string i_motivo { get; set; }
			public string i_notas { get; set; }
			public string i_tipo { get; set; }
			public string i_tarjetatoka { get; set; }
			public decimal r_montorequisicion { get; set; }
			public int r_idgasto { get; set; }
			public decimal r_importecomprobar { get; set; }
			public decimal i_importenodeducible { get; set; }
			public decimal i_importereembolsable { get; set; }
			public decimal i_importenoreembolsable { get; set; }
			public decimal i_importenoaceptable { get; set; }
			public int i_rechazado { get; set; }
			public string bandeja_usuario { get; set; }
			public string usuarios_autorizadores { get; set; }
			public string a_fsolicitud { get; set; }
			public string a_flibera { get; set; }
			public string i_nombreresponsabe { get; set; }
			public int i_autorizado { get; set; }
			public string i_comentarioaut { get; set; }
			public string i_mescontable { get; set; }
			public string i_comentario_1 { get; set; }
			public string i_comentario_2 { get; set; }
			public string i_comentario_3 { get; set; }
			public string i_comentario_4 { get; set; }
			public string i_uautoriza { get; set; }
			public decimal i_tsreembolso { get; set; }
			public decimal i_tnreembolso { get; set; }
			public int i_conciliacionxml { get; set; }
			public int i_conciliacionbancos { get; set; }
			public int i_conciliacionconvenios { get; set; }
			public int i_contabilizar { get; set; }
		}
		public List<Resultado> Post(Parametros Datos)
		{
			try
			{
				Datos.RepDe += " 00:00:00";
				Datos.RepA += " 23:59:59";
				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};

				List<Resultado> Resultado = new List<Resultado>();
				string query = "SELECT * FROM vw_BrowseInforme ";
				query += "WHERE ";

				query += "(";
				if (Datos.TipoFecha == "*")
				{
					query += "i_fcrea BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' ";
					query += "OR (i_finicio BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' OR i_ffin BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "')";
				}
				else if (Datos.TipoFecha == "registro")
				{
					query += "i_fcrea BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "'";
				}
				else if (Datos.TipoFecha == "periodo")
				{
					query += "(i_finicio BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' OR i_ffin BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "')";
				}
				query += ") ";

				if (Datos.Estatus != "*") {
					query += "AND e_estatus = '" + Datos.Estatus + "' ";
				}

				if (Datos.UResponsable != "*")
				{
					query += "AND i_uresponsable = '" + Datos.UResponsable + "' ";
				}

				query += "ORDER BY i_id";

				DA = new SqlDataAdapter(query, Conexion);
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					string FechaCrea = "";
					string FechaInicio = "";
					string FechaFin = "";
					string FechaSolicitud = "";
					string FechaLibera = "";

					foreach (DataRow row in DT.Rows)
					{
						FechaCrea = FechaInicio = FechaFin = FechaSolicitud = FechaLibera = "";

						FechaCrea = FormatFecha(Convert.ToString(row["i_fcrea"]));
						FechaInicio = FormatFecha(Convert.ToString(row["i_finicio"]));
						FechaFin = FormatFecha(Convert.ToString(row["i_ffin"]));
						FechaSolicitud = FormatFecha(Convert.ToString(row["a_fsolicitud"]));
						FechaLibera = FormatFecha(Convert.ToString(row["a_flibera"]));

						if (row["a_fsolicitud"] != null && Convert.ToString(row["a_fsolicitud"]) != "")
						{
							FechaSolicitud = Convert.ToDateTime(row["a_fsolicitud"]).ToShortDateString() + " " + Convert.ToDateTime(row["a_fsolicitud"]).ToShortTimeString();
						}

						Resultado ent = new Resultado
						{
							i_id = Convert.ToInt32(row["i_id"]),
							i_ninforme = Convert.ToInt32(row["i_ninforme"]),
							i_estatus = Convert.ToInt32(row["i_estatus"]),
							e_estatus = Convert.ToString(row["e_estatus"]),
							i_fcrea = FechaCrea,
							i_ucrea = Convert.ToString(row["i_ucrea"]),
							i_uresponsable = Convert.ToString(row["i_uresponsable"] is DBNull ? "" : row["i_uresponsable"]),
							i_finicio = FechaInicio,
							i_ffin = FechaFin,
							i_nmb = Convert.ToString(row["i_nmb"] is DBNull ? "" : row["i_nmb"]),
							i_total = Convert.ToDecimal(row["i_total"] is DBNull ? 0 : row["i_total"]),
							i_totalg = Convert.ToDecimal(row["i_totalg"] is DBNull ? 0 : row["i_totalg"]),
							responsable = Convert.ToString(row["responsable"] is DBNull ? "" : row["responsable"]),
							r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]),
							i_motivo = Convert.ToString(row["i_motivo"] is DBNull ? "" : row["i_motivo"]),
							i_notas = Convert.ToString(row["i_notas"] is DBNull ? "" : row["i_notas"]),
							i_tipo = Convert.ToString(row["i_tipo"] is DBNull ? "" : row["i_tipo"]),
							i_tarjetatoka = Convert.ToString(row["i_tarjetatoka"] is DBNull ? "" : row["i_tarjetatoka"]),
							r_montorequisicion = Convert.ToDecimal(row["r_montorequisicion"] is DBNull ? 0 : row["r_montorequisicion"]),
							r_idgasto = Convert.ToInt32(row["r_idgasto"] is DBNull ? 0 : row["r_idgasto"]),
							r_importecomprobar = Convert.ToDecimal(row["r_importecomprobar"] is DBNull ? 0 : row["r_importecomprobar"]),
							i_importenodeducible = Convert.ToDecimal(row["i_importenodeducible"] is DBNull ? 0 : row["i_importenodeducible"]),
							i_importereembolsable = Convert.ToDecimal(row["i_importereembolsable"] is DBNull ? 0 : row["i_importereembolsable"]),
							i_importenoreembolsable = Convert.ToDecimal(row["i_importenoreembolsable"] is DBNull ? 0 : row["i_importenoreembolsable"]),
							i_importenoaceptable = Convert.ToDecimal(row["i_importenoaceptable"] is DBNull ? 0 : row["i_importenoaceptable"]),
							i_rechazado = Convert.ToInt32(row["i_rechazado"] is DBNull ? 0 : row["i_rechazado"]),
							bandeja_usuario = Convert.ToString(row["bandeja_usuario"] is DBNull ? "" : row["bandeja_usuario"]),
							usuarios_autorizadores = Convert.ToString(row["usuarios_autorizadores"] is DBNull ? "" : row["usuarios_autorizadores"]),
							a_fsolicitud = FechaSolicitud,
							a_flibera = FechaLibera,
							i_nombreresponsabe = Convert.ToString(row["i_nombreresponsabe"] is DBNull ? "" : row["i_nombreresponsabe"]),
							i_autorizado = Convert.ToInt32(row["i_autorizado"] is DBNull ? 0 : row["i_autorizado"]),
							i_comentarioaut = Convert.ToString(row["i_comentarioaut"] is DBNull ? "" : row["i_comentarioaut"]),
							i_mescontable = "",
							i_comentario_1 = Convert.ToString(row["i_comentario_1"] is DBNull ? "" : row["i_comentario_1"]),
							i_comentario_2 = Convert.ToString(row["i_comentario_2"] is DBNull ? "" : row["i_comentario_2"]),
							i_comentario_3 = Convert.ToString(row["i_comentario_3"] is DBNull ? "" : row["i_comentario_3"]),
							i_comentario_4 = Convert.ToString(row["i_comentario_4"] is DBNull ? "" : row["i_comentario_4"]),
							i_uautoriza = Convert.ToString(row["i_uautoriza"] is DBNull ? "" : row["i_uautoriza"]),
							i_tsreembolso = Convert.ToDecimal(row["i_tsreembolso"] is DBNull ? 0 : row["i_tsreembolso"]),
							i_tnreembolso = Convert.ToDecimal(row["i_tnreembolso"] is DBNull ? 0 : row["i_tnreembolso"]),
							i_conciliacionxml = Convert.ToInt16(row["i_conciliacionxml"] is DBNull ? 0 : row["i_conciliacionxml"]),
							i_conciliacionbancos = Convert.ToInt16(row["i_conciliacionbancos"] is DBNull ? 0 : row["i_conciliacionbancos"]),
							i_conciliacionconvenios = Convert.ToInt16(row["i_conciliacionconvenios"] is DBNull ? 0 : row["i_conciliacionconvenios"]),
							i_contabilizar = Convert.ToInt16(row["i_contabilizar"] is DBNull ? 0 : row["i_contabilizar"]),
						};
		
						Resultado.Add(ent);
					}
					return Resultado;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public string FormatFecha(string fecha)
		{
			try
			{
				if (fecha != null && Convert.ToString(fecha) != "")
				{
					return Convert.ToDateTime(fecha).ToShortDateString();
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

	}
}