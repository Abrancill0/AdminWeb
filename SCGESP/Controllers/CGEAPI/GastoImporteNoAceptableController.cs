using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class GastoImporteNoAceptableController : ApiController
    {
        public class Parametros
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public double ImporteAceptable { get; set; }
            public double ImporteNoAceptable { get; set; }
            public double ImporteNoDeducible { get; set; }
            public double Monto { get; set; }
        }
        public string Post(Parametros Datos) {
            DataTable DT;
            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };

            string query = "EXEC GastoImpNoAceptableNoDeducible " + 
                Datos.IdInforme + " , " +
                Datos.IdGasto + ", " +
                Datos.ImporteAceptable + ", " +
                Datos.ImporteNoAceptable + ", " +
                Datos.ImporteNoDeducible + ", " +
                Datos.Monto + ";";
            try
            {
                DT = EjecutarQuery(query, Conexion);
                return "OK";
            }
            catch (Exception err) {
                var error = Convert.ToString(err);
                return "Error: " + error;
            }
            
        }
        public DataTable EjecutarQuery(string query, SqlConnection Conexion)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();
            DA = new SqlDataAdapter(query, Conexion);
            DA.Fill(DT);
            return DT;
        }
    }
}
