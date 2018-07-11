using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultarMovSinGastoController : ApiController
    {
        public class ListResult
        {
            public int IdMovBanco { get; set; }
            public string Descripcion { get; set; }
            public string Banco { get; set; }
            public string Tarjeta { get; set; }
            public string Fecha { get; set; }
            public decimal Importe { get; set; }
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public string FechaGasto { get; set; }
            public decimal ImporteGasto { get; set; }
        }

        public class ParametrosMovBanco
        {
            //datos gasto
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public string FGasto { get; set; }
            public string Tarjeta { get; set; }
            public decimal Importe { get; set; }
            //rangos fecha e importes
            public string RepDe { get; set; }
            public string RepA { get; set; }
            public decimal ImporteDe { get; set; }
            public decimal ImporteA { get; set; }
        }
        public IEnumerable<ListResult> Post(ParametrosMovBanco Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "DECLARE @importe decimal; " +
                              "SET @importe = " + Datos.Importe + "; " +
                              "DECLARE @fecha date; " +
                              "SET @fecha = '" + Datos.FGasto + "'; " +
                              "SELECT ISNULL(g_idmovbanco,0) AS idmovban INTO #idmovbangastos FROM gastos WHERE ISNULL(g_idmovbanco,0) != 0 " +
                              "SELECT * FROM( " +
                              "SELECT * , (1 - (difimporte / @importe)) * 100 AS ppimporte, " +
                              "@importe AS importebuscado , (1 / diffecha) *100 AS ppfecha , @fecha as fechabuscada " +
                              "FROM (" +
                              "SELECT m_id AS idmovbanco, m_tarjeta AS tarjeta " +
                              ", m_banco AS banco, m_fmovimiento AS fecha " +
                              ", m_observaciones AS observaciones, m_importe AS importe " +
                              ", ISNULL(m_referencia, '') AS referencia " +
                              ", IIF(ABS(@importe) > ABS(m_importe), ABS(@importe) - ABS(m_importe), ABS(m_importe) - ABS(@importe)) AS difimporte " +
                              ", CAST(IIF(m_fmovimiento > @fecha, DATEDIFF(DAY, @fecha, m_fmovimiento), DATEDIFF(DAY, m_fmovimiento, @fecha)) AS decimal) as diffecha " +
                              "FROM movbancarios " +
                              "WHERE m_id NOT IN (SELECT idmovban FROM #idmovbangastos) " +
                              "AND LOWER(m_tipomovimiento) = 'consumo' " +
                              "AND (ISNULL(m_idinforme, 0) = 0 AND ISNULL(m_idgasto, 0) = 0) " +
                              "AND m_tarjeta = '" + Datos.Tarjeta + "' " +
                              "AND (m_fmovimiento BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "') " +
                              "AND (m_importe BETWEEN " + Datos.ImporteDe + " AND " + Datos.ImporteA + ") " +                              
                              ") DATOS1 " +
                              ") DATOS2 " +
                              "ORDER BY ppimporte DESC, ppfecha DESC";
            
            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    int RowIdMovBanco = Convert.ToInt32(row["idmovbanco"]);
                    string RowDescripcion = Convert.ToString(row["observaciones"]);
                    string RowBanco = Convert.ToString(row["banco"]);
                    string RowTarjeta = Convert.ToString(row["tarjeta"]);
                    string RowFecha = Convert.ToDateTime(row["fecha"]).ToString("dd/MM/yyyy");
                    decimal RowImporte = Convert.ToDecimal(row["importe"]);

                    ListResult ent = new ListResult
                    {
                        IdMovBanco = RowIdMovBanco,
                        Descripcion = RowDescripcion,
                        Banco = RowBanco,
                        Tarjeta = RowTarjeta,
                        Fecha = RowFecha,
                        Importe = RowImporte,
                        IdInforme = Datos.IdInforme,
                        IdGasto = Datos.IdGasto,
                        FechaGasto = Convert.ToDateTime(Datos.FGasto).ToString("dd/MM/yyyy"),
                        ImporteGasto = Datos.Importe
                    };
                    lista.Add(ent);
                }
                return lista;
            }
            else {
                return null;
            }
            
        }
    }
}
