using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class GuardarMovBancoController : ApiController
    {
        public class ListResult
        {
            public bool GuardadoOk { get; set; }
            public string Descripcion { get; set; }
        }

        public class ParametrosMovBanco
        {
            public string Tarjeta { get; set; }
            public string Tipo { get; set; }
            public string Fecha { get; set; }
            public string Descripcion { get; set; }
            public decimal Importe { get; set; }
            public string Banco { get; set; }
            public int Duplicado { get; set; }
            public int IdMovimiento { get; set; }
            public string Nombre { get; set; }
            public string Nomina { get; set; }
            public string Embosado { get; set; }
            public string Usuario { get; set; }
            public string IdInforme { get; set; }
        }

        public ListResult Post(ParametrosMovBanco Datos)
        {
            SqlCommand comando = new SqlCommand("InsertMBancos")
            {
                CommandType = CommandType.StoredProcedure
            };

            //Declaracion de parametros
            comando.Parameters.Add("@tarjeta", SqlDbType.VarChar);
            comando.Parameters.Add("@tipomovimiento", SqlDbType.VarChar);
            comando.Parameters.Add("@banco", SqlDbType.VarChar);
            comando.Parameters.Add("@fmovimiento", SqlDbType.Date);
            comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
            comando.Parameters.Add("@importe", SqlDbType.Decimal);
            comando.Parameters.Add("@referencia", SqlDbType.VarChar);
            comando.Parameters.Add("@nombre", SqlDbType.VarChar);
            comando.Parameters.Add("@embosado", SqlDbType.VarChar);
            comando.Parameters.Add("@nomina", SqlDbType.VarChar);
            comando.Parameters.Add("@ucrea", SqlDbType.VarChar);
            comando.Parameters.Add("@existe", SqlDbType.Int);
            comando.Parameters.Add("@idmov", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@tarjeta"].Value = Datos.Tarjeta;
            comando.Parameters["@tipomovimiento"].Value = Datos.Tipo;
            comando.Parameters["@banco"].Value = Datos.Banco;
            comando.Parameters["@fmovimiento"].Value = Convert.ToDateTime(Datos.Fecha);
            comando.Parameters["@observaciones"].Value = Datos.Descripcion;
            comando.Parameters["@importe"].Value = Datos.Importe;
            comando.Parameters["@referencia"].Value = "";
            comando.Parameters["@nombre"].Value = Datos.Nombre;
            comando.Parameters["@embosado"].Value = Datos.Embosado;
            comando.Parameters["@nomina"].Value = Datos.Nomina;
            comando.Parameters["@ucrea"].Value = Datos.Usuario;
            comando.Parameters["@existe"].Value = Datos.Duplicado;
            comando.Parameters["@idmov"].Value = Datos.IdMovimiento;
            comando.Parameters["@idinforme"].Value = Datos.IdInforme;

            //Ejecutar comando
            bool exito = false;
            string mensaje = "";
            try
            {
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                comando.ExecuteNonQuery();
                comando.Connection.Close();
                exito = true;
                mensaje = "Movimiento guardado";
            }
            catch (Exception ex) {
                var error = ex;
                exito = false;
                mensaje = "Movimiento guardado. " + Convert.ToString(error);
            }

            ListResult lista = new ListResult
            {
                GuardadoOk = exito,
                Descripcion = mensaje
            };

            return lista;
        }

    }
}
