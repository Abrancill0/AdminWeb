using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class ActualizaInfReqController : ApiController
	{
		public class Parametros
		{
			public int IdInforme { get; set; }
			public int RmReqId { get; set; }
			public string RmReqJustificacion { get; set; }
			public string FInicio { get; set; }
			public string FFin { get; set; }
			public string Usuario { get; set; }
			public string Empleado { get; set; }
		}
		public string Post(Parametros Datos)
		{
			DataTable DT;
			SqlConnection Conexion = new SqlConnection
			{
				ConnectionString = VariablesGlobales.CadenaConexion
			};

			string query = "UPDATE informe SET i_finicio = '" + Datos.FInicio + "', " + 
							" i_ffin = '" + Datos.FFin + "', i_nmb = '" + Datos.RmReqJustificacion + "' " + 
							" WHERE i_id = " + Datos.IdInforme + ";";
			try
			{
				DT = EjecutarQuery(query, Conexion);
				return "OK";
			}
			catch (Exception err)
			{
				var error = Convert.ToString(err);
				return "Error: " + error;
			}
		}
		public DataTable EjecutarQuery(string query, SqlConnection Conexion)
		{
			SqlDataAdapter DA;
			DataTable DT = new DataTable();
			DA = new SqlDataAdapter(query, Conexion);
			DA.Fill(DT);
			return DT;
		}
	}
}
