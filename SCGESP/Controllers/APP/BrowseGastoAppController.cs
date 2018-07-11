using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{

    public class BrowseGastoAppController : ApiController
    {
        public class ParametrosGastoInforme
        {
            public int idproyecto { get; set; }
            public int idinforme { get; set; }
            public int idempresa { get; set; }
        }

        public class ObtieneInformeResult
        {
            public int g_id { get; set; }
            public int g_idinforme { get; set; }
            public int g_idproyecto { get; set; }
            public int g_idgorigen { get; set; }
            public string g_ugasto { get; set; }
            public string g_concepto { get; set; }
            public string g_negocio { get; set; }
            public string g_formapago { get; set; }
            public int g_categoria { get; set; }
            public double g_total { get; set; }
            public string g_observaciones { get; set; }
            public string g_comprobante { get; set; }
            public int g_estatus { get; set; }
            public string g_idapp { get; set; }
            public string g_dirxml { get; set; }
            public string g_dirpdf { get; set; }
            public string g_dirotros { get; set; }
            public string i_uresponsable { get; set; }
            public string g_autorizado { get; set; }
            public string g_masmenos { get; set; }
            public string g_conciliacionxml { get; set; }
            public string g_conciliacionbancos { get; set; }
            public string g_conciliacionconvenios { get; set; }
            public int g_contabilizar { get; set; }
            public int g_aplica { get; set; }
            public string g_rfc { get; set; }
            public string g_contacto { get; set; }
            public string g_telefono { get; set; }
            public string g_correo { get; set; }
            public string g_fgasto { get; set; }
            public string g_comentarioaut { get; set; }
            public int i_id { get; set; }
            public string g_hgasto { get; set; }
            public double MONTO { get; set; }
            public string g_nombreCategoria { get; set; }
            public double g_ivaCategoria { get; set; }
            public string formaPago { get; set; }
            public string efectivo { get; set; }
            public string cuenta { get; set; }
            public string c_nocuenta { get; set; }
            public string dirotros { get; set; }
            public int g_ncomensales { get; set; }
            public string g_nmbcomensales { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(ParametrosGastoInforme Datos)
        {
            SqlCommand comando = new SqlCommand("browseGastosInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idproyecto", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idproyecto"].Value = Datos.idproyecto;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            //comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //ObtieneInformeResult items;

            List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

            if (DT.Rows.Count > 0)
            {


                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {

                    DateTime g_fgasto1 = Convert.ToDateTime(row["g_fgasto"]);
                    string Fecha = g_fgasto1.ToString("dd-MM-yyyy");

                    ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                        g_id = Convert.ToInt32(row["g_id"]),
                        g_idinforme = Convert.ToInt32(row["g_idinforme"]),
                        g_idproyecto = Convert.ToInt32(row["g_idproyecto"]),
                        g_idgorigen = Convert.ToInt32(row["g_idgorigen"]),
                        g_ugasto = Convert.ToString(row["g_ugasto"]),
                        g_concepto = Convert.ToString(row["g_concepto"]),
                        g_negocio = Convert.ToString(row["g_negocio"]),
                        g_formapago = Convert.ToString(row["g_formapago"]),
                        g_categoria = Convert.ToInt32(row["g_categoria"]),
                        g_total = Convert.ToDouble(row["g_total"]),
                        g_observaciones = Convert.ToString(row["g_observaciones"]),
                        g_comprobante = Convert.ToString(row["g_comprobante"]),
                        g_estatus = Convert.ToInt32(row["g_estatus"]),
                        g_idapp = Convert.ToString(row["g_idapp"]),
                        g_dirxml = Convert.ToString(row["g_dirxml"]),
                        g_dirpdf = Convert.ToString(row["g_dirpdf"]),
                        g_dirotros = Convert.ToString(row["g_dirotros"]),
                        i_uresponsable = Convert.ToString(row["i_uresponsable"]),
                        g_autorizado = Convert.ToString(row["g_autorizado"]),
                        g_masmenos = Convert.ToString(row["g_masmenos"]),
                        g_conciliacionbancos = Convert.ToString(row["g_conciliacionbancos"]),
                        g_contabilizar = Convert.ToInt32(row["g_contabilizar"]),
                        g_aplica = Convert.ToInt32(row["g_aplica"]),
                        g_rfc = Convert.ToString(row["g_rfc"]),
                        g_contacto = Convert.ToString(row["g_contacto"]),
                        g_telefono = Convert.ToString(row["g_telefono"]),
                        g_correo = Convert.ToString(row["g_correo"]),
                        g_fgasto = Fecha,
                        g_comentarioaut = Convert.ToString(row["g_comentarioaut"]),
                        g_hgasto = Convert.ToString(row["hgasto"]),
                        i_id = Convert.ToInt32(row["i_id"]),
                        MONTO = Convert.ToDouble(row["MONTO"]),
                        g_nombreCategoria = Convert.ToString(row["g_nombreCategoria"]),
                        g_ivaCategoria = Convert.ToDouble(row["g_ivaCategoria"]),
                        g_ncomensales = Convert.ToInt32(row["ncomensales"]),
                        g_nmbcomensales = Convert.ToString(row["nmbcomensales"])
                    };

                    lista.Add(ent);
                }

                return lista;
            }
            else
            {
                return lista;
            }
        }

    }
}
