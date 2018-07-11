using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class SaveDatosAdicionalesGastoController : ApiController
    {
        public class datos
        {
            public int idgasto { get; set; }
            public int idinforme { get; set; }
            public int idproyecto { get; set; }
            public string rfc { get; set; }
            public string contacto { get; set; }
            public string telefono { get; set; }
            public string correo { get; set; }
            public int ncomensales { get; set; }
            public string nmbcomensales { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string mensaje { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(datos Datos)
        {
            //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.ugasto);

            SqlCommand comando = new SqlCommand("SaveDatosAdicionalesGasto");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@id", SqlDbType.Int);
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@idproyecto", SqlDbType.Int);
            comando.Parameters.Add("@rfc", SqlDbType.VarChar);
            comando.Parameters.Add("@contacto", SqlDbType.VarChar);
            comando.Parameters.Add("@telefono", SqlDbType.VarChar);
            comando.Parameters.Add("@correo", SqlDbType.VarChar);
            comando.Parameters.Add("@ncomensales", SqlDbType.Int);
            comando.Parameters.Add("@nmbcomensales", SqlDbType.VarChar);

            string RFCValida;
            if (Datos.rfc is null)
            {
                RFCValida = "";
            }
            else
            {
                RFCValida = Datos.rfc;
            }

            string TelefonoValida;
            if (Datos.telefono is null)
            {
                TelefonoValida = "";
            }
            else
            {
                TelefonoValida = Datos.telefono;
            }

            string correoValida;
            if (Datos.correo is null)
            {
                correoValida = "";
            }
            else
            {
                correoValida = Datos.correo;
            }
            
            //Asignacion de valores a parametros
            comando.Parameters["@id"].Value = Datos.idgasto;
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@idproyecto"].Value = Datos.idproyecto;
            comando.Parameters["@rfc"].Value = RFCValida;
            comando.Parameters["@contacto"].Value = Datos.contacto == null ? "" : Datos.contacto;
            comando.Parameters["@telefono"].Value = TelefonoValida;
            comando.Parameters["@correo"].Value = correoValida;

            comando.Parameters["@ncomensales"].Value = Datos.ncomensales;
            comando.Parameters["@nmbcomensales"].Value = Datos.nmbcomensales;
            
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
                    ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                       // mensaje = Convert.ToString(row["MSN"]),
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

