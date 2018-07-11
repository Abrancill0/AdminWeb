using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class DeleteGastoController : ApiController
    {
        public class datos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string mensaje { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(datos Datos)
        {
            //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.ugasto);

            SqlCommand comando = new SqlCommand("DeleteGasto");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.id;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
           
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
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                        mensaje = Convert.ToString(row["ELIMINADO"]),
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
