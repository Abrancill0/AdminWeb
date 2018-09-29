using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class CuentaNumeroInformesAppController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
           
        }

        public class ParametrosSalida
        {
            public int NumeroInformes { get; set; }
            public string Mensaje { get; set; }
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ParametrosSalida> Post(ParametrosEntrada Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("CountInformeApp");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);
                //comando.Parameters.Add("@idempresa", SqlDbType.Int);

                //Asignacion de valores a parametros
                comando.Parameters["@uconsulta"].Value = Datos.Usuario;

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

                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            NumeroInformes = Convert.ToInt32(row["NumeroInformes"]),
                            Mensaje = "OK"
                        };

                        lista.Add(ent);
                    }
                    return lista;
                }
                else
                {
                    ParametrosSalida ent = new ParametrosSalida
                    {
                        NumeroInformes = 0,
                         Mensaje = "No se encontro nada"
                    };

                    lista.Add(ent);

                    return lista;
                }


            }
            catch (Exception ex)
            {

                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                ParametrosSalida ent = new ParametrosSalida
                {
                    NumeroInformes = 0,
                    Mensaje = ex.ToString()
                };

                lista.Add(ent);

                return lista;

            }
        }
    }
}
