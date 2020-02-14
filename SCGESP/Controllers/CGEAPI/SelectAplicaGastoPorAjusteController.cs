using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class SelectAplicaGastoPorAjusteController : ApiController
    {
		public class Parametros
		{
			public int IdInforme { get; set; }
		}
		public class Respuesta
		{
			public List<ListGastos> Gastos { get; set; }
			public decimal TotalDiferencia { get; set; }
			public string Mensaje { get; set; }
			public bool Ok { get; set; }
		}
		public class ListGastos
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
		public Respuesta Post(Parametros Datos)
		{
			Respuesta Resultado = new Respuesta() {
				Mensaje = "",
				Ok = false
			};

			try
			{
				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};
				string consulta = "SELECT * FROM vw_BrowseInformesAplicanGastoPorAjuste WHERE IdInforme = " + Datos.IdInforme + ";";

				DA = new SqlDataAdapter(consulta, Conexion);
				DA.Fill(DT);
				Resultado.Ok = false;
				if (DT.Rows.Count > 0) {
					DataRow RowInforme = DT.Rows[0];
					decimal TotalAjuste = Convert.ToDecimal(RowInforme["Diferencia"] is DBNull ? 0 : RowInforme["Diferencia"]);
					Resultado.TotalDiferencia = TotalAjuste;
					if (TotalAjuste > 0)
					{
						consulta = "SELECT T1.*, T1.g_id AS g_idgorigen " +
							", CONVERT(varchar,g_fgasto,103) AS g_fgasto, hgasto, g_ugasto, g_fcreo, g_ucreo, g_concepto " + 
							", g_negocio, g_formapago, g_observaciones, g_comprobante " +
							", g_estatus, g_aplica, g_conciliacionbancos " +
							", g_idmovbanco, g_ivaCategoria " +
							"FROM vw_BrowseDifValXMLvsTotalPorGasto AS T1 INNER JOIN " +
							"gastos AS T2 ON T2.g_idinforme = T1.g_idinforme AND T2.g_id = T1.g_id " +
							"WHERE T1.g_idinforme = " + Datos.IdInforme + ";";
						DT = new DataTable();
						DA = new SqlDataAdapter(consulta, Conexion);
						DA.Fill(DT);
						List<ListGastos> Gastos = new List<ListGastos>();
						if (DT.Rows.Count > 0)
						{
							foreach (DataRow RGasto in DT.Rows)
							{
								string categoria = Convert.ToString(RGasto["g_nombreCategoria"]);
								string concepto = "Ajuste automático del gasto: " + categoria + " (" + Convert.ToString(RGasto["g_concepto"]) + ")";
								Gastos.Add(
									new ListGastos()
									{
										IdInforme = Convert.ToInt32(RGasto["g_idinforme"]),
										IdGasto = Convert.ToInt32(RGasto["g_idgorigen"]),
										IdMovBanco = Convert.ToInt32(RGasto["g_idmovbanco"] is DBNull ? 0 : RGasto["g_idmovbanco"]),
										IdCategoria = Convert.ToInt32(RGasto["g_categoria"]),
										Categoria = categoria,
										Concepto = concepto,
										Negocio = concepto,
										Observaciones = concepto,
										FormaPago = Convert.ToString(RGasto["g_formapago"] is DBNull ? "" : RGasto["g_formapago"]),
										ConciliacionBanco = Convert.ToInt16(RGasto["g_conciliacionbancos"] is DBNull ? 0 : RGasto["g_conciliacionbancos"]),
										FGasto = Convert.ToString(RGasto["g_fgasto"]),
										HGasto = Convert.ToString(RGasto["hgasto"] is DBNull ? "" : RGasto["hgasto"]),
										IvaCategoria = Convert.ToDouble(RGasto["g_ivaCategoria"] is DBNull ? 0 : RGasto["g_ivaCategoria"]),
										TGastado = Convert.ToDouble(RGasto["Diferencia"] is DBNull ? 0 : RGasto["Diferencia"]),
										TComprobar = Convert.ToDouble(RGasto["Diferencia"] is DBNull ? 0 : RGasto["Diferencia"]),
										UCrea = Seguridad.Encriptar(Convert.ToString(RGasto["g_ucreo"] is DBNull ? "" : RGasto["g_ucreo"])),
										UGasto = Convert.ToString(RGasto["g_ugasto"] is DBNull ? "" : RGasto["g_ugasto"]),
										Tipo = 3,
										ExtFile = "",
										NombreArc = "",
										BinXML = "",
										AfectaImpComprobado = 1,
										AfectaImpGastado = 1
									}
									);
							}

							Resultado.Gastos = Gastos;
							Resultado.Mensaje = "Gastos con diferencia a ajustar seleccionados.";
							Resultado.Ok = true;
						}
						else {
							Resultado.Mensaje = "Sin gastos con diferencia a ajustar.";
						}
					}
					else
					{
						Resultado.Mensaje = "Sin diferencia a ajustar.";
					}
				}
				else
				{
					Resultado.Mensaje = "No existe diferencia para generar gasto por ajuste.";
				}
			}
			catch (Exception ex)
			{
				Resultado.Mensaje = "Error: " + ex.Message.ToString();
			}

			return Resultado;

		}
	}
}
