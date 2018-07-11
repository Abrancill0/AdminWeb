using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class GastosController : ApiController
    {
        public class ParametrosGastos
        {
            public int idinforme { get; set; }
            public int idproyecto { get; set; }
            public string fgasto { get; set; }
            public int ugasto { get; set; }
            public string concepto { get; set; }
            public string negocio { get; set; }
            public int formapago { get; set; }
            public int cuenta { get; set; }
            public double subtotal { get; set; }
            public double iva { get; set; }
            public double total { get; set; }
            public int ucreo { get; set; }
            public int comprobante { get; set; }
            public int estatus { get; set; }
            public string idapp { get; set; }
            public string dirxml { get; set; }
            public string dirpdf { get; set; }
            public string dirotros { get; set; }
            public string observaciones { get; set; }
            public string rfc { get; set; }
            public string contacto { get; set; }
            public string telefono { get; set; }
            public string correo { get; set; }
        }

        public string PostInsertGasto(ParametrosGastos Datos)
        {
            SqlCommand comando = new SqlCommand("InsertGasto");
            comando.CommandType = CommandType.StoredProcedure;
            
            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@fgasto", SqlDbType.Date);
            comando.Parameters.Add("@ugasto", SqlDbType.Int);
            comando.Parameters.Add("@concepto", SqlDbType.VarChar);
            comando.Parameters.Add("@negocio", SqlDbType.VarChar);
            comando.Parameters.Add("@formapago", SqlDbType.Int);
            comando.Parameters.Add("@cuenta", SqlDbType.Int);
            comando.Parameters.Add("@subtotal", SqlDbType.Float);
            comando.Parameters.Add("@iva", SqlDbType.Float);
            comando.Parameters.Add("@total", SqlDbType.Float);
            comando.Parameters.Add("@ucreo", SqlDbType.Int);
            comando.Parameters.Add("@comprobante", SqlDbType.Int);
            comando.Parameters.Add("@estatus", SqlDbType.Int);
            comando.Parameters.Add("@idapp", SqlDbType.VarChar);
            comando.Parameters.Add("@dirxml", SqlDbType.VarChar);
            comando.Parameters.Add("@dirpdf", SqlDbType.VarChar);
            comando.Parameters.Add("@dirotros", SqlDbType.VarChar);
            comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
            comando.Parameters.Add("@rfc", SqlDbType.VarChar);
            comando.Parameters.Add("@contacto", SqlDbType.VarChar);
            comando.Parameters.Add("@telefono", SqlDbType.VarChar);
            comando.Parameters.Add("@correo", SqlDbType.VarChar);

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@fgasto"].Value = Datos.fgasto;
            comando.Parameters["@ugasto"].Value = Datos.ugasto;
            comando.Parameters["@concepto"].Value = Datos.concepto;
            comando.Parameters["@negocio"].Value = Datos.negocio;
            comando.Parameters["@formapago"].Value = Datos.formapago;
            comando.Parameters["@cuenta"].Value = Datos.cuenta;
            comando.Parameters["@subtotal"].Value = Datos.subtotal;
            comando.Parameters["@iva"].Value = Datos.iva;
            comando.Parameters["@total"].Value = Datos.total;
            comando.Parameters["@ucreo"].Value = Datos.ucreo;
            comando.Parameters["@comprobante"].Value = Datos.comprobante;
            comando.Parameters["@estatus"].Value = Datos.estatus;
            comando.Parameters["@idapp"].Value = Datos.idapp;
            comando.Parameters["@dirxml"].Value = Datos.dirxml;
            comando.Parameters["@dirpdf"].Value = Datos.dirpdf;
            comando.Parameters["@dirotros"].Value = Datos.dirotros;
            comando.Parameters["@observaciones"].Value = Datos.observaciones;
            comando.Parameters["@rfc"].Value = Datos.rfc;
            comando.Parameters["@contacto"].Value = Datos.contacto;
            comando.Parameters["@telefono"].Value = Datos.telefono;
            comando.Parameters["@correo"].Value = Datos.correo;

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

                return "";
            }
            else
            {
                return null;
            }
        }

  
    }
}
