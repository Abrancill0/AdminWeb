using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class UpdateCategoriaGastoAjusteController : ApiController
    {
        public class Parametros
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public int IdCategoria { get; set; }
            public string Categoria { get; set; }
            public double IvaCategoria { get; set; }
            public double TGastado { get; set; }
            public double TComprobar { get; set; }
            public string DirXML { get; set; }
            public int TipoAjuste { get; set; }
        }
        public class ListResult
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public bool ActualizadaOk { get; set; }
            public string Descripcion { get; set; }
        }
        public ListResult PostUpdateCategoriaGastoAjuste(Parametros Datos)
        {

            ListResult resultado = new ListResult();
            
            try
            {
                SqlDataAdapter DA;
                DataTable DT = new DataTable();

                SqlConnection Conexion = new SqlConnection
                {
                    ConnectionString = VariablesGlobales.CadenaConexion
                };
                string consulta = "UPDATE gastos " +
                                  "SET g_categoria = " + Datos.IdCategoria + ", " +
                                  "g_nombreCategoria = '" + Datos.Categoria + "', " +
                                  "g_ivaCategoria = " + Datos.IvaCategoria + " " +
                                  "WHERE g_idinforme = " + Datos.IdInforme + " AND g_id = " + Datos.IdGasto + "; ";

                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);

                resultado.ActualizadaOk = true;
                resultado.Descripcion = (Datos.TipoAjuste == 1 ? "La categoria de la propina se actualizo a: " : "La categoria del comprobante (CFDI) se actualizo a: ") + Datos.Categoria + ".";
                resultado.IdInforme = Datos.IdInforme;
                resultado.IdGasto = Datos.IdGasto;
            }
            catch (Exception err)
            {
                string error = "Error: " + err.ToString();
                resultado.ActualizadaOk = false;
                resultado.Descripcion = (Datos.TipoAjuste == 1 ? "La categoria de la propina no se actualizo. " : "La categoria del comprobante (CFDI) no se actualizo. ") + error;
                resultado.IdInforme = Datos.IdInforme;
                resultado.IdGasto = Datos.IdGasto;
            }
            return resultado;
        }
    }
}
