using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers
{
    public class ReportePorCategoriasController : ApiController
    {
		public class Parametros
		{
			public int IdRequisicion { get; set; }
			public int NInforme { get; set; }
			public string TipoFecha { get; set; }
			public string RepDe { get; set; }
			public string RepA { get; set; }
			public string UResponsable { get; set; }
			public string IdEmpleado { get; set; }
			public string UsuarioActivo { get; set; }
			public string Categoria { get; set; }
		}

		public class Resultado
		{
			public int Requisicion { get; set; }
			public int IdInforme { get; set; }
			public int Informe { get; set; }
			public string UResponsable { get; set; }
			public string NombreResponsabe { get; set; }
			public string Departamento { get; set; }
			public string TipoGasto { get; set; }
			public string Categoria { get; set; }
			public string Justificacion { get; set; }
			public int NGastos { get; set; }
			public decimal Total { get; set; }
			public string Estatus { get; set; }
			public string FGastoI { get; set; }
			public string FGastoF { get; set; }
			public string Periodo { get; set; }
		}
		public List<Resultado> Post(Parametros Datos) {
			try
			{
				string uConsulta = Seguridad.DesEncriptar(Datos.UsuarioActivo);
				Datos.RepDe += " 00:00:00";
				Datos.RepA += " 23:59:59";

				SqlDataAdapter DA;
				DataTable DT = new DataTable();

				SqlConnection Conexion = new SqlConnection
				{
					ConnectionString = VariablesGlobales.CadenaConexion
				};

				List<Resultado> Resultado = new List<Resultado>();
				string query = "SELECT " +
						" requisicion, idinforme, informe, uresponsable, nombreresponsabe, categoria, justificacion, ngastos, total, estatus, fgastoi, fgastof, periodo, i_fcrea " +
						" FROM vw_BrowseCategoriasInforme ";
				query += " WHERE ";

				query += "(";
				if (Datos.TipoFecha == "*")
				{
					query += "i_fcrea BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' ";
					query += "OR (fgastoi BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' OR fgastof BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "')";
				}
				else if (Datos.TipoFecha == "registro")
				{
					query += "i_fcrea BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "'";
				}
				else if (Datos.TipoFecha == "periodo")
				{
					query += "(fgastoi BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "' OR fgastof BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "')";
				}
				query += ") ";

				if (Datos.Categoria != "*")
				{
					query += "AND categoria LIKE '" + Datos.Categoria + "' ";
				}

				if (Datos.UResponsable != "*")
				{
					query += "AND uresponsable = '" + Datos.UResponsable + "' ";
				}

				query += " ORDER BY requisicion DESC, categoria ASC";


				DA = new SqlDataAdapter(query, Conexion);
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					//consulta requisiciones 
					DataTable DTRequisiciones = new DataTable();
					DocumentoSalida Requisiciones = BrowseRequisiciones(uConsulta, FormatFecha(Datos.RepDe), FormatFecha(Datos.RepA), Datos.IdEmpleado);
					if (Requisiciones.Resultado == "1")
					{
						DTRequisiciones = Requisiciones.obtieneTabla("Catalogo");
					}

					foreach (DataRow row in DT.Rows)
					{
						int idrequisicion = Convert.ToInt32(row["requisicion"]);
						string RmReqTipoGastoNombre = "";
						string RmReqCentroNombre = "";
						try
						{
							DataView DVRequisicion = SelecionaRequisicionId(DTRequisiciones, idrequisicion);
							RmReqTipoGastoNombre = Convert.ToString(DVRequisicion[0]["RmReqTipoGastoNombre"]);
							RmReqCentroNombre = Convert.ToString(DVRequisicion[0]["RmReqCentroNombre"]);
						}
						catch (Exception e)
						{
							//
						}

						Resultado ent = new Resultado
						{
							Requisicion = idrequisicion,
							IdInforme = Convert.ToInt32(row["idinforme"]),
							Informe = Convert.ToInt32(row["informe"]),
							UResponsable = Convert.ToString(row["uresponsable"]),
							NombreResponsabe = Convert.ToString(row["nombreresponsabe"]),
							TipoGasto = RmReqTipoGastoNombre,
							Departamento = RmReqCentroNombre,
							Categoria = Convert.ToString(row["categoria"]),
							Justificacion = Convert.ToString(row["justificacion"]),
							NGastos = Convert.ToInt32(row["ngastos"]),
							Total =  Convert.ToDecimal(row["total"]),
							Estatus = Convert.ToString(row["estatus"]),
							FGastoI = Convert.ToString(row["fgastoi"]),
							FGastoF = Convert.ToString(row["fgastof"]),
							Periodo = Convert.ToString(row["periodo"])
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
		private string FormatFecha(string fecha)
		{
			try
			{
				if (fecha != null && Convert.ToString(fecha) != "")
				{
					return Convert.ToDateTime(fecha).ToString("dd/MM/yyyy");//.ToShortDateString();
				}
				else
				{
					return "";
				}

			}
			catch (Exception)
			{

				return "";
			}
		}
		private DocumentoSalida BrowseRequisiciones(string uConsulta, string fechaInicial, string fechaFinal, string IdEmpleado)
		{
			try
			{
				DocumentoEntrada entrada = new DocumentoEntrada
				{
					Usuario = uConsulta, //Datos.Usuario;  
					Origen = "AdminWEB",  //Datos.Origen; 
					Transaccion = 120760,
					Operacion = 1
				};
				//entrada.agregaElemento("RmTirRutaProceso", Convert.ToInt32(4));
				entrada.agregaElemento("FechaInicial", fechaInicial);//fechaInicial.ToString("dd/MM/yyyy")
				entrada.agregaElemento("FechaFinal", fechaFinal);
				if (Convert.ToInt32(IdEmpleado) > 0)
				{
					entrada.agregaElemento("RmReqSolicitante", Convert.ToInt32(IdEmpleado));
				}
				
				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
				return respuesta;
			}
			catch (Exception)
			{

				throw;
			}
		}
		private DataView SelecionaRequisicionId(DataTable Requisiciones, int idrequisicion)
		{
			try
			{
				string expression = "RmReqId = " + idrequisicion;
				DataView Requisicion = new DataView(Requisiciones)
				{
					RowFilter = expression
				};
				return Requisicion;
			}
			catch (Exception)
			{
				throw;
			}
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
