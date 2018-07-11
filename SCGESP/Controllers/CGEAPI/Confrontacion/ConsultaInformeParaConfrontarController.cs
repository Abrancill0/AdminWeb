using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConsultaInformeParaConfrontarController : ApiController
    {
        public class ListResult
        {
            public int IdInforme { get; set; }
            public string NmbInforme { get; set; }
            public int NoInforme { get; set; }
            public int IdGasto { get; set; }
            public string Concepto { get; set; }
            public string Negocio { get; set; }
            public string Categoria { get; set; }
            public string FormaPago { get; set; }
            public string FechaGasto { get; set; }
            public string HoraGasto { get; set; }
            public decimal Monto { get; set; }
            public string Observaciones { get; set; }
            public int EnBanco { get; set; }
            public int IdMovBanco { get; set; }
            public string Banco { get; set; }
            public string Tarjeta { get; set; }
            public string FechaMovimiento { get; set; }
            public decimal ImporteMovimiento { get; set; }
            public string ObservacionesMovimiento { get; set; }
            public int IdRequisicion { get; set; }
            public decimal ImporteRequisicion { get; set; }
        }
        public class ParametrosInforme
        {
            public string IdInforme { get; set; }
        }
        public IEnumerable<ListResult> Post(ParametrosInforme Datos)
        {
            SqlCommand comando = new SqlCommand("ConfrontarInformeVsMovBanco")
            {
                CommandType = CommandType.StoredProcedure
            };
            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.IdInforme;
            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();

            DA.Fill(DT);

            List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    int RowIdInforme = Convert.ToInt32(row["idinforme"]);
                    int RowNoInforme = Convert.ToInt32(row["ninforme"]);
                    string RowNmbInforme = Convert.ToString(row["nmbinforme"]);
                    int RowIdGasto = Convert.ToInt32(row["idgasto"]);
                    string RowConcepto = Convert.ToString(row["concepto"]);
                    string RowNegocio = Convert.ToString(row["negocio"]);
                    string RowFgasto = Convert.ToDateTime(row["fgasto"]).ToString("dd/MM/yyyy");
                    string RowHgasto = Convert.ToString(row["hgasto"]);
                    string RowFormaPago = Convert.ToString(row["formapago"]);
                    decimal Monto1 = Convert.ToDecimal(row["total"]);
                    decimal Monto2 = Convert.ToDecimal(row["valor"]);
                    decimal RowMonto = Monto2 != Monto1 && Monto2 > 0 ? Monto2 : Monto1;
                    string RowCategoria = Convert.ToString(row["categoria"]);
                    string RowObservaciones = Convert.ToString(row["observaciones"]);

                    int RowIdRequisicion = Convert.ToInt32(row["idrequisicion"]);
                    decimal RowImporteRequisicion = Convert.ToDecimal(row["montorequisicion"]);

                    int RowIdMovBanco = 0;
                    int RowEnBanco = 0;
                    string RowBanco = "";
                    string RowTarjeta = "";
                    string RowFMovimiento = "";
                    decimal RowImporteMovimiento = 0;
                    string RowObservacionesMovimiento = "";

                    try {
                        RowIdMovBanco = Convert.ToInt32(row["midmovbanco"]);
                        if (RowIdMovBanco > 0) {
                            RowEnBanco = RowIdMovBanco > 0 ? 1 : 0;
                            RowBanco = Convert.ToString(row["banco"]);
                            RowTarjeta = Convert.ToString(row["tarjeta"]);
                            RowFMovimiento = Convert.ToDateTime(row["fmovimiento"]).ToString("dd/MM/yyyy");
                            RowImporteMovimiento = Convert.ToDecimal(row["importe"]);
                            RowObservacionesMovimiento = Convert.ToString(row["mobservaciones"]);
                        }
                    } catch (Exception ex) {
                        var error = ex;
                    }

                    ListResult ent = new ListResult
                    {
                        IdInforme = RowIdInforme,
                        NoInforme = RowNoInforme,
                        NmbInforme = RowNmbInforme,
                        IdGasto = RowIdGasto,
                        Concepto = RowConcepto,
                        Negocio = RowNegocio,
                        FormaPago = RowFormaPago,
                        FechaGasto = RowFgasto,
                        HoraGasto = RowHgasto,
                        Monto = RowMonto,
                        Categoria = RowCategoria,
                        Observaciones = RowObservaciones,
                        EnBanco = RowEnBanco,
                        IdMovBanco = RowIdMovBanco,
                        Banco = RowBanco,
                        Tarjeta = RowTarjeta,
                        FechaMovimiento = RowFMovimiento,
                        ImporteMovimiento = RowImporteMovimiento,
                        ObservacionesMovimiento = RowObservacionesMovimiento,
                        IdRequisicion = RowIdRequisicion,
                        ImporteRequisicion = RowImporteRequisicion
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
