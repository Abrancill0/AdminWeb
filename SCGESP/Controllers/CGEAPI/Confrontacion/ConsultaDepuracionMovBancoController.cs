using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaDepuracionMovBancoController : ApiController
    {
        public class ListResult
        {
            public int Id { get; set; }
            public string Tarjeta { get; set; }
            public string Tipomovimiento { get; set; }
            public string Banco { get; set; }
            public string Fmovimiento { get; set; }
            public string Observaciones { get; set; }
            public decimal Importe { get; set; }
            public string Referencia { get; set; }
            public string Nombre { get; set; }
            public string Embosado { get; set; }
            public string Nomina { get; set; }
            public int Idinforme { get; set; }
            public int Idgasto { get; set; }
            public string Concepto { get; set; }
            public string Negocio { get; set; }
            public decimal Total { get; set; }
            public decimal Valor { get; set; }
            public string Fgasto { get; set; }
            public string Formapago { get; set; }
            public string NombreCategoria { get; set; }
            public string Ugasto { get; set; }
            public string Ucreo { get; set; }
            public string Ninforme { get; set; }
            public string NmbInf { get; set; }
        }

        public class ParametrosMovBanco
        {
            public string FechaIni { get; set; }
            public string FechaFin { get; set; }
        }

        public IEnumerable<ListResult> Post(ParametrosMovBanco Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string FechaIni = Convert.ToDateTime(Datos.FechaIni).ToString("yyyy-MM-dd");
            string FechaFin = Convert.ToDateTime(Datos.FechaFin).ToString("yyyy-MM-dd");
            string consulta = "SELECT m_id, m_tarjeta, m_tipomovimiento, m_banco, m_fmovimiento, m_observaciones" +
                              ", m_importe, m_referencia, m_nombre, m_embosado, m_nomina" +
                              ", ISNULL(m_idinforme, 0) AS m_idinforme, ISNULL(m_idgasto, 0) AS m_idgasto" +
                              ", ISNULL(g_concepto, '') AS g_concepto, ISNULL(g_negocio, '') AS g_negocio" +
                              ", ISNULL(g_total, 0) AS g_total, ISNULL(g_valor, 0) AS g_valor" +
                              ", ISNULL(g_fgasto, '') AS g_fgasto, ISNULL(g_formapago, '') AS g_formapago" +
                              ", ISNULL(g_nombreCategoria, '') AS g_nombreCategoria" +
                              ", ISNULL(g_ugasto, '') AS g_ugasto, ISNULL(g_ucreo, '') AS g_ucreo" +
                              ", ISNULL(i_ninforme, '') AS i_ninforme, ISNULL(i_nmb, '') AS i_nmb " +
                          "FROM movbancarios m LEFT OUTER JOIN " +
                            "gastos g ON g.g_idinforme = m.m_idinforme AND g.g_id = m.m_idgasto LEFT OUTER JOIN " +
                            "informe i ON i.i_id = m.m_idinforme " +
                          "WHERE m_fmovimiento BETWEEN '" + FechaIni + "' AND '" + FechaFin + "'";

            DA = new SqlDataAdapter(consulta, Conexion);
            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    int RowId = Convert.ToInt32(row["m_id"]);
                    string RowTarjeta = Convert.ToString(row["m_tarjeta"]);
                    string RowTipomovimiento = Convert.ToString(row["m_tipomovimiento"]);
                    string RowBanco = Convert.ToString(row["m_banco"]);
                    string RowFmovimiento = Convert.ToDateTime(row["m_fmovimiento"]).ToString("dd/MM/yyyy");
                    string RowObservaciones = Convert.ToString(row["m_observaciones"]);
                    decimal RowImporte = Convert.ToDecimal(row["m_importe"]);
                    string RowReferencia = Convert.ToString(row["m_referencia"]);
                    string RowNombre = Convert.ToString(row["m_nombre"]);
                    string RowEmbosado = Convert.ToString(row["m_embosado"]);
                    string RowNomina = Convert.ToString(row["m_nomina"]);
                    int RowIdinforme = Convert.ToInt32(row["m_idinforme"]);
                    int RowIdgasto = Convert.ToInt32(row["m_idgasto"]);
                    string RowConcepto = Convert.ToString(row["g_concepto"]);
                    string RowNegocio = Convert.ToString(row["g_negocio"]);
                    decimal RowTotal = Convert.ToDecimal(row["g_total"]);
                    decimal RowValor = Convert.ToDecimal(row["g_valor"]);
                    string RowFgasto = RowTotal > 0 ? Convert.ToDateTime(row["g_fgasto"]).ToString("dd/MM/yyyy"): "";
                    string RowFormapago = Convert.ToString(row["g_formapago"]);
                    string RowNombreCategoria = Convert.ToString(row["g_nombreCategoria"]);
                    string RowUgasto = Convert.ToString(row["g_ugasto"]);
                    string RowUcreo = Convert.ToString(row["g_ucreo"]);
                    string RowNinforme = Convert.ToString(row["i_ninforme"]);
                    string RowNmbInf = Convert.ToString(row["i_nmb"]);
                    ListResult resultado = new ListResult
                    {
                        Id = RowId,
                        Tarjeta = RowTarjeta,
                        Tipomovimiento = RowTipomovimiento,
                        Banco = RowBanco,
                        Fmovimiento = RowFmovimiento,
                        Observaciones = RowObservaciones,
                        Importe = RowImporte,
                        Referencia = RowReferencia,
                        Nombre = RowNombre,
                        Embosado = RowEmbosado,
                        Nomina = RowNomina,
                        Idinforme = RowIdinforme,
                        Idgasto = RowIdgasto,
                        Concepto = RowConcepto,
                        Negocio = RowNegocio,
                        Total = RowTotal,
                        Valor = RowValor,
                        Fgasto = RowFgasto,
                        Formapago = RowFormapago,
                        NombreCategoria = RowNombreCategoria,
                        Ugasto = RowUgasto,
                        Ucreo = RowUcreo,
                        Ninforme = RowNinforme,
                        NmbInf = RowNmbInf
                    };
                    lista.Add(resultado);
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
