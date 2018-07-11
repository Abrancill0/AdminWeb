using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaMovBancariosParaConfrontarController : ApiController
    {
        public class ListResult
        {
            public int IdMovBanco { get; set; }
            public string Descripcion { get; set; }
            public string Banco { get; set; }
            public string Tarjeta { get; set; }
            public string Fecha { get; set; }
            public decimal Importe { get; set; }
            public int Conciliacion { get; set; }
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
        }

        public class Parametros
        {
            public int IdInforme { get; set; }
            public string Tarjeta { get; set; }
            //rangos fecha e importes
            public string RepDe { get; set; }
            public string RepA { get; set; }
            public decimal ImporteDe { get; set; }
            public decimal ImporteA { get; set; }
        }
        public IEnumerable<ListResult> Post(Parametros Datos)
        {
            if(Datos.IdInforme > 0)
                ConfrontaMovBancoVsInforme(Datos.IdInforme);

            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string IdInforme = Datos.IdInforme == 0 ? "" : Datos.IdInforme.ToString();
            string condicionInforme = IdInforme == "" ? "" : (" OR (m_idinforme = " + IdInforme + " OR m_idinfcarga = " + IdInforme + " ) ");
            string consulta = "SELECT " +
                "m_id AS idmovbanco, m_tarjeta AS tarjeta, m_banco AS banco, m_fmovimiento AS fecha, " +
                "m_observaciones AS observaciones, m_importe AS importe, " +
                "ISNULL(m_referencia, '') AS referencia," +
                "IIF(ISNULL(m_idinforme, 0) = 0 AND ISNULL(m_idgasto, 0) = 0, 0, 1) AS conciliacion," +
                "ISNULL(m_idinforme, 0) AS idinforme," +
                "ISNULL(m_idgasto, 0) AS idgasto " +
                "FROM movbancarios mb " +
                "WHERE (LOWER(m_tipomovimiento) = 'consumo' " +
                "AND (ISNULL(m_idinforme, 0) = 0 AND ISNULL(m_idgasto, 0) = 0) " +
                "AND m_tarjeta = '" + Datos.Tarjeta + "' " +
                "AND (m_fmovimiento BETWEEN '" + Datos.RepDe + "' AND '" + Datos.RepA + "') " +
                "AND (m_importe BETWEEN '" + Datos.ImporteDe + "' AND '" + Datos.ImporteA + "')) " +
                condicionInforme +
                " ORDER BY m_fmovimiento ASC;";

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

                    int RowConciliacion = Convert.ToInt16(row["conciliacion"]);
                    int RowIdInforme = Convert.ToInt16(row["idinforme"]);
                    int RowIdGasto = Convert.ToInt16(row["idgasto"]);

                    ListResult ent = new ListResult
                    {
                        IdMovBanco = RowIdMovBanco,
                        Descripcion = RowDescripcion,
                        Banco = RowBanco,
                        Tarjeta = RowTarjeta,
                        Fecha = RowFecha,
                        Importe = RowImporte,
                        IdInforme = RowIdInforme,
                        IdGasto = RowIdGasto,
                        Conciliacion = RowConciliacion
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

        public static void ConfrontaMovBancoVsInforme(int IdInforme) {
            try
            {
                SqlCommand comando = new SqlCommand("ConfrontarInformeVsMovBanco")
                {
                    CommandType = CommandType.StoredProcedure
                };
                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                //Asignacion de valores a parametros
                comando.Parameters["@idinforme"].Value = IdInforme;
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();

                DA.Fill(DT);
            }
            catch (Exception err) {
                string error = err.ToString();
            }
        }
    }
}
