using Ele.Generales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace SCGESP.Clases
{
	public static class GetUsuarios
	{
		public class Resultado
		{
			public string Usuario { get; set; }
			public string IdEmpleado { get; set; }
			public string Nombre { get; set; }
			public string IdDepartamento { get; set; }
			public string Departamento { get; set; }
		}
		internal static List<Resultado> Post()
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
					DataTable SelUsuarios = Usuarios();
					DataTable SelEmpleados = Empleados();
					//SgUsuEmpleado
					foreach (DataRow row in DT.Rows)
					{
						string usu = Convert.ToString(row["usuario"]).Trim();
						string idempleado = "";//Convert.ToString(SelUsuarios[0]["SgUsuEmpleado"]).Trim()
						string nombre = Convert.ToString(row["nombre"]).Trim();
						string departamento = "";
						string iddepartamento = "";
						try
						{
							if (usu != "")
							{
								//SgUsuEmpleado GrEmpId
								DataView DVSelUsuarios = new DataView(SelUsuarios)
								{
									RowFilter = "SgUsuId = '" + usu + "'"
								};
								idempleado = Convert.ToString(DVSelUsuarios[0]["SgUsuEmpleado"]).Trim();
								nombre = Convert.ToString(DVSelUsuarios[0]["SgUsuNombre"]).Trim();

								DataView DVSelEmpleados = new DataView(SelEmpleados)
								{
									RowFilter = "GrEmpId = '" + idempleado + "'"
								};
								departamento = Convert.ToString(DVSelEmpleados[0]["GrEmpCentroNombre"]).Trim();
								iddepartamento = Convert.ToString(DVSelEmpleados[0]["GrEmpCentro"]).Trim();
							}
						}
						catch (Exception ex)
						{
							var er = ex;
						}

						Resultado ent = new Resultado
						{
							Usuario = usu,
							IdEmpleado = idempleado,
							Nombre = nombre,
							IdDepartamento = iddepartamento,
							Departamento = departamento
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
		private static DataTable Usuarios()
		{
			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = "imartinez",
				Origen = "AdminWEB",  //Datos.Origen; 
				Transaccion = 100004,
				Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
			};

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
			DataTable DTUsuarios = new DataTable();
			if (respuesta.Resultado == "1")
			{
				DTUsuarios = respuesta.obtieneTabla("Catalogo");
			}
			//DataView DVUsuarios = new DataView(DTUsuarios);
			return DTUsuarios;
		}
		private static DataTable Empleados()
		{
			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = "imartinez",
				Origen = "AdminWEB",  //Datos.Origen; 
				Transaccion = 120037,
				Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
			};

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
			DataTable DTEmpleados = new DataTable();
			if (respuesta.Resultado == "1")
			{
				DTEmpleados = respuesta.obtieneTabla("Catalogo");
			}
			//DataView DVEmpleados = new DataView(DTEmpleados);
			return DTEmpleados;
		}
		private static DocumentoSalida PeticionCatalogo(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp();
			ws.Timeout = -1;
			string respuesta = ws.PeticionCatalogo(doc);
			return new DocumentoSalida(respuesta);
		}
	}
}