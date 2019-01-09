using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class ActualizaPendientesAppController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string Pendientes { get; set; }
            public string Token { get; set; }

        }

        public class ObtieneParametrosSalida
        {
            public string Resultado { get; set; }
        }

        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
                try
                {

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();
                
                SqlCommand comando = new SqlCommand("ActualizaPendientesNotificaciones");
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.Add("@Usuario", SqlDbType.VarChar);
                    comando.Parameters.Add("@TokenID", SqlDbType.VarChar);
                    comando.Parameters.Add("@Pendientes", SqlDbType.Int);

                    comando.Parameters["@Usuario"].Value = Datos.Usuario;
                    comando.Parameters["@TokenID"].Value = Datos.Token;
                    comando.Parameters["@Pendientes"].Value = Datos.Pendientes;

                    comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando.CommandTimeout = 0;
                    comando.Connection.Open();

                    DataTable DT = new DataTable();
                    SqlDataAdapter DA = new SqlDataAdapter(comando);
                    comando.Connection.Close();
                    DA.Fill(DT);

                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow row in DT.Rows)
                        {
                            ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                            {
                                Resultado = Convert.ToString(row["Pendientes"]),
                               
                            };

                            lista.Add(ent);

                        };

                        return lista;
                    }
                    else
                    {
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {
                            Resultado = "No se pudo actualizar los pendientes",
                          
                        };

                        lista.Add(ent);

                        return lista;
                    }

                }
                catch (Exception ex)
                {

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        Resultado = ex.ToString()
                    };

                    lista.Add(ent);

                    return lista;
                }

            
        }


    }
}
