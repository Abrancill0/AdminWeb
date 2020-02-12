using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class ConfigGastoAutomaticoAjusteController : ApiController
    {
		public class Parametros
		{
			public string Accion { get; set; }
			public bool GenerarGastoAjuste { get; set; }
			public decimal ToleranciaInformeMenorIgual { get; set; }
		}
		public class Respuesta
		{
			public bool GenerarGastoAjuste { get; set; }
			public decimal ToleranciaInformeMenorIgual { get; set; }
			public string Mensaje { get; set; }
			public bool Ok { get; set; }
		}

		public Respuesta Post(Parametros Datos)
		{
			Respuesta Resultado = new Respuesta();
			try
			{
				bool ok = false;
				Resultado.Ok = ok;
				if (Datos.Accion == "actualizar")
				{
					if ((Datos.GenerarGastoAjuste == true && Datos.ToleranciaInformeMenorIgual > 0) || 
						Datos.GenerarGastoAjuste == false
						) {
						Datos.ToleranciaInformeMenorIgual = Datos.GenerarGastoAjuste == false ? 0 : Datos.ToleranciaInformeMenorIgual;

						ok = Actualizar(Datos);
						Resultado.ToleranciaInformeMenorIgual = Datos.ToleranciaInformeMenorIgual;
						Resultado.GenerarGastoAjuste = Datos.GenerarGastoAjuste;
						Resultado.Ok = ok;
						Resultado.Mensaje = ok ? "Configuración de gasto automático actualizada." : "No se actualizo la configuración de gasto automático";
					}
					else
					{
						if (Datos.GenerarGastoAjuste == true && Datos.ToleranciaInformeMenorIgual <= 0) 
							Resultado.Mensaje = "Se requiere un importe mayor a $ 0.00";
						else
							Resultado.Mensaje = "Datos requeridos.";
					}
				}
				else if (Datos.Accion == "seleccionar")
				{
					Resultado = Seleccionar();
				}
				else {
					Resultado.Mensaje = "No se ejecuto la petición: " + Datos.Accion;
				}
			}
			catch (Exception)
			{
				Resultado.Mensaje = "Error al ejecutar petición";
			}
			return Resultado;
		}

		private Respuesta Seleccionar()
		{
			Respuesta Resultado = new Respuesta();
			try
			{
				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};
				string consulta = "SELECT TOP(1) c_generar_gasto_ajuste, c_tolerancia_informe_menor_igual " +
								  "FROM configuracion; ";

				DA = new SqlDataAdapter(consulta, Conexion);
				DA.Fill(DT);
				Resultado.Ok = false;
				if (DT.Rows.Count > 0) {
					DataRow row = DT.Rows[0];
					Resultado.GenerarGastoAjuste = Convert.ToInt16(row["c_generar_gasto_ajuste"] is DBNull ? 0 : row["c_generar_gasto_ajuste"]) > 0 ? true : false;
					Resultado.ToleranciaInformeMenorIgual = Convert.ToDecimal(row["c_tolerancia_informe_menor_igual"] is DBNull ? 0 : row["c_tolerancia_informe_menor_igual"]);
					Resultado.Mensaje = "Configuración de gasto automático seleccionada.";
					Resultado.Ok = true;
				}
				else
				{
					Resultado.Mensaje = "Configuración de gasto automático NO seleccionada.";
				}
				return Resultado;
			}
			catch (Exception)
			{
				Resultado.Mensaje = "Error al seleccionar configuración de gasto automático.";
				return Resultado;
			}
		}
		private bool Actualizar(Parametros Datos)
		{
			try
			{
				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};
				string consulta = "UPDATE configuracion SET " + 
						"c_generar_gasto_ajuste = " + (Datos.GenerarGastoAjuste ? "1" : "0") + ", " + 
						"c_tolerancia_informe_menor_igual = " + Datos.ToleranciaInformeMenorIgual + "; ";

				DA = new SqlDataAdapter(consulta, Conexion);
				DA.Fill(DT);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
