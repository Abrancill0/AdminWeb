using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class UpdateComensalesGastoController : ApiController
    {
        public class ListResult
        {
            public bool ComensalesOK { get; set; }
            public string Descripcion { get; set; }
        }
        public class Parametros
        {
            public int IdInforme { get; set; }
            public int IdGasto { get; set; }
            public int NComensales { get; set; }
            public string Nmbcomensales { get; set; }
        }
        public ListResult Post(Parametros Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "UPDATE gastos SET " + 
                              " g_ncomensales = " + Datos.NComensales + ", " + 
                              " g_nmbcomensales = '" + Datos.Nmbcomensales + "' " + 
                              " WHERE g_idinforme = " + Datos.IdInforme + " AND g_id = " + Datos.IdGasto + "; ";
            try
            {
                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);
                ListResult resultado = new ListResult
                {
                    ComensalesOK = true,
                    Descripcion = "Comensales Guardados."
                };
                return resultado;
            }
            catch (Exception err)
            {
                var error = Convert.ToString(err);
                ListResult resultado = new ListResult
                {
                    ComensalesOK = false,
                    Descripcion = "Error al guardar comensales. " + error
                };
                return resultado;
            }
        }
    }
}
