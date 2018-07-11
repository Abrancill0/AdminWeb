using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class EstatusInformeController : ApiController
    {
        public class ListaEstatus
        {
            public int i_estatus { get; set; }
            public string e_estatus { get; set; }
        }
        public List<ListaEstatus> PostInsertGasto()
        {
            SqlCommand comando = new SqlCommand("ObtieneEstatusInforme");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            List<ListaEstatus> lista = new List<ListaEstatus>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    ListaEstatus ent = new ListaEstatus
                    {
                        i_estatus = Convert.ToInt32(row["i_estatus"]),
                        e_estatus = Convert.ToString(row["e_estatus"]),
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
