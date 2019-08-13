using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
	public class SelectUsuariosController : ApiController
	{
		public class Resultado
		{
			public string Usuario { get; set; }
			public string Nombre { get; set; }
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
				string query = "SELECT usuario, nombre FROM vw_usuarios_informe ORDER BY nombre ASC";

				DA = new SqlDataAdapter(query, Conexion);
				DA.Fill(DT);
				
				if (DT.Rows.Count > 0)
				{
					foreach (DataRow row in DT.Rows)
					{
						Resultado ent = new Resultado
						{
							Usuario = Convert.ToString(row["usuario"]).Trim(),
							Nombre = Convert.ToString(row["nombre"]).Trim()
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