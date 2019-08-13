using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
	public class SelectEstatusInformeController : ApiController
	{
		public class Resultado
		{
			public string Estatus { get; set; }
		}
		public List<Resultado> Post()
		{
			try
			{
				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};

				List<Resultado> Resultado = new List<Resultado>();
				string query = "SELECT estatus FROM vw_estatus_informe ORDER BY orden;";

				DA = new SqlDataAdapter(query, Conexion);
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					foreach (DataRow row in DT.Rows)
					{
						Resultado ent = new Resultado
						{
							Estatus = Convert.ToString(row["estatus"]).Trim()
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
			catch (Exception)
			{

				return null;
			}
		}
	}
}