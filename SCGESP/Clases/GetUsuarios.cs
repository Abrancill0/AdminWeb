using Ele.Generales;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
		internal static List<Resultado> Post(int VerSoloActivos)
		{
			try
			{
				List<Resultado> Resultado = new List<Resultado>();

				DataTable SelUsuarios = Usuarios(VerSoloActivos);
				DataTable SelEmpleados = Empleados();

				//SgUsuEmpleado, SgUsuActivo

				/*
				var ListUsuariosEmpleados = (from Usu in SelUsuarios.AsEnumerable()
									join Emp in SelEmpleados.AsEnumerable() on Convert.ToString(Usu["SgUsuEmpleado"]) equals Convert.ToString(Emp["GrEmpId"])
									//(string)Emp["GrEmpId"] = (string)Usu["SgUsuEmpleado"]
				select new
									{
										SgUsuId = (string)Usu["SgUsuId"],
										SgUsuEmpleado = (string)Usu["SgUsuEmpleado"],
										SgUsuNombre = (string)Usu["SgUsuNombre"],
										SgUsuActivo = (bool)Usu["SgUsuActivo"],
										GrEmpCentroNombre = (string)Emp["GrEmpCentroNombre"],
										GrEmpCentro = (string)Emp["GrEmpCentro"]
									}).ToList();
				DataTable DTUsuariosEmpleados = new DataTable();
				using (var reader = ObjectReader.Create(ListUsuariosEmpleados))
				{
					DTUsuariosEmpleados.Load(reader);
				}*/

				foreach (DataRow RowUsu in SelUsuarios.Rows)
				{
					string usu = Convert.ToString(RowUsu["SgUsuId"]).Trim();
					string idempleado = Convert.ToString(RowUsu["SgUsuEmpleado"]).Trim();
					string nombre = Convert.ToString(RowUsu["SgUsuNombre"]).Trim();
					string departamento = "";//Convert.ToString(RowUsu["GrEmpCentroNombre"]).Trim();
					string iddepartamento = "";//Convert.ToString(RowUsu["GrEmpCentro"]).Trim();

					try
					{
						if(idempleado != "")
						{
							DataTable Emp = SelEmpleados.AsEnumerable()
								.Where(w => Convert.ToString(w["GrEmpId"]) == idempleado)
								.CopyToDataTable();
							departamento = Convert.ToString(Emp.Rows[0]["GrEmpCentroNombre"]).Trim();
							iddepartamento = Convert.ToString(Emp.Rows[0]["GrEmpCentro"]).Trim();
						}
					}
					catch (Exception ex)
					{
						throw;
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
			catch (Exception ex)
			{

				return null;
			}
		}
		private static DataTable Usuarios(int VerSoloActivos)
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
				DataTable DTUsuarios1 = respuesta.obtieneTabla("Catalogo");
				if (VerSoloActivos == 1)
				{
					DTUsuarios = DTUsuarios1.AsEnumerable()
						.Where(w => Convert.ToString(w["SgUsuActivo"]) == "True")
						.CopyToDataTable();
				}
				else
				{
					DTUsuarios = DTUsuarios1;
				}
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