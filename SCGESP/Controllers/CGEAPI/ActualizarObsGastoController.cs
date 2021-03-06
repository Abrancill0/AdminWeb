﻿using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class ActualizarObsGastoController : ApiController
    {
		public class Parametros
		{
			public int IdInforme { get; set; }
			public int IdGasto { get; set; }
			public string Observaciones { get; set; }
		}
		public string Post(Parametros Datos)
		{
			DataTable DT;
			SqlConnection Conexion = new SqlConnection
			{
				ConnectionString = VariablesGlobales.CadenaConexion
			};

			string query = "UPDATE gastos SET g_observaciones = '" + Datos.Observaciones + "' " +
							" WHERE g_idinforme = " + Datos.IdInforme + " AND g_id = " + Datos.IdGasto + ";";
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
