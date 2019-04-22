using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class RechazarInformeController : ApiController
    {

        public class datos
        {
            public int idinforme { get; set; }
            public string comentarioaut { get; set; }
            public string usuario { get; set; }
        }


        public string Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.usuario);

            SqlCommand comando = new SqlCommand("RegresarInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@comentarioaut", SqlDbType.VarChar);
            comando.Parameters.Add("@usuario", SqlDbType.VarChar);
            

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@comentarioaut"].Value = Datos.comentarioaut;
            comando.Parameters["@usuario"].Value = UsuarioDesencripta;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //ObtieneInformeResult items;
            
            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    string mensaje = Convert.ToString(row["msn"]);
                    string titulo = Convert.ToString(row["titulo"]);
                    string usuarioResponsable = Convert.ToString(row["usuarioResponsable"]);
                    string autorizador = Convert.ToString(row["autorizador"]);

                    if (autorizador != "")
                    {
                        EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", autorizador, "", titulo, mensaje, 0);
                    }
                    else {
                        EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", usuarioResponsable, "", titulo, mensaje, 0);
                    }
                    

                }
            }

            return "";
        }
    }
}
