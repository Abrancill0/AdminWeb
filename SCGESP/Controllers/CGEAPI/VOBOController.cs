using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class VOBOv2Controller : ApiController
    {
        public class ParametrosVOBO
        {
            public string c_accion { get; set; }
        }

        public class ObtieneVOBOResult
        {
            public string c_usuario_nombre { get; set; }
            public string c_correo { get; set; }
            public string c_accion { get; set; }
            public int c_valor_default { get; set; }
            public int c_chk_bloqueado { get; set; }
            public string c_duracion_ini { get; set; }
            public string c_duracion_fin { get; set; }
 
        }

        public List<ObtieneVOBOResult> Post(string c_accion)
        {
            try
            {
                SqlCommand comando = new SqlCommand("ObtieneVOBO");//ok
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@c_accion", SqlDbType.VarChar);

                //Asignacion de valores a parametros
                comando.Parameters["@c_accion"].Value = c_accion;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                //ObtieneInformeResult items;

                List<ObtieneVOBOResult> lista = new List<ObtieneVOBOResult>();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {

                        ObtieneVOBOResult ent = new ObtieneVOBOResult
                        {
                            c_usuario_nombre = Convert.ToString(row["c_usuario_nombre"]),
                            c_correo = Convert.ToString(row["c_correo"]),
                            c_accion = Convert.ToString(row["c_usuario"]),
                            c_valor_default = Convert.ToInt32(row["c_valor_default"]),
                            c_chk_bloqueado = Convert.ToInt32(row["c_chk_bloqueado"]),
                            c_duracion_ini = Convert.ToString(row["c_duracion_ini"]),
                            c_duracion_fin = Convert.ToString(row["c_duracion_fin"]),

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
                List<ObtieneVOBOResult> lista = new List<ObtieneVOBOResult>();
                ObtieneVOBOResult ent = new ObtieneVOBOResult
                {
                    c_usuario_nombre = ex.ToString(),

                };

                lista.Add(ent);

                return lista;
            }
            

        }


    }
}
