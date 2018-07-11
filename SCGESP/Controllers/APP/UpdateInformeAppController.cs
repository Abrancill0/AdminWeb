using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{
    public class UpdateInformeAppController : ApiController
    {
        public class datos
        {
            public int id { get; set; }
            public string motivo { get; set; }
            public string i_nmb { get; set; }
            public string umodifico { get; set; }
            public string notas { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string MSN { get; set; }
            public int id { get; set; }
        }

        public IEnumerable<ObtieneInformeResult> Post(datos Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("UpdateInformeApp");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@id", SqlDbType.Int);
                comando.Parameters.Add("@motivo", SqlDbType.VarChar);
                comando.Parameters.Add("@i_nmb", SqlDbType.VarChar);
                comando.Parameters.Add("@umodifico", SqlDbType.VarChar);
                comando.Parameters.Add("@notas", SqlDbType.VarChar);

                //Asignacion de valores a parametros
                comando.Parameters["@id"].Value = Datos.id;
                comando.Parameters["@motivo"].Value = Datos.motivo;
                comando.Parameters["@i_nmb"].Value = Datos.i_nmb;
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.umodifico);
                comando.Parameters["@umodifico"].Value = UsuarioDesencripta;
                comando.Parameters["@notas"].Value = Datos.notas;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();


                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        ObtieneInformeResult ent = new ObtieneInformeResult
                        {
                            MSN = Convert.ToString(row["MSN"]),
                            id = Convert.ToInt32(row["id"])
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
                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

                ObtieneInformeResult ent = new ObtieneInformeResult
                {
                    MSN = Convert.ToString(ex.ToString()),
                    id = Convert.ToInt32(0)
                };

                lista.Add(ent);

                return lista;
            }
            
        }
    }
}
