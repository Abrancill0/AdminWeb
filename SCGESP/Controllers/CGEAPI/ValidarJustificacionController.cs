using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class ValidarJustificacionController : ApiController
	{
		public class Parametros
		{
			public int IdInforme { get; set; }
		}
		public class ListResultado
		{
			public string Mensaje { get; set; }
			public int Error { get; set; }
			public IEnumerable<ListResult> Lista { get; set; }
		}
		public class ListResult
		{
			public string Justificacion { get; set; }
			public string Categoria { get; set; }
			public string Comensales { get; set; }
			public string Valor { get; set; }
		}
		public ListResultado Post(Parametros Datos)
		{
			try
			{
				SqlCommand comando = new SqlCommand("ValidaValoresCategoria")
				{
					CommandType = CommandType.StoredProcedure
				};//browseGastosInforme

				//Declaracion de parametros
				comando.Parameters.Add("@idinforme", SqlDbType.Int);

				//Asignacion de valores a parametros
				comando.Parameters["@idinforme"].Value = Datos.IdInforme;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
				comando.CommandTimeout = 0;
				comando.Connection.Open();

				DataTable DT = new DataTable();
				SqlDataAdapter DA = new SqlDataAdapter(comando);
				comando.Connection.Close();
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					List<ListResult> Lista_Justificacion = new List<ListResult>();

					foreach (DataRow row in DT.Rows)
					{
						ListResult justificacion_faltante = new ListResult{
							Justificacion = Convert.ToString(row["justificacion"]),
							Categoria = Convert.ToString(row["categoria"]),
							Comensales = Convert.ToString(row["comensales"]),
							Valor = Convert.ToString(row["valor"])
						};
						Lista_Justificacion.Add(justificacion_faltante);
					}

					ListResultado lista = new ListResultado
					{
						Error = 2,
						Mensaje = "Justificar " + DT.Rows.Count + " Gasto(s)",
						Lista = Lista_Justificacion
					};
					return lista;
				}
				else
				{
					ListResultado lista = new ListResultado
					{
						Error = 0,
						Mensaje = "OK",
						Lista = null
					};
					return lista;
				}


			}
			catch (Exception ex)
			{
				ListResultado lista = new ListResultado
				{
					Error = 1,
					Mensaje = "Error al validar justificación del informe. " + ex.ToString(),
					Lista = null
				};
				return lista;
			}

		}
	}
}
