using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaTipoAdminAutorizadorController : ApiController
    {
        public class ListResult
        {
            public string UAutoriza { get; set; }
            public string IdEmpleado { get; set; }
            public string Nombre { get; set; }
            public int Administrador { get; set; }
            public int Suplente { get; set; }
        }

        public IEnumerable<ListResult> Post()
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "SELECT uautoriza, idempleado, administrador, Nombre FROM AutorizaOpcional";


            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    string RowUAutoriza = Convert.ToString(row["uautoriza"]);
                    string RowIdEmpleado = Convert.ToString(row["idempleado"]);
                    string RowNombre = Convert.ToString(row["Nombre"]);
                    int RowAdministrador = Convert.ToInt16(row["administrador"]);
                    int RowSuplente = RowAdministrador == 1 ? 0 : 1;

                    ListResult ent = new ListResult
                    {
                        UAutoriza = RowUAutoriza,
                        IdEmpleado = RowIdEmpleado,
                        Nombre = RowNombre,
                        Administrador = RowAdministrador,
                        Suplente = RowSuplente
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
