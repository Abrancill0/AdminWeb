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
    public class ObtenerTipoInformeController : ApiController
    {
        public class ListResult
        {
            public int c_id { get; set; }
            public string c_clave { get; set; }
            public string c_nmb { get; set; }
        }

        public class datos
        {
            public int idempresa { get; set; }
        }

        public IEnumerable<ListResult> Post(datos dato)
        {
        //    SqlDataAdapter DA;
        //    DataTable DT = new DataTable();

        //    SqlConnection Conexion = new SqlConnection();
        //    Conexion.ConnectionString = VariablesGlobales.CadenaConexion;
          
        //    string consulta = "SELECT c_id, c_clave, c_nmb FROM cat_informes" +
        //                      " WHERE c_idempresa = " + dato.idempresa  + " ORDER BY c_clave, c_nmb";

        //    DA = new SqlDataAdapter(consulta, Conexion);
        //    DA.Fill(DT);


        //    ListResult[] items;

        //    if (DT.Rows.Count > 0)
        //    {
        //        DataRow row = DT.Rows[0];

        //        items = new ListResult[]
        //        {
        //           new ListResult{c_id = Convert.ToInt32(row["c_id"]),
        //                          c_clave = Convert.ToString(row["c_clave"]),
        //                          c_nmb = Convert.ToString(row["c_nmb"])
        //                         }
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
