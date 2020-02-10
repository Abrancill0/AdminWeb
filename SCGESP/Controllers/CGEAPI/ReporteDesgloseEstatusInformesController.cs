using SCGESP.Clases;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class ReporteDesgloseEstatusInformesController : ApiController
	{
		public class Parametros
		{
			public int IdRequisicion { get; set; }
			public int NoInforme { get; set; }
			public string TipoFecha { get; set; }
			public string RepDe { get; set; }
			public string RepA { get; set; }
			public string UResponsable { get; set; }
			public string IdEmpleado { get; set; }
			public string UsuarioActivo { get; set; }
			public string Estatus { get; set; }
			public int VerEstatusAdminERP { get; set; }
		}
		public class Resultado
		{
			public int Requisicion { get; set; }
			public int IdInforme { get; set; }
			public int Informe { get; set; }
			public string UResponsable { get; set; }
			public string NombreResponsabe { get; set; }
			public string Justificacion { get; set; }
			public string EstatusActual { get; set; }
			public List<ListEstatus> Estatus { get; set; }
		}
		public class ListEstatus
		{
			public string Estatus { get; set; }
			public int Actual { get; set; }
			public string Usuario { get; set; }
			public string NombreUsurio { get; set; }
			public string Fecha { get; set; }
		}
		public List<Resultado> Post(Parametros Datos)
		{
			try
			{
				List<Resultado> Resultado = new List<Resultado>();
				Datos.RepDe += " 00:00:00";
				Datos.RepA += " 23:59:59";
				SqlCommand comando = new SqlCommand("BrowseDesgloseEstatusInforme")
				{
					CommandType = CommandType.StoredProcedure
				};

				//Declaracion de parametros
				comando.Parameters.Add("@IdInforme", SqlDbType.Int);
				comando.Parameters.Add("@NoInforme", SqlDbType.Int);
				comando.Parameters.Add("@IdRequisicion", SqlDbType.VarChar);
				comando.Parameters.Add("@Usuario", SqlDbType.VarChar);
				comando.Parameters.Add("@TipoFecha", SqlDbType.VarChar);
				comando.Parameters.Add("@RepDe", SqlDbType.VarChar);
				comando.Parameters.Add("@RepA", SqlDbType.VarChar);

				//Asignacion de valores a parametros
				comando.Parameters["@IdInforme"].Value = 0;
				comando.Parameters["@NoInforme"].Value = Datos.NoInforme;
				comando.Parameters["@IdRequisicion"].Value = Datos.IdRequisicion;
				comando.Parameters["@Usuario"].Value = Datos.UResponsable;
				comando.Parameters["@TipoFecha"].Value = Datos.TipoFecha;
				comando.Parameters["@RepDe"].Value = Datos.RepDe;
				comando.Parameters["@RepA"].Value = Datos.RepA;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
				comando.CommandTimeout = 0;
				comando.Connection.Open();

				DataTable DT = new DataTable();
				SqlDataAdapter DA = new SqlDataAdapter(comando);
				comando.Connection.Close();
				DA.Fill(DT);

				if (DT.Rows.Count > 0)
				{
					var SelUsuarios = GetUsuarios.Post();

					foreach (DataRow row in DT.Rows)
					{
						Resultado res = new Resultado
						{
							Requisicion = Convert.ToInt32(row["IdRequisicion"]),
							IdInforme = Convert.ToInt32(row["IdInforme"]),
							Informe = Convert.ToInt32(row["NInforme"]),
							Justificacion = Convert.ToString(row["Justificacion"]),
							EstatusActual = Convert.ToString(row["EstatusActual"]),
							UResponsable = Convert.ToString(row["UResponsable"]).Trim(),
							NombreResponsabe = Convert.ToString(row["NombreResponsable"])
						};

						int nres = Resultado.Where(x => x.IdInforme == res.IdInforme).Count();
						//var listr = from x in Resultado where x.IdInforme == res.IdInforme select x;
						
						if (nres == 0)
							Resultado.Add(res);
					}

					for (int i = 0; i < Resultado.Count; i++)
					{
						string nmbUsuario = "";
						try
						{
							var rUsuario = SelUsuarios.Where(x => x.Usuario.Trim() == Resultado[i].UResponsable).First();
							nmbUsuario = rUsuario.Nombre != "" ? rUsuario.Nombre : Resultado[i].UResponsable;
						}
						catch (Exception)
						{
							nmbUsuario = Resultado[i].UResponsable;
						}
						
						Resultado[i].NombreResponsabe = nmbUsuario;

						string EstActual = Resultado[i].EstatusActual;
						string _sqlWhere = "IdInforme = " + Resultado[i].IdInforme + "";
						string _sqlOrder = "Orden ASC";
						DataTable DTEstatus = DT.Select(_sqlWhere, _sqlOrder).CopyToDataTable();
						List<ListEstatus> Estatus = new List<ListEstatus>();
						foreach (DataRow row in DTEstatus.Rows) {
							string estatus = Convert.ToString(row["Estatus"]);
							string usuario = Convert.ToString(row["Usuario"]).Trim();

							try
							{
								var rUsuario = SelUsuarios.Where(x => x.Usuario.Trim() == usuario).First();
								nmbUsuario = rUsuario.Nombre != "" ? rUsuario.Nombre : usuario;
							}
							catch (Exception)
							{
								nmbUsuario = usuario;
							}
							ListEstatus est = new ListEstatus {
								Actual = EstActual == estatus ? 1 : 0,
								Estatus = estatus,
								Usuario = usuario,
								NombreUsurio = nmbUsuario,
								Fecha = row["Fecha"] is DBNull ? "" : Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy hh:mm:ss")
							};
							Estatus.Add(est);
						}
						Resultado[i].Estatus = Estatus;
					}
				}
				return Resultado;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
