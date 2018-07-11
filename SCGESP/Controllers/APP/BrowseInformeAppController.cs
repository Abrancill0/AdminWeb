using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{
    public class BrowseInformeAppv2Controller : ApiController
    {
        public class Parametros1Informes
        {
            public int estatus { get; set; }
            public string uresponsable { get; set; }
            public string uconsulta { get; set; }
            public string empleadoactivo { get; set; }
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
            public string anticipo { get; set; }
            public string disponible { get; set; }
      
            public string p_nmb { get; set; }
            public string i_motivo { get; set; }
            public string i_notas { get; set; }
            public string proycontable { get; set; }
            public string i_tipo { get; set; }
            public string i_mescontable { get; set; }
            public string i_tarjetatoka { get; set; }
            public double MontoRequisicion { get; set; }
            public string PruebaAPP { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(Parametros1Informes Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.uresponsable);
            string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.empleadoactivo);
            return ObtieneInformesActuales(Datos.estatus, UsuarioDesencripta);
        }

      
        public List<ObtieneInformeResult> ObtieneInformesActuales(int status, string usuario)
        {
            try
            {
                SqlCommand comando = new SqlCommand("BrowseInformeApp");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@uresponsable", SqlDbType.VarChar);
                comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);
                //comando.Parameters.Add("@idempresa", SqlDbType.Int);

                //Asignacion de valores a parametros
                comando.Parameters["@uresponsable"].Value = usuario;
                comando.Parameters["@uconsulta"].Value = usuario;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                //ObtieneInformeResult items;

                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();


                if (DT.Rows.Count > 0)
                {
                    string FechaInicio = "";
                    string FechaFin = "";

                    // DataRow row = DT.Rows[0];
                    foreach (DataRow row in DT.Rows)
                    {
                        if (row["i_finicio"] != null && Convert.ToString(row["i_finicio"]) != "")
                        {
                            FechaInicio = Convert.ToDateTime(row["i_finicio"]).ToShortDateString();
                        }
                        else
                        {
                            FechaInicio = "";
                        }

                        if (row["i_ffin"] != null && Convert.ToString(row["i_ffin"]) != "")
                        {
                            FechaFin = Convert.ToDateTime(row["i_ffin"]).ToShortDateString();
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
                            i_finicio = Convert.ToString(FechaInicio),
                            i_ffin = Convert.ToString(FechaFin),
                            i_total = Convert.ToDouble(row["i_total"]),
                            i_totalg = Convert.ToDouble(row["i_totalg"]),
                            r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]),
                            usuconsulta = Convert.ToString(row["usuconsulta"]),
                            i_motivo = Convert.ToString(row["i_motivo"]),
                            i_notas = Convert.ToString(row["i_notas"]),
                            i_tipo = Convert.ToString(row["i_tipo"]),
                            i_tarjetatoka = Convert.ToString(row["i_tarjetatoka"]),
                            MontoRequisicion = Convert.ToDouble(row["r_montorequisicion"]),
                            PruebaAPP = "Test123"
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
            catch (Exception ex)
            {
              
                return null;

            }

            
        }

      
    }
}

