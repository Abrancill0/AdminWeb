using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class SelectInformeController : ApiController
    {
        public class ListResult
        { 
            public int    i_id { get; set; }
            public int    i_idproyecto { get; set; }
            public int    i_ninforme { get; set; }
            public string p_nmb { get; set; }
            public string proyectocontable { get; set; }
            public string i_viaje { get; set; }
            public string i_motivo { get; set; }
            public string i_notas { get; set; }
            public string i_autorizado { get; set; }
            public string i_comentarioaut { get; set; }
            public string i_uautoriza { get; set; }
            public int    i_estatus { get; set; }
            public string e_estatus { get; set; }
            public string i_uresponsable { get; set; }
            public string responsable { get; set; }
            public string del { get; set; }
            public string al { get; set; }
            public double i_total { get; set; }
            public double i_totalg { get; set; }
            public double reembolso { get; set; }
            public double anticipo { get; set; }
            public double disponible { get; set; }
            public double ppagarares { get; set; }
            public double i_tsreembolso { get; set; }
            public double i_tnreembolso { get; set; }
            public int    i_idempresa { get; set; }
            public int    i_conciliacionxml { get; set; }
            public int    i_conciliacionbancos { get; set; }
            public int    i_conciliacionconvenios { get; set; }
            public int    i_contabilizar { get; set; }
            public int    i_tipo { get; set; }
            public string clavetipo { get; set; }
            public string tipo { get; set; }
            public string i_mescontable { get; set; }
            public int    r_idrequisicion { get; set; }
            public double r_montorequisicion { get; set; }
            public double PagarResponsable { get; set; }
            public double MontoGastado { get; set; }
            public double Disponible { get; set; }
            public int    DesactivaControl { get; set; }
            public int    rechazado { get; set; }
            public string i_tarjetatoka { get; set; }
			public string hAutorizarComprobar { get; set; }
			public string comentario_1 { get; set; }
			public string comentario_2 { get; set; }
			public string comentario_3 { get; set; }
			public string comentario_4 { get; set; }
			public int esvobo { get; set; }
			public int esvobo_2 { get; set; }
			public int idinforme_2 { get; set; }
			public int autorizador_final { get; set; }


		}

        public class datos
        {
            public int id { get; set; }
        }

        public IEnumerable<ListResult> Post(datos dato)
        {

            SqlCommand comando = new SqlCommand("SelectInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = dato.id;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            // comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);


            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                string FechaInicio = "";
                string FechaFin = "";
                foreach (DataRow row in DT.Rows)
                {

                    if (row["del"] != null && Convert.ToString(row["del"]) != "")
                    {
                        FechaInicio = Convert.ToDateTime(row["del"]).ToShortDateString();
                    }
                    else
                    {
                        FechaInicio = "";
                    }

                    if (row["al"]!= null && Convert.ToString(row["al"]) != "")
                    {
                        FechaFin = Convert.ToDateTime(row["al"]).ToShortDateString();
                    }
                    else
                    {
                        FechaFin = "";
                    }
                    
                    
                    ListResult ent = new ListResult
                    {
                         i_id = Convert.ToInt32(row["i_id"]),
                         i_ninforme = Convert.ToInt32(row["i_ninforme"]),
                         p_nmb = Convert.ToString(row["i_nmb"]),
                         i_motivo = Convert.ToString(row["i_motivo"]),
                         i_notas = Convert.ToString(row["i_notas"]),
                         i_autorizado = Convert.ToString(row["i_autorizado"]),
                         i_comentarioaut = Convert.ToString(row["i_comentarioaut"]),
                         i_uautoriza = Convert.ToString(row["i_uautoriza"]),
                         e_estatus = Convert.ToString(row["e_estatus"]),
                         i_uresponsable = Convert.ToString(row["i_uresponsable"]),
                         responsable = Convert.ToString(row["Responsable"]),
                         del = Convert.ToString(FechaInicio),
                         al = Convert.ToString(FechaFin),
                         i_total = Convert.ToDouble(row["i_total"]),
                         i_totalg = Convert.ToDouble(row["i_totalg"]),
                         proyectocontable = Convert.ToString(row["proyectocontable"]),
                         i_tsreembolso = Convert.ToDouble(row["i_tsreembolso"]),
                         i_tnreembolso = Convert.ToDouble(row["i_tnreembolso"]),
                         i_idempresa = Convert.ToInt32(row["i_idempresa"]),
                         i_conciliacionxml = Convert.ToInt32(row["i_conciliacionxml"]),
                         i_conciliacionbancos = Convert.ToInt32(row["i_conciliacionbancos"]),
                         i_conciliacionconvenios = Convert.ToInt32(row["i_conciliacionconvenios"]),
                         i_contabilizar = Convert.ToInt32(row["i_contabilizar"]), 
                         i_tipo = Convert.ToInt32(row["i_tipo"]),
                         r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]),
                         r_montorequisicion = Convert.ToDouble(row["r_montorequisicion"]),
                         PagarResponsable = Convert.ToDouble(row["PagarResponsable"]),
                         MontoGastado = Convert.ToDouble(row["MontoGastado"]),
                         Disponible = Convert.ToDouble(row["Disponible"]),
                         i_estatus = Convert.ToInt32(row["i_estatus"]),
                        DesactivaControl = Convert.ToInt32(row["DesactivaControl"]),
                        rechazado = Convert.ToInt32(row["i_rechazado"]),
                        i_tarjetatoka = Convert.ToString(row["i_tarjetatoka"]),
						hAutorizarComprobar = Convert.ToString(row["hAutorizarComprobar"]),
						comentario_1 = Convert.ToString(row["i_comentario_1"]),
						comentario_2 = Convert.ToString(row["i_comentario_2"]),
						comentario_3 = Convert.ToString(row["i_comentario_3"]),
						comentario_4 = Convert.ToString(row["i_comentario_4"]),
						esvobo = Convert.ToInt16(row["esvobo"]),
						esvobo_2 = Convert.ToInt16(row["esvobo_2"]),
						idinforme_2 = Convert.ToInt16(row["idinforme_2"]),
						autorizador_final = Convert.ToInt16(row["autorizador_final"])
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
