using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class SelectDatosAdicionalesController : ApiController
    {
        public class datos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
            public int idproyecto { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string g_rfc { get; set; }
            public string g_contacto { get; set; }
            public string g_telefono { get; set; }
            public string g_correo { get; set; }
            public int ncomensales { get; set; }
            public string nmbcomensales { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(datos Datos)
        {
            //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.ugasto);

            SqlCommand comando = new SqlCommand("selectDatosAdicionales");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@idproyecto", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.id;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@idproyecto"].Value = Datos.idproyecto;

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
                        g_rfc = Convert.ToString(row["g_rfc"]),
                        g_contacto = Convert.ToString(row["g_contacto"]),
                        g_telefono = Convert.ToString(row["g_telefono"]),
                        g_correo = Convert.ToString(row["g_correo"]),
                        ncomensales = Convert.ToInt32(row["g_ncomensales"]),
                        nmbcomensales = Convert.ToString(row["g_nmbcomensales"])
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