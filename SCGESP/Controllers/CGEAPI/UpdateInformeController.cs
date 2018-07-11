using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class UpdateInformeController : ApiController
    {
        public class datos
        {
            public int id { get; set; }
            public int idproyecto { get; set; }
            public string motivo { get; set; }
            public string i_nmb { get; set; }
            public string umodifico { get; set; }
            public string notas { get; set; }
            public int idempresa { get; set; }
            public string proycontable { get; set; }
            public int tipo { get; set; }
            public DateTime mes { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string MSN { get; set; }
            public int id { get; set; }
            public int idempresa { get; set; }
        }

        public IEnumerable<ObtieneInformeResult> Post(datos Datos)
        {
            SqlCommand comando = new SqlCommand("UpdateInforme");
            comando.CommandType = CommandType.StoredProcedure;
            //DateTime dt = Datos.mes;
            string Mes = Datos.mes.ToString("dd-MM-yyyy");

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@motivo", SqlDbType.VarChar);
            comando.Parameters.Add("@i_nmb", SqlDbType.VarChar);
            comando.Parameters.Add("@umodifico", SqlDbType.VarChar);
            comando.Parameters.Add("@notas", SqlDbType.VarChar);
            comando.Parameters.Add("@idempresa", SqlDbType.Int);
            comando.Parameters.Add("@proycontable", SqlDbType.VarChar);
            comando.Parameters.Add("@tipo", SqlDbType.Int);
            comando.Parameters.Add("@mes", SqlDbType.VarChar);

            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.id;
            comando.Parameters["@motivo"].Value = Datos.motivo;
            comando.Parameters["@i_nmb"].Value = Datos.i_nmb;
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.umodifico);

            comando.Parameters["@umodifico"].Value = UsuarioDesencripta;
            comando.Parameters["@notas"].Value = Datos.notas;
            comando.Parameters["@idempresa"].Value = Datos.idempresa;
            comando.Parameters["@proycontable"].Value = Datos.proycontable;
            comando.Parameters["@tipo"].Value = Datos.tipo;
            comando.Parameters["@mes"].Value = Mes;
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
                        MSN = Convert.ToString(row["MSN"])
                        //id = Convert.ToInt32(row["id"]),
                        //idproyecto = Convert.ToInt32(row["idproyecto"]),
                        //idempresa = Convert.ToInt32(row["idempresa"])
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