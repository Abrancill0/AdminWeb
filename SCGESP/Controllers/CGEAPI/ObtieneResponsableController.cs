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
    public class ObtieneResponsableController : ApiController
    {
        public class ListResult
        {
            public int u_id { get; set; }
            public string u_usuario { get; set; }
            public string nombre { get; set; }
            public string uactivo { get; set; }
        }

        public class datos
        {
            public int idempresa { get; set; }
            public string uactivo { get; set; }
        }

        public IEnumerable<ListResult> Post(datos dato)
        {
        //    SqlDataAdapter DA;
        //    DataTable DT = new DataTable();

        //    SqlConnection Conexion = new SqlConnection();
        //    Conexion.ConnectionString = VariablesGlobales.CadenaConexion;
        //    //string consulta = "SELECT DISTINCT i_uresponsable, u_usuario, u_nmb + ' ' + u_apm AS responsable FROM informe INNER JOIN " +
        //    //                  " usuarios ON u_id = i_uresponsable AND u_default = 0 " +
        //    //                  " WHERE c_idempresa =" + idempresa + " AND i_estatus NOT IN(5, 8) " +
        //    //                  " GROUP BY i_uresponsable, u_usuario, u_nmb, u_apm " +
        //    //                  " ORDER BY u_nmb ";

        //    string consulta = "SELECT u_id, u_usuario, (u_nmb + ' ' + u_apm) AS nombre, '" + dato.uactivo + "' as uactivo" +
        //                       " FROM usuarios WHERE u_idempresa =" + dato.idempresa + " AND u_default = 0";


        //    DA = new SqlDataAdapter(consulta, Conexion);
        //    DA.Fill(DT);


        //    ListResult[] items;

        //    if (DT.Rows.Count > 0)
        //    {
        //        DataRow row = DT.Rows[0];

        //        items = new ListResult[]
        //        {
        //           new ListResult{u_id = Convert.ToInt32(row["u_id"]),
        //                                    u_usuario = Convert.ToString(row["u_usuario"]),
        //                                    nombre = Convert.ToString(row["nombre"]),
        //                                    uactivo = Convert.ToString(row["uactivo"]),
        //                                   }
        //        };
        //        return items;

        //    }
        //    else
        //    {
                return null;
        //    }
       }

    }
}
