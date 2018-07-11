using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Web.Http;

namespace SCGESP.Controllers
{
    public class GuardaInformeController : ApiController
    {
         public class datos
            {
                public int idproyecto { get; set; }
                public int ucrea { get; set; }
                public int uresponsable { get; set; }
                public string motivo { get; set; }
                public string viaje { get; set; }
                public double pago { get; set; }
                public int estatus { get; set; }
                public string idapp { get; set; }
                public int tipo { get; set; }
                public int idempresa { get; set; }
                public string mes { get; set; }
        }

         public class ObtieneInformeResult
            {
                public string MSN { get; set; }
                public int NINFORME { get; set; }
                public int idproyecto { get; set; }
                public int idinforme { get; set; }
            }

            public IEnumerable<ObtieneInformeResult> Post(datos Datos)
            {
                SqlCommand comando = new SqlCommand("InsertInforme");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@idproyecto", SqlDbType.Int);
                comando.Parameters.Add("@ucrea", SqlDbType.Int);
                comando.Parameters.Add("@uresponsable", SqlDbType.Int);
                comando.Parameters.Add("@motivo", SqlDbType.VarChar);
                comando.Parameters.Add("@viaje", SqlDbType.VarChar);
                comando.Parameters.Add("@pago", SqlDbType.Float);
                comando.Parameters.Add("@estatus", SqlDbType.Int);
                comando.Parameters.Add("@idapp", SqlDbType.VarChar);
                comando.Parameters.Add("@tipo", SqlDbType.Int);
                comando.Parameters.Add("@idempresa", SqlDbType.Int);
                comando.Parameters.Add("@mes", SqlDbType.Date);

                //Asignacion de valores a parametros
                comando.Parameters["@idproyecto"].Value = Datos.idproyecto;
                comando.Parameters["@ucrea"].Value = Datos.ucrea;
                comando.Parameters["@uresponsable"].Value = Datos.uresponsable;
                comando.Parameters["@motivo"].Value = Datos.motivo;
                comando.Parameters["@viaje"].Value = Datos.viaje;
                comando.Parameters["@pago"].Value = Datos.pago;
                comando.Parameters["@estatus"].Value = Datos.estatus;
                comando.Parameters["@idapp"].Value = Datos.idapp;
                comando.Parameters["@tipo"].Value = Datos.tipo;
                comando.Parameters["@idempresa"].Value = Datos.idempresa;
                comando.Parameters["@mes"].Value = Datos.mes;
            //comando.Parameters["@pid"].Direction = ParameterDirection.Output;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

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
                        MSN = Convert.ToString(row["CREADO"]),
                        NINFORME = Convert.ToInt32(row["NINFORME"]),
                        idproyecto = Convert.ToInt32(row["idproyecto"]),
                        idinforme = Convert.ToInt32(row["idinforme"])
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