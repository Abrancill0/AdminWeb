using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaUsuariosConInformeController : ApiController
    {
        public class ListResult
        {
            public string Usuario { get; set; }
            public string Nombre { get; set; }
        }
        public IEnumerable<ListResult> Post()
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "SELECT DISTINCT i_uresponsable AS usuario, i_nombreresponsabe AS nombre " +
                              "FROM informe " +
                              "ORDER BY i_nombreresponsabe ASC";


            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    string RowUsuario = Convert.ToString(row["usuario"]);
                    string RowNombre = Convert.ToString(row["nombre"]);
                    ListResult ent = new ListResult
                    {
                        Usuario = RowUsuario,
                        Nombre = RowNombre
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
