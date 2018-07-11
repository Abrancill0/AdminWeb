using System;
using SCGESP.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI.Autorizaciones
{
    public class EnviovoboController : ApiController
    {
        public class ParametrosVOBO
        {
            public string @usuarioActual { get; set; }
            public string @usuariovobo { get; set; }
            public string @idinforme { get; set; }
        }


        public string Post(ParametrosVOBO Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("EnviaVoBo");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@usuarioActual", SqlDbType.VarChar);
                comando.Parameters.Add("@usuariovobo", SqlDbType.VarChar);
                comando.Parameters.Add("@idinforme", SqlDbType.VarChar);

                string ususariodesencripta = Seguridad.DesEncriptar(Datos.usuarioActual);

                //Asignacion de valores a parametros
                comando.Parameters["@usuarioActual"].Value = ususariodesencripta;
                comando.Parameters["@usuariovobo"].Value = Datos.usuariovobo;
                comando.Parameters["@idinforme"].Value = Datos.idinforme;


                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);


                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        string mensaje = Convert.ToString(row["msn"]);
                        string titulo = Convert.ToString(row["titulo"]);
                        string usuarioActual = Convert.ToString(row["usuarioActual"]);
                        string usuariovobo = Convert.ToString(row["usuariovobo"]);
                        string correo = Convert.ToString(row["correo"]);

                        EnvioCorreosELE.Envio(usuarioActual, correo, "", usuariovobo, "", titulo, mensaje, 0);

                    }

                    return "OK";
                }
                else
                {
                    return "Error";
                }
            }
            catch (System.Exception ex)
            {

                return ex.ToString();
            }
          

          
        }


    }
}
