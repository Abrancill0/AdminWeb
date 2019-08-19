using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI.Autorizaciones
{
    public class BrowseInformeAutorizaController : ApiController
    {
        public class Parametros1Informes
        {
            public int estatus { get; set; }
            public string uresponsable { get; set; }
            public int idempresa { get; set; }
            public string uconsulta { get; set; }
        }

        public class ObtieneInformeResult
        {
            public int i_id { get; set; }
            public int i_idproyecto { get; set; }
            public int i_ninforme { get; set; }
            public string i_nmb { get; set; }
            public int i_estatus { get; set; }
            public string e_estatus { get; set; }
            public string i_fcrea { get; set; }
            public string i_uresponsable { get; set; }
            public string responsable { get; set; }
            public string i_finicio { get; set; }
            public string i_ffin { get; set; }
            public double i_total { get; set; }
            public double i_totalg { get; set; }
            public int i_idempresa { get; set; }
            public string usuconsulta { get; set; }
            public int r_idrequisicion { get; set; }
			public double r_montorequisicion { get; set; } 

		}

        public List<ObtieneInformeResult> PostObtieneInformes(Parametros1Informes Datos)
        {
            SqlCommand comando = new SqlCommand("BrowseInformeAutoriza");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@estatus", SqlDbType.Int);
            comando.Parameters.Add("@uresponsable", SqlDbType.VarChar);
            //comando.Parameters.Add("@idempresa", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@estatus"].Value = Datos.estatus;
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.uresponsable);

            comando.Parameters["@uresponsable"].Value = UsuarioDesencripta;
            //comando.Parameters["@idempresa"].Value = Datos.idempresa;
            //comando.Parameters["@pid"].Direction = ParameterDirection.Output;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //ObtieneInformeResult items;

            List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

            if (DT.Rows.Count > 0)
            {
				// DataRow row = DT.Rows[0];
				string FechaInicio = "";
				string FechaFin = "";
				foreach (DataRow row in DT.Rows)
                {
					if (row["i_finicio"] != null && Convert.ToString(row["i_finicio"]) != "")
					{
						FechaInicio = Convert.ToDateTime(row["i_finicio"]).ToString("dd/MM/yyyy");//.ToShortDateString();
					}
					else
					{
						FechaInicio = "";
					}

					if (row["i_ffin"] != null && Convert.ToString(row["i_ffin"]) != "")
					{
						FechaFin = Convert.ToDateTime(row["i_ffin"]).ToString("dd/MM/yyyy");//.ToShortDateString();
					}
					else
					{
						FechaFin = "";
					}

					ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                        i_id = Convert.ToInt32(row["i_id"]),
                        i_ninforme = Convert.ToInt32(row["i_ninforme"]),
                        i_nmb = Convert.ToString(row["i_nmb"]),
                        i_estatus = Convert.ToInt32(row["i_estatus"]),
                        e_estatus = Convert.ToString(row["e_estatus"]),
                        i_fcrea = Convert.ToString(row["i_fcrea"]),
                        i_uresponsable = Convert.ToString(row["i_uresponsable"]),
                        responsable = Convert.ToString(row["responsable"]),
                        i_finicio = FechaInicio,
                        i_ffin = FechaFin,
                        i_total = Convert.ToDouble(row["i_total"]),
                        i_totalg = Convert.ToDouble(row["i_totalg"]),
                        r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]),
						r_montorequisicion = Convert.ToDouble(row["r_montorequisicion"])

					};

                    lista.Add(ent);
                }

                return lista;
            }
            else
            {
                return null;
            }
        }
    }
}
