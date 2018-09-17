using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class GuardaTokenController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string neq_equipo { get; set; }
            public int neq_id_usuario { get; set; }
            public string neq_dispositivo { get; set; }
            public int neq_app_id { get; set; }
            public DateTime neq_fecha_hora_creo { get; set; }
            public DateTime neq_fecha_hora_modifico { get; set; }
        }

        public class ParametrosSalida
        {
            public string Mensaje { get; set; }
            public int Usuario { get; set; }
            public string Token { get; set; }
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ParametrosSalida> Post(ParametrosEntrada Datos)
        {
            try
            {

                SqlCommand comando = new SqlCommand("TokenNotification");
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@neq_equipo", SqlDbType.VarChar);
                comando.Parameters.Add("@neq_id_usuario", SqlDbType.Int);
                comando.Parameters.Add("@neq_dispositivo", SqlDbType.VarChar);
                comando.Parameters.Add("@neq_app_id", SqlDbType.Int);
                comando.Parameters.Add("@neq_fecha_hora_creo", SqlDbType.VarChar);
                comando.Parameters.Add("@neq_fecha_hora_modifico", SqlDbType.VarChar);

                //Asignacion de valores a parametros
                comando.Parameters["@neq_equipo"].Value = Datos.neq_equipo;
                comando.Parameters["@neq_id_usuario"].Value = Datos.neq_id_usuario;
                comando.Parameters["@neq_dispositivo"].Value = Datos.neq_dispositivo;
                comando.Parameters["@neq_app_id"].Value = Datos.neq_app_id;
                comando.Parameters["@neq_fecha_hora_creo"].Value = Datos.neq_fecha_hora_creo;
                comando.Parameters["@neq_fecha_hora_modifico"].Value = Datos.neq_fecha_hora_modifico;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);


                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            Mensaje = Convert.ToString(row["Mensaje"]),
                            Usuario = Convert.ToInt32(row["Usuario"]),
                            Token = Convert.ToString(row["Token"])
                        };

                        lista.Add(ent);
                    }

                    return lista;

                }
                else
                {
                    ParametrosSalida ent = new ParametrosSalida
                    {
                        Mensaje = "Error al realizar el guardado/Actualizacion",
                        Usuario = 0,
                        Token = ""
                    };

                    lista.Add(ent);


                    return lista;
                }
            }
            catch (System.Exception ex)
            {
                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                ParametrosSalida ent = new ParametrosSalida
                {
                    Mensaje = ex.ToString(),
                    Usuario = 0,
                    Token = ""
                };

                lista.Add(ent);

                return lista;

            }

        }
    }
}
