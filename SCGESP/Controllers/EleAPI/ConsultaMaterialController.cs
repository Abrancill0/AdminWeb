using SCGESP.Clases;
using System.Xml;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers.EleAPI
{
    public class ConsultaMaterialController : ApiController
    {
        public class datos
        {
            public int RmRdeRequisicion { get; set; }
            public int TipoRequisicion { get; set; }
            public Boolean valida { get; set; }
        }

        public class Result
        {
            public int GrMatId { get; set; }
            public string GrMatNombre { get; set; }
            public double GrMatPrecio { get; set; }
            public double GrMatIva { get; set; }
            public int GrMatGrupo { get; set; }
            public string GrGmaCuentaAdquisicion { get; set; }
            public int GrMatUnidadMedida { get; set; }
        }

        public List<Result> PostObtieneInformes(datos Datos)
        {

            SqlCommand comando = new SqlCommand("ConsultaMaterial");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@RmRdeRequisicion", SqlDbType.Int);
            comando.Parameters.Add("@TipoRequisicion", SqlDbType.Int);
            comando.Parameters.Add("@valida", SqlDbType.Bit);

            //Asignacion de valores a parametros
            comando.Parameters["@RmRdeRequisicion"].Value = Datos.RmRdeRequisicion;
            comando.Parameters["@TipoRequisicion"].Value = Datos.TipoRequisicion;
            comando.Parameters["@valida"].Value = Datos.valida;

            comando.Connection = new SqlConnection(""); ///VariablesGlobales.CadenaConexionEle);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //ObtieneInformeResult items;

            List<Result> lista = new List<Result>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    Result ent = new Result
                    {
                        GrMatId = Convert.ToInt16(row["GrMatId"]),
                        GrMatNombre = Convert.ToString(row["GrMatNombre"]),
                        GrMatPrecio = Convert.ToDouble(row["GrMatPrecio"]),
                        GrMatIva = Convert.ToDouble(row["GrMatIva"]),
                        GrMatGrupo = Convert.ToInt32(row["GrMatGrupo"]),
                        GrGmaCuentaAdquisicion = Convert.ToString(row["GrGmaCuentaAdquisicion"]),
                        GrMatUnidadMedida = Convert.ToInt32(row["GrMatUnidadMedida"])
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
