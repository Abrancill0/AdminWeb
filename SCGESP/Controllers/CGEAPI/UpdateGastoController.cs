using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class UpdateGastoController : ApiController
    {
        public class ParametrosGastos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
            public string fgasto { get; set; }
            public string hgasto { get; set; }
            public string concepto { get; set; }
            public string negocio { get; set; }
            public string formapago { get; set; }
            public int categoria { get; set; }
            public double total { get; set; }
            public string nombreCategoria { get; set; }
            public double ivaCategoria { get; set; }
            public string observaciones { get; set; }

            public int ncomensales { get; set; }
            public string nmbcomensales { get; set; }
            public double importecomprobar { get; set; }
            public double importenodeducible { get; set; }
            public double importereembolsable { get; set; }
            public double importenoreembolsable { get; set; }
            public double importenoaceptable { get; set; }

        }

        public string PostInsertGasto(ParametrosGastos Datos)
        {
            SqlCommand comando = new SqlCommand("UpdateGasto");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@fgasto", SqlDbType.Date);
            comando.Parameters.Add("@hgasto", SqlDbType.VarChar); 
            comando.Parameters.Add("@concepto", SqlDbType.VarChar);
            comando.Parameters.Add("@negocio", SqlDbType.VarChar);
            comando.Parameters.Add("@formapago", SqlDbType.VarChar);
            comando.Parameters.Add("@categoria", SqlDbType.Int);
            comando.Parameters.Add("@total", SqlDbType.Float);
            comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
            comando.Parameters.Add("@nombreCategoria", SqlDbType.VarChar);
            comando.Parameters.Add("@ivaCategoria", SqlDbType.Float);

            comando.Parameters.Add("@ncomensales", SqlDbType.Int);
            comando.Parameters.Add("@nmbcomensales", SqlDbType.VarChar);
            comando.Parameters.Add("@deducible", SqlDbType.Int);
            comando.Parameters.Add("@importenodeducible", SqlDbType.Float);
            comando.Parameters.Add("@importereembolsable", SqlDbType.Float);
            comando.Parameters.Add("@importenoreembolsable", SqlDbType.Float);
            comando.Parameters.Add("@importenoaceptable", SqlDbType.Float);

            DateTime Fecha;

            string day = Datos.fgasto.Substring(0, 2);
            string month = Datos.fgasto.Substring(3, 2);
            string year = Datos.fgasto.Substring(6, 4);

            try
            {
                Fecha = Convert.ToDateTime(year + "-" + month + "-" + day);
            }
            catch (Exception)
            {
                Fecha = Convert.ToDateTime(day + "-" + month + "-" + year);

            }


            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.id;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@fgasto"].Value = Fecha;

            comando.Parameters["@ncomensales"].Value = Datos.ncomensales;
            comando.Parameters["@nmbcomensales"].Value = Datos.nmbcomensales != null ? Datos.nmbcomensales : "";
            comando.Parameters["@deducible"].Value = Datos.importenodeducible == 0 ? 1 : 0;
            comando.Parameters["@importenodeducible"].Value = Datos.importenodeducible;
            comando.Parameters["@importereembolsable"].Value = Datos.importereembolsable;
            comando.Parameters["@importenoreembolsable"].Value = Datos.importenoreembolsable;
            comando.Parameters["@importenoaceptable"].Value = Datos.importenoaceptable;

            string hora = "";
            if (Datos.hgasto != null)
            {
                hora = Datos.hgasto;
            }

            comando.Parameters["@hgasto"].Value = hora;
            comando.Parameters["@concepto"].Value = Datos.concepto;
            comando.Parameters["@negocio"].Value = Datos.negocio;
            comando.Parameters["@formapago"].Value = Datos.formapago;
            comando.Parameters["@categoria"].Value = Datos.categoria;
            comando.Parameters["@total"].Value = Datos.total;
            comando.Parameters["@nombreCategoria"].Value = Datos.nombreCategoria;
            comando.Parameters["@ivaCategoria"].Value = Datos.ivaCategoria;

            string Obs = "";
            if (Datos.observaciones !=  null)
            {
                Obs = Datos.observaciones;
            }
            comando.Parameters["@observaciones"].Value = Obs;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);


            if (DT.Rows.Count > 0)
            {
                DataRow row = DT.Rows[0];

               var idinforme = row[2].ToString();

                return idinforme;
            }
            else
            {
                return null;
            }
        }


    }
}
