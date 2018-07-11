using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.EleAPI
{
    public class FormaPagoController : ApiController
    {
        public class datos
        {
            public string GrEmpID { get; set; }
        }

        public class Result
        {
            public string GrEmpTarjetaToka { get; set; }
        }

        public List<Result> PostObtieneInformes(datos Datos)
        {
            SqlCommand comando = new SqlCommand("FormaPago");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@GrEmpID", SqlDbType.VarChar);

            string EmpleadoDesencripta=Seguridad.DesEncriptar(Datos.GrEmpID);

            //Asignacion de valores a parametros
            comando.Parameters["@GrEmpID"].Value = EmpleadoDesencripta;

            comando.Connection = new SqlConnection("");//VariablesGlobales.CadenaConexionEle);
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
                        GrEmpTarjetaToka = Convert.ToString(row["GrEmpTarjetaToka"])
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

