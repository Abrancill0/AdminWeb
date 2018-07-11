using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class ConsultaConfVoBoController : ApiController
    {
        public class ListResult
        {
            public int Id { get; set; }
            public string Usuario { get; set; }
            public int ValorDefault { get; set; }
            public int ChkBloqueado { get; set; }
            public int ValidarImporte { get; set; }
            public decimal ImporteMayorQue { get; set; }
        }
        
        public IEnumerable<ListResult> Post()
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "SELECT c_id, c_usuario, c_accion, c_valor_default, c_chk_bloqueado, c_validar_importe, c_importe_mayor_que " +
                              "FROM configuracion WHERE LOWER(c_accion) = 'vobo'; ";


            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    ListResult ent = new ListResult
                    {
                        Id = Convert.ToInt16(row["c_id"]),
                        Usuario = Convert.ToString(row["c_usuario"]).Trim(),
                        ValorDefault = Convert.ToInt16(row["c_valor_default"]),
                        ChkBloqueado = Convert.ToInt16(row["c_chk_bloqueado"]),
                        ValidarImporte = Convert.ToInt16(row["c_validar_importe"]),
                        ImporteMayorQue = Convert.ToInt16(row["c_importe_mayor_que"])
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
