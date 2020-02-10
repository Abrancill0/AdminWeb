using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class Resultado
	{
		public string Categoria { get; set; }
	}
	public class SelectCategoriasController : ApiController
    {
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
				string query = "SELECT categoria FROM vw_BrowseCategoriasGastos WHERE ISNULL(categoria, '') <> '' ORDER BY categoria ASC";

				DA = new SqlDataAdapter(query, Conexion);
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					foreach (DataRow row in DT.Rows)
					{

						Resultado ent = new Resultado
						{
							Categoria = Convert.ToString(row["categoria"]).Trim()
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
